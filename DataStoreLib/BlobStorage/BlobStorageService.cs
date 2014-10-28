
namespace DataStoreLib.BlobStorage
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class BlobStorageService
    {
        public const string Blob_ImageContainer = "posters";
        public const string Blob_XMLFileContainer = "crawlfiles";
        public const string Blob_NewsImages = "newsimages";
        public const string Blob_AlgoLogs = "algorithmlogs";

        #region Private Methods
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

        public List<string> GetUploadedFileFromBlob(string containerName)
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
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        public void SetBlobProperties(string containerName)
        {
            CloudBlobContainer container = GetCloudBlobContainer(containerName);
            var blobs = container.ListBlobs(null, true);
            Parallel.ForEach(blobs, blob => SetBlobProperties(blob as CloudBlockBlob));
        }

        public void SetBlobProperties(string containerName, string[] fileNames)
        {
            CloudBlobContainer container = GetCloudBlobContainer(containerName);
            var blobs = container.ListBlobs(null, true).Where(b => fileNames.Any(f => b.Uri.ToString().Contains(f)));
            Parallel.ForEach(blobs, blob => SetBlobProperties(blob as CloudBlockBlob));
        }

        public void SetBlobProperties(CloudBlockBlob blob)
        {
            // Set all the properties on the blob as required

            // set cache-control header if necessary
            string header = "max-age=31536000";
            if (blob.Properties.CacheControl != header)
            {
                blob.Properties.CacheControl = header;
                blob.SetProperties();
            }
        }

        public string UploadImageFileOnBlob(string containerName, string fileName, Stream stream)
        {
            try
            {
                if (stream.Length > 0)
                {
                    stream.Position = 0;
                    CloudBlobContainer blobContainer = GetCloudBlobContainer(containerName);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);
                    blob.UploadFromStream(stream);

                    // Set properties on the blob
                    SetBlobProperties(blob);

                    switch (containerName)
                    {
                        case "newsimages":
                        case "algorithmlogs":
                            return GetSinglFile(containerName, fileName);
                        default:
                            return fileName;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return "";
        }

        public int GetImageFileCount(string containerName, string pattern)
        {
            try
            {
                List<string> blobFiles = GetUploadedFileFromBlob(containerName);

                var filtersFiles = blobFiles.FindAll(m => m.Contains(pattern));

                return filtersFiles.Count + 1;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return 0;
            }
        }

        public string GetSinglFile(string containerName, string fileName)
        {
            try
            {
                List<string> blobFiles = GetUploadedFileFromBlob(containerName);

                var file = blobFiles.Find(m => m.Contains(fileName));

                return file;
                //return fileName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> GetAllFiles(List<string> blobFiles, string pattern)
        {
            try
            {
                return blobFiles.FindAll(m => m.Contains(pattern));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public string UploadXMLFileOnBlob(string containerName, string fileName, string fileText)
        {
            try
            {
                if (fileText.Length > 0)
                {
                    //fileText.Position = 0;
                    CloudBlobContainer blobContainer = GetCloudBlobContainer(containerName);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName);
                    blob.UploadText(fileText);

                    return GetSinglFile(containerName, fileName);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return "";
        }

        public string GetUploadeXMLFileContent(string containerName, string fileName)
        {
            try
            {
                if (fileName.IndexOf("/") > -1)
                {
                    fileName = fileName.Substring(fileName.LastIndexOf("/") + 1);
                }

                CloudBlobContainer blobContainer = GetCloudBlobContainer(containerName);
                var blob = blobContainer.GetBlockBlobReference(fileName);

                var xmlData = blob.DownloadText();

                return xmlData;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return "";
            }
        }

        public bool DeleteFileFromBlob(string containerName, string fileName)
        {
            try
            {
                CloudBlobContainer blobContainer = GetCloudBlobContainer(containerName);
                var blob = blobContainer.GetBlockBlobReference(fileName);

                if (blob != null)
                    blob.Delete();

                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return false;
            }
        }
    }
}
