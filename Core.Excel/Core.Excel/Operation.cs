using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Core.Excel
{
    /// <summary>
    /// Perform excel operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Operation<T> where T : ICollectionToExcel
    {
        readonly string _sheetName;
        readonly string _startingCell;

        /// <summary>
        /// Initialize operation
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="startingCell"></param>
        public Operation(string sheetName = "", string startingCell = "")
        {
            _sheetName = sheetName;
            _startingCell = startingCell;
        }

        /// <summary>
        /// Gets stream of excel from collection
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public Stream GetExcelFromCollection(IEnumerable<T> collection)
        {
            Stream stream = new MemoryStream();

            if (collection.Any() && !string.IsNullOrEmpty(_sheetName))
            {
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(_sheetName);

                PropertyInfo[] properties = collection.First().GetType().GetProperties();
                var headerNames = properties.Select(prop => prop.Name).ToList();

                for (int i = 0; i < headerNames.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headerNames[i];
                }

                worksheet.Cell(2, 1).InsertData(collection);
                workbook.SaveAs(stream);

                stream.Position = 0;
            }
            else
            {
                throw new ArgumentException("Sheetname or collection is empty");
            }

            return stream;
        }
    }

    public class Operation
    {
        /// <summary>
        /// Excel stream to datatable
        /// </summary>
        /// <param name="fileStream">Excel stream</param>
        /// <param name="sheetNumber">Sheet number</param>
        /// <returns>Datatable from excel</returns>
        public DataTable GetTableFromExcel(Stream fileStream, int sheetNumber = 1)
        {
            DataTable dt = new DataTable();

            if (fileStream != null && fileStream.Length > 0)
            {
                using (XLWorkbook workbook = new XLWorkbook(fileStream))
                {
                    if (workbook != null)
                    {
                        IXLWorksheet worksheet = workbook.Worksheet(sheetNumber);
                        bool firstRow = true;

                        //Range for reading the cells based on the last cell used.  
                        string readRange = "1:1";

                        if (worksheet != null)
                        {
                            foreach (IXLRow row in worksheet.RowsUsed())
                            {
                                if (row != null)
                                {
                                    if (firstRow)
                                    {
                                        //Checking the Last cellused for column generation in datatable  
                                        readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);

                                        foreach (IXLCell cell in row.Cells(readRange))
                                        {
                                            dt.Columns.Add(cell.Value.ToString());
                                        }

                                        firstRow = false;
                                    }
                                    else
                                    {
                                        //Adding a Row in datatable  
                                        dt.Rows.Add();

                                        int cellIndex = 0;

                                        //Updating the values of datatable  
                                        foreach (IXLCell cell in row.Cells(readRange))
                                        {
                                            dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                            cellIndex++;
                                        }
                                    }
                                }
                            }

                            if (firstRow)
                            {
                                // Empty excel
                                throw new Exception("Excel file is empty or could not read rows");
                            }
                        }
                    }
                }
            }
            else
            {
                // Empty stream
                throw new ArgumentException("Excel file stream is empty or null");
            }

            return dt;
        }
    }
}
