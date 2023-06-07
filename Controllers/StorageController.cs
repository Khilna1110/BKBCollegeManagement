using Azure;
using BKBCollegeManagement.Models;
using BKBCollegeManagement.Services;
using Microsoft.AspNetCore.Mvc;


namespace BKBCollegeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly IAzureStorage _storage;

        public StorageController(IAzureStorage storage)
        {
            _storage = storage;
        }


        //Get all the files from bkb-edusimplified-container hosted on Azure Cloud
        [HttpGet(nameof(Get))]
        public async Task<IActionResult> Get()
        {
            // Get all files at the Azure Storage Location and return them
            List<BlobDto>? files = await _storage.ListAsync();

            // Returns an empty array if no files are present at the storage container
            return StatusCode(StatusCodes.Status200OK, files);
        }



        //Upload a Timetable for a course
        [HttpPost(nameof(UploadTimetable))]
        public async Task<IActionResult> UploadTimetable(IFormFile file,String courseName)
        {
            BlobResponseDto? response = await _storage.UploadAsync(file, courseName);

            // Check if we got an error
            if (response.Error == true)
            {
                // We got an error during upload, return an error with details to the client
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
            }
            else
            {
                // Return a success message to the client about successfull upload
                return StatusCode(StatusCodes.Status200OK, response);
            }
        }


        //Download the timetable for a course
        [HttpGet("DownloadTimetable/{courseName}")]
        public async Task<IActionResult> DownloadTimetable(string courseName)
        {
            string? fileURL = await _storage.DownloadAsync(courseName);

            // Check if file was found
            if (fileURL == null)
            {
                // Was not, return error message to client
                return StatusCode(StatusCodes.Status500InternalServerError, $"File could not be downloaded.");
            }
            else if(fileURL.Equals("Timetable not uploaded yet for this course!"))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Timetable not uploaded yet for this course!");
            }
            else
            {
                // File was found, return it to client
                return StatusCode(StatusCodes.Status200OK, fileURL); ;
            }
        }


        //delete timetable for course
        [HttpDelete("courseName")]
        public async Task<IActionResult> Delete(string courseName)
        {
            BlobResponseDto response = await _storage.DeleteAsync(courseName);

            // Check if we got an error
            if (response.Error == true)
            {
                // Return an error message to the client
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
            }
            else
            {
                // File has been successfully deleted
                return StatusCode(StatusCodes.Status200OK, response.Status);
            }
        }



        //Upload Study material for a course's subject
        [HttpPost(nameof(UploadStudyMaterial))]
        public async Task<IActionResult> UploadStudyMaterial(IFormFile file, string courseName,string subjectName)
        {
            BlobResponseDto? response = await _storage.UploadStudyMaterialAsync(file, courseName,subjectName);

            // Check if we got an error
            if (response.Error == true)
            {
                // We got an error during upload, return an error with details to the client
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
            }
            else
            {
                // Return a success message to the client about successfull upload
                return StatusCode(StatusCodes.Status200OK, response);
            }
        }



        //Get all the files from bkb-edusimplified-container hosted on Azure Cloud
        [HttpGet(nameof(GetStudyMaterials))]
        public async Task<IActionResult> GetStudyMaterials(string courseName,string subjectName)
        {
            // Get all files at the Azure Storage Location and return them
            List<BlobDto>? files = await _storage.ListStudyMaterialsAsync(courseName,subjectName);

            // Returns an empty array if no files are present at the storage container
            return StatusCode(StatusCodes.Status200OK, files);
        }

    }
}
