
## Overview
Blazor components is a Razor class library, targeting Blazor and rozor supporting apps above `.Net Core 3`. The library has some reusable components.

## Components
- Search input control with delay
- Input field with label
- Layout banner
- Loading bars
- Chip(tag)
- Icon button
- Pagination

## Usage 

```html
<Core.BlazorComponents.Inputs.LabelInputField Class="flat"
                                              Label="Employee Number (Oracle)"
                                              LabelClass="ft-07 mb-03 field-label"
                                              Value="@EditedEmployee.OracleEmployeeNumber"
                                              ValueClass="label-field-value" />
<Core.BlazorComponents.Inputs.LabelInputField Class="small"
                                              Label="Employee Number (Oracle)"
                                              LabelClass="ft-07 mb-03 field-label"
                                              Value="@EditedEmployee.OracleEmployeeNumber"
                                              ValueClass="form-control input"
                                              OnTextChange="@((e) => EditedEmployee.OracleEmployeeNumber = e )"
                                              Type="ComponentInputType.text" />
```                                                