using System;
using System.Threading.Tasks;
using Windows.Storage;
using System.IO;
using System.Text;

namespace ClutchWinBaseball.WP8
{
    public class CacheFileManager
    {
        const string DataFolderName = "DataFolder";

        private Windows.Storage.StorageFolder tempFolder;

        public CacheFileManager(StorageFolder folder)
        {
            tempFolder = folder;
        }

        public async Task<string> CacheInquiryAsync(string fileName)
        {
            var folder = await tempFolder.GetFolderAsync(DataFolderName);

            var contents = string.Empty;
            if (folder != null)
            {
                var exists = await folder.FileExistsAsync(fileName);
                if (exists)
                {
                    var file = await folder.OpenStreamForReadAsync(fileName);
                    using (StreamReader streamReader = new StreamReader(file))
                    {
                        contents = streamReader.ReadToEnd();
                    }
                }
            }
            return contents;
        }

        public async Task<bool> CacheUpdateAsync(string fileName, string jsonContent)
        {
            bool returnValue = true;
            byte[] fileBytes = Encoding.UTF8.GetBytes(jsonContent);
            var dataFolder = await tempFolder.CreateFolderAsync(DataFolderName, CreationCollisionOption.OpenIfExists);
            var file = await dataFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(fileBytes, 0, fileBytes.Length);
            }
            return returnValue;
        }

        public async Task<bool> DeleteAllFilesAsync()
        {
            var folder = await tempFolder.GetFolderAsync(DataFolderName);
            if (folder != null)
            {
                await folder.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            return true;
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var folder = await tempFolder.GetFolderAsync(DataFolderName);

            if (folder != null)
            {
                var exists = await folder.FileExistsAsync(fileName);
                if (exists)
                {
                    StorageFile file = await folder.GetFileAsync(fileName);
                    try { await file.DeleteAsync(StorageDeleteOption.PermanentDelete); }
                    catch { }
                }
            }
            return true;
        }
    }
}
