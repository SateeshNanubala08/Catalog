using Amazon.S3.Model;
using Catalog.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repositories
{

    public interface IFileRepository
    {
        Task<bool> UploadFile(int uploadFileName);
        Task<List<string>> FilesList();
        Task<Stream> GetFile(string key);
        Task<bool> UpdateFile(int uploadFileName, string key);
        Task<bool> DeleteFile(string key); 
    }


    public class FileRepository : IFileRepository
    {
        private readonly IAWSS3BucketHelper _AWSS3BucketHelper;

        public FileRepository(IAWSS3BucketHelper AWSS3BucketHelper)
        {
            this._AWSS3BucketHelper = AWSS3BucketHelper;
        }
        public async Task<bool> UploadFile(int id)
        {
            //try
            //{
                var filename = id.ToString() + ".json";

                var path = Path.Combine("Files", filename);
                using (FileStream fsSource = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    string fileExtension = Path.GetExtension(path);
                    string fileName = string.Empty;
                    fileName = filename ;
                    return await _AWSS3BucketHelper.UploadFile(fsSource, fileName);
                }
            //}
            //catch (Exception ex)
            //{
            //    throw ex; 
            //}
        }

        public async Task<List<string>> FilesList()
        {
            //try
            //{
                ListVersionsResponse listVersions = await _AWSS3BucketHelper.FilesList();
                return listVersions.Versions.Select(c => c.Key).ToList();
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
        }
        public async Task<Stream> GetFile(string key)
        {
            //try
            //{
                Stream fileStream = await _AWSS3BucketHelper.GetFile(key);
                if (fileStream == null)
                {
                    Exception ex = new Exception("File Not Found");
                    throw ex;
                }
                else
                {
                    return fileStream;
                }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        public async Task<bool> UpdateFile(int uploadFileName, string key)
        {
            //try
            //{
                var path = Path.Combine("Files", uploadFileName.ToString() + ".Json");
                using (FileStream fsSource = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    return await _AWSS3BucketHelper.UploadFile(fsSource, key);
                }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        public async Task<bool> DeleteFile(string key)
        {
            //try
            //{
                return await _AWSS3BucketHelper.DeleteFile(key);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
    }
}
