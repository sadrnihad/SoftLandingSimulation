using SoftLanding.Utilities.Enums;

namespace SoftLanding.Utilities.Extensions
{
    public static class Validator
    {
        public static bool ValidateType(this IFormFile formFile, string type)
        {
            if (formFile.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }
        public static bool ValidateSize(this IFormFile formFile, FileSize fileSize, int size)
        {
            switch (fileSize)
            {
                case FileSize.KB:
                    return formFile.Length > size * 1024;
                case FileSize.MB:
                    return formFile.Length > size * 1024 * 1024;
                case FileSize.GB:
                    return formFile.Length > size * 1024 * 1024 * 1024;
            }
            return false;

        }

        public async static Task<string> CreateFileAsync(this IFormFile formFile, params string[] roots)
        {
            string fileName = string.Concat(Guid.NewGuid().ToString(), formFile.FileName);
            string path = string.Empty;
            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }

            path = Path.Combine(path, fileName);

            using (FileStream fileStream = new(path, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return fileName;
        }
    }
}