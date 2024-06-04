namespace Pronia.Extensions
{
    public static class FileExtension
    {
        public static bool IsValidType(this IFormFile formFile, string type)
            => formFile.ContentType.Contains(type);

        public static bool IsValidSize(this IFormFile formFile, int KByte)
            => formFile.Length <= KByte *1024;

        public static async Task<string> FileManageAsync(this IFormFile formFile, string path)
        {
            string ext = Path.GetExtension(formFile.FileName);
            string filename = Path.GetRandomFileName();

            await using FileStream fileStream = new FileStream(Path.Combine(path, filename+ext), FileMode.Create);
            await formFile.CopyToAsync(fileStream);
            
            return filename + ext;
        }

        public static async Task Delete(this string FileName, string path)
        {
            File.Delete(Path.Combine(path, FileName));

        }
    }
}
