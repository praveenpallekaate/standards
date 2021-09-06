## Overview
Excel package is used to perform some basic spreadsheet operatons. The package is supported for apps from `.Net Core 3`.

## Services
- Create stream for excel from collection. The model that is being streamed should inherit `ICollectionToExcel`
- Create `DataTable` from spreadsheet.

## Usage
Below is the package usage details.
```csharp
    using Core.Excel
    ...
    private readonly Operation _excelOperation;

    public ctor()
    {
        _excelOperation = new Operation<model>("sheet1", "A1");
    }

    ...
        var stream = _excelOperation.GetExcelFromCollection(collection);

    ...

    public ctor()
    {
        _excelOperation = new Operation();
    }

    ...

        var dt = _excelOperation.GetTableFromExcel(fileStream, 1);
```