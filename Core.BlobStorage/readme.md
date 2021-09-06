## Overview
Blob Storage package is used managing Azure blob. It's supported in apps above `.Net Core 3`.

## Services
Below are the services provided by the package
- Upload file
- Download file

## Usage
```csharp
    using Core.BlobStorage
    ...
    private readonly Storage _blobStorage = null;

    public ctor()
    {
        _blobStorage = new Storage(_appConfig.Storage.BlobConnectionString);
    }

    public async Task UploadAsync()
    {
        ...
        await _blobStorage.UploadFileAsBlobAsync(stream, filename, container);
    }

    public async Task DownloadAsync()
    {
        ...
        var file = await _blobStorage.DownloadFileAsStreamAsync(filename, container);
        ...
    }
```