using AbidiCompanySenario.ViewModels.Personnels;
using System.Text;

namespace ChristianBeauty.Utilities
{
    public static class FileHandler
    {
        public static async Task<string?> FileUploadAsync(IFormFile formFile, IWebHostEnvironment webHostEnvironment)
        {
            if (formFile.Length > 0)
            {
                var uniqueFileName = GetUniqueFileName(formFile.FileName);
                var uploadFolderPath = Path.Combine(
                    webHostEnvironment.WebRootPath,
                    "Files",
                    "AcademicDegrees"
                );   

                // Check if the target folder exists, create it if not
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                var filePath = Path.Combine(uploadFolderPath, uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await formFile.CopyToAsync(stream);

                return uniqueFileName;
            }
            return null;
        }

        public static void DeleteFile(string imageName, IWebHostEnvironment webHostEnvironment)
        {
            var filePath = Path.Combine(
                webHostEnvironment.WebRootPath,
                    "Files",
                    "AcademicDegrees",
                imageName
            );

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);

                // Check if the folder is empty after deleting the file
                var folderPath = Path.GetDirectoryName(filePath);
                if (Directory.GetFiles(folderPath).Length == 0)
                {
                    Directory.Delete(folderPath);
                }
            }
        }


   



        private static string GetUniqueFileName(string fileName)
        {
            string name = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            return $"{name}_{timestamp}{extension}";
        }


     
    }
}
