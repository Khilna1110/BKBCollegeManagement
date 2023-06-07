using BKBCollegeManagement.Models;

namespace BKBCollegeManagement.Services
{
    public interface IAzureStorage
    {

        /// <summary>
        /// This method uploads a file submitted with the request
        /// </summary>
        /// <param name="file">File for upload</param>
        /// <returns>Blob with status</returns>
        Task<BlobResponseDto> UploadAsync(IFormFile file,String courseName);

        /// <summary>
        /// This method downloads a file with the specified filename
        /// </summary>
        /// <param name="blobFilename">Filename</param>
        /// <returns>Blob</returns>
        Task<string> DownloadAsync(string courseName);

        /// <summary>
        /// This method deleted a file with the specified filename
        /// </summary>
        /// <param name="blobFilename">Filename</param>
        /// <returns>Blob with status</returns>
        Task<BlobResponseDto> DeleteAsync(string courseName);

        /// <summary>
        /// This method returns a list of all files located in the container
        /// </summary>
        /// <returns>Blobs in a list</returns>
        Task<List<BlobDto>> ListAsync();

        // /// <summary>
        /// This method uploads a file for study material submitted with the request
        /// </summary>
        /// <param name="file">File for upload</param>
        /// <returns>Blob with status</returns>
        Task<BlobResponseDto> UploadStudyMaterialAsync(IFormFile file, string courseName,string subjectName);


       
        /// <summary>
        /// This method returns a list of all files located in the container
        /// </summary>
        /// <returns>Blobs in a list</returns>
        Task<List<BlobDto>> ListStudyMaterialsAsync(string courseName,string subjectName);
    }
}
