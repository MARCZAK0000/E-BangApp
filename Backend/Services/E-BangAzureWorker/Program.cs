using Azure.Storage.Blobs;
using E_BangAzureWorker;
using Microsoft.Extensions.Azure;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton(
    pr => new BlobServiceClient
    (builder.Configuration.GetConnectionString("test"),
    new BlobClientOptions(BlobClientOptions.ServiceVersion.V2020_02_10)));
var host = builder.Build();
host.Run();
