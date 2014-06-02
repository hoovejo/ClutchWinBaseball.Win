using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace ClutchWinBaseball
{
    public class CacheFileManager
    {
        private Windows.Storage.StorageFolder tempFolder;

        public CacheFileManager(StorageFolder folder)
        {
            tempFolder = folder;
        }

        public async Task<string> CacheInquiryAsync(string fileName)
        {
            var contents = string.Empty;
            var exists = await tempFolder.FileExistsAsync(fileName);
            if (exists)
            {
                StorageFile file = await tempFolder.GetFileAsync(fileName);
                contents = await FileIO.ReadTextAsync(file);
            }
            return contents;
        }

        public async Task<bool> CacheUpdateAsync(string fileName, string jsonContent)
        {
            bool returnValue = true;
            StorageFile file = await tempFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, jsonContent);
            return returnValue;
        }

        public async Task<bool> DeleteAllFilesAsync()
        {
            await ApplicationData.Current.ClearAsync(ApplicationDataLocality.Local | ApplicationDataLocality.Temporary);
            return true;
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var exists = await tempFolder.FileExistsAsync(fileName);
            if (exists)
            {
                StorageFile file = await tempFolder.GetFileAsync(fileName);
                try { await file.DeleteAsync(StorageDeleteOption.PermanentDelete);  }
                catch { }
            }
            return true;
        }
    }
}
