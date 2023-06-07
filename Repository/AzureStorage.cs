using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using BKBCollegeManagement.Models;
using BKBCollegeManagement.Services;
using Azure;
using System.Reflection.Metadata;
using System.ComponentModel;
using System.IO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Azure.Storage;
using static System.Net.WebRequestMethods;
using static System.Reflection.Metadata.BlobBuilder;

namespace BKBCollegeManagement.Repository
{
    public class AzureStorage : IAzureStorage
    {
        //region Dependency Injection / Constructor

        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly string _accountName;
        private readonly string _accessKey;
        private readonly ILogger<AzureStorage> _logger;

        public AzureStorage(IConfiguration configuration, ILogger<AzureStorage> logger)
        {
            _storageConnectionString = configuration.GetValue<string>("BlobConnectionString");
            _storageContainerName = configuration.GetValue<string>("BlobContainerName");
            _accountName = configuration.GetValue<string>("AccountName");
            _accessKey = configuration.GetValue<string>("AccessKey");
            _logger = logger;
        }

        //endregion

        public async Task<List<BlobDto>> ListAsync()
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            // Create a new list object for 
            List<BlobDto> files = new List<BlobDto>();

            await foreach (BlobItem file in container.GetBlobsAsync(prefix:"courses/"))
            {
                // Add each file retrieved from the storage container to the files list by creating a BlobDto object
                string uri = container.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            // Return all files to the requesting method
            return files;
        }


        public async Task<List<BlobDto>> ListStudyMaterialsAsync(string courseName,string subjectName)
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            // Create a new list object for 
            List<BlobDto> files = new List<BlobDto>();

            string prefixPath = "courses/" + courseName + "/subjects/" + subjectName + "/";
            await foreach (BlobItem file in container.GetBlobsAsync(prefix: prefixPath))
            {
                // Add each file retrieved from the storage container to the files list by creating a BlobDto object
                var name = file.Name;
                
                // Get a reference to the blob uploaded earlier from the API in the container from configuration settings
                BlobClient client = container.GetBlobClient(file.Name);


                Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()

                {
                    BlobContainerName = _storageContainerName,
                    BlobName = file.Name,
                    ExpiresOn = DateTime.UtcNow.AddDays(7)//Let SAS token expire after 5 minutes.
                };
                blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);//User will only be able to read the blob and it's properties
                var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_accountName, _accessKey)).ToString();
                string sasUrl = client.Uri.AbsoluteUri + "?" + sasToken;


                files.Add(new BlobDto
                {
                    Uri = sasUrl,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            // Return all files to the requesting method
            return files;
        }

        

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob,String courseName)
        {
            // Create new upload response object that we can return to the requesting method
            BlobResponseDto response = new();

            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            //await container.CreateAsync();
            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlobClient client = container.GetBlobClient("courses/"+ courseName +"/timetable/"+blob.FileName);

                // Open a stream for the file we want to upload
                await using (Stream? data = blob.OpenReadStream())
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {blob.FileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                // Log error to console and create a new response we can return to the requesting method
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            // Return the BlobUploadResponse object
            return response;
        }

        public async Task<string> DownloadAsync(string courseName)
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);


            try

            {
                string prefixPath = "courses/" + courseName + "/timetable/";
                var file1 = client.GetBlobs(prefix: prefixPath).OrderByDescending(m => m.Properties.LastModified).First();


                // Get a reference to the blob uploaded earlier from the API in the container from configuration settings
                BlobClient file = client.GetBlobClient(file1.Name);

                // Check if the file exists in the container

                

                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    // Download the file details async
                    var content = await file.DownloadContentAsync();

                    // Add data to variables in order to return a BlobDto
                    string name = file1.Name;

                    string contentType = content.Value.Details.ContentType;

                    Azure.Storage.Sas.BlobSasBuilder blobSasBuilder = new Azure.Storage.Sas.BlobSasBuilder()

                    {
                        BlobContainerName = _storageContainerName,
                        BlobName = file1.Name,
                        ExpiresOn = DateTime.UtcNow.AddDays(7)//Let SAS token expire after 5 minutes.
                    };
                    blobSasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Read);//User will only be able to read the blob and it's properties
                    var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_accountName, _accessKey)).ToString();
                    string sasUrl = file.Uri.AbsoluteUri + "?" + sasToken;
                    // Create new BlobDto with blob data from variables
                    return  sasUrl;

                }

            }


            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // Log error to console
                _logger.LogError($"File was not found.");
            }
            catch(InvalidOperationException ex)
            {

                return "Timetable not uploaded yet for this course!";
        
            }

            // File does not exist, return null and handle that in requesting method
            return null;
        }

        public async Task<BlobResponseDto> DeleteAsync(string courseName)
        {
            BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            BlobResponseDto response = new();

            string prefixPath = "courses/" + courseName + "/timetable/";
            var file1 = client.GetBlobs(prefix: prefixPath).OrderByDescending(m => m.Properties.LastModified).First();


            BlobClient file = client.GetBlobClient(file1.Name);


            try
            {
                // Delete the file
                await file.DeleteAsync();
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // File did not exist, log to console and return new response to requesting method
                _logger.LogError($"File {file1.Name} was not found.");
                return new BlobResponseDto { Error = true, Status = $"File with name {file1.Name} not found." };
            }

            // Everything is OK and file got uploaded
            response.Status = $"File {file1.Name} Deleted Successfully";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = file1.Name;
            return response;

        }


        public async Task<BlobResponseDto> UploadStudyMaterialAsync(IFormFile blob, string courseName,string subjectName)
        {
            // Create new upload response object that we can return to the requesting method
            BlobResponseDto response = new();

            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);
            //await container.CreateAsync();
            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlobClient client = container.GetBlobClient("courses/" + courseName + "/subjects/" + subjectName+"/"+blob.FileName);

                // Open a stream for the file we want to upload
                await using (Stream? data = blob.OpenReadStream())
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {blob.FileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                // Log error to console and create a new response we can return to the requesting method
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            // Return the BlobUploadResponse object
            return response;
        }
    }
}

