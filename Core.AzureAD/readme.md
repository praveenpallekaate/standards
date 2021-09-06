## Overview
Azure AD package allows to connect to Azure AD using Microsoft GraphClient. It is supported by apps from `.Net Core 3`

## Services
Below are the services provided by the package

- Get current user details
- Get user details by email with picture

## Usage
```csharp
    using Core.AzureAD;
    ...
    private readonly GraphServiceClient _graphServiceClient = null;
    private readonly AzureADService _azureADService = null;

    public Ctor(GraphServiceClient graphServiceClient)
    {
        _graphServiceClient = graphServiceClient;

        // Initialize AD service
        _azureADService = new AzureADService(_graphServiceClient);
    }

    public async Task GetUserAsync()
    {
        ...
        var user = (await _azureADService.GetUsersForEmailAsync(item)).FirstOrDefault();
    }
    ...
```