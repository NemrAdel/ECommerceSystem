namespace AdminDashboard.Helpers
{
    public class PictureSettings
    {
        public static string UploadFile(IFormFile file , string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", "images", folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fileName=Guid.NewGuid() + file.FileName;
            var filePath=Path.Combine(folderPath,fileName);
            var fileStream=new FileStream(filePath,FileMode.Create);
            file.CopyTo(fileStream);
            return Path.Combine ("images//products", fileName);
        }
        public static void DeleteFile( string folderName,string filePath)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", "images", folderName);
            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
