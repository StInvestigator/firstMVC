using firstMVC.Models;

namespace firstMVC.Services
{
    public class LocalFileService
    {
        private readonly string _root;
        public LocalFileService(string root)
        {
            _root = root;
        }
        protected string GetFullPath(string filename)
        {
            string dir1 = filename[0].ToString();
            string dir2 = filename[1].ToString();
            string directory = Path.Combine(_root, dir1, dir2);
            return Path.Combine(directory, filename);
        }
        public async Task<Image> CreateImage(IFormFile file)
        {
            var filename = Guid.NewGuid().ToString().ToLower() + Path.GetExtension(file.FileName);
            var filePath = GetFullPath(filename);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using(var newFile = File.OpenWrite(filePath))
            {
                await file.CopyToAsync(newFile);
            }

            return new Image
            {
                Name = filename,
                Path = Path.Combine("\\uploads", "img", filename[0].ToString(), filename[1].ToString(), filename),
                OriginalName = Path.GetFileNameWithoutExtension(file.FileName)
            };
        }
        public void DeleteImage(Image file) 
        {
            var fullPath = GetFullPath(file.Name);
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }
    }
}
