using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using DataStoreLib.Utils;
using System.IO;

namespace DataStoreLib.BlobStorage
{
    public class BlobStorageService
    {
        private CloudBlobContainer GetCloudBlobContainer(string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting("StorageTableConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobCantainer = blobClient.GetContainerReference(containerName);

            if (blobCantainer.CreateIfNotExists())
            {
                blobCantainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            return blobCantainer;
        }

        public string UploadFileOnBlob(string fileName, Stream stream, string containerName)
        {
            try
            {
                if (stream.Length > 0)
                {
                    stream.Position = 0;
                    CloudBlobContainer blobContainer = GetCloudBlobContainer(containerName);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);
                    blob.UploadFromStream(stream);

                    return GetSinglFile(containerName, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "";
        }

        private List<string> GetUploadedFileFromBlob(string containerName)
        {
            try
            {
                CloudBlobContainer blobContainer = GetCloudBlobContainer(containerName);
                List<string> blobs = new List<string>();

                foreach (var blobItem in blobContainer.ListBlobs())
                {
                    blobs.Add(blobItem.Uri.ToString());
                }

                return blobs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetFileCounter(string containerName, string pattern)
        {
            try
            {
                List<string> blobFiles = GetUploadedFileFromBlob(containerName);

                var filtersFiles = blobFiles.FindAll(m => m.Contains(pattern));

                return filtersFiles.Count + 1;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return 0;
        }

        public string GetSinglFile(string containerName, string fileName)
        {
            try
            {
                List<string> blobFiles = GetUploadedFileFromBlob(containerName);

                var file = blobFiles.Find(m => m.Contains(fileName));

                return file;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}
