//using Azure.Storage.Blobs;
//using Azure.Storage.Blobs.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RunTest
//{
//    public interface IBlobService
//    {
//        Task UploadFileBlobAsync(byte[] content, string fileName, BlobContainer container);
//        Task<byte[]> GetFileBlobAsync(string fileName, BlobContainer container);
//        Task<bool> DoesBlobExists(string fileName, BlobContainer container);
//        Task DeleteFromBlob(string fileName, BlobContainer container);
//    }

//    public class BlobService : IBlobService
//    {
//        private readonly BlobServiceClient _blobServiceClient;

//        public BlobService(BlobServiceClient blobServiceClient)
//        {
//            _blobServiceClient = blobServiceClient;
//        }

//        public async Task<bool> DoesBlobExists(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            return await blobClient.ExistsAsync();
//        }

//        public async Task UploadFileBlobAsync(byte[] content, string fileName, BlobContainer container)
//        {
//            using (Stream stream = new MemoryStream(content))
//            {
//                var containerClient = GetContainerClient(container);
//                var blobClient = containerClient.GetBlobClient(fileName);
//                await blobClient.UploadAsync(stream);
//            }
//        }

//        public async Task<byte[]> GetFileBlobAsync(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            if (await blobClient.ExistsAsync())
//            {
//                var response = await blobClient.DownloadAsync();

//                using (MemoryStream stream = new MemoryStream())
//                {
//                    response.Value.Content.CopyTo(stream);

//                    return stream.ToArray();
//                }
//            }
//            else
//            {
//                return null;
//            }
//        }

//        private BlobContainerClient GetContainerClient(BlobContainer container)
//        {
//            var containerClient = _blobServiceClient.GetBlobContainerClient(container.ToString());
//            containerClient.CreateIfNotExists(PublicAccessType.Blob);
//            return containerClient;
//        }

//        public async Task DeleteFromBlob(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);
//            if (await blobClient.ExistsAsync())
//            {
//                await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
//            }
//        }
//    }

//    public enum BlobContainer
//    {
//        images,
//        documents,
//        publiccontent
//    }

//}//using Azure.Storage.Blobs;
//using Azure.Storage.Blobs.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RunTest
//{
//    public interface IBlobService
//    {
//        Task UploadFileBlobAsync(byte[] content, string fileName, BlobContainer container);
//        Task<byte[]> GetFileBlobAsync(string fileName, BlobContainer container);
//        Task<bool> DoesBlobExists(string fileName, BlobContainer container);
//        Task DeleteFromBlob(string fileName, BlobContainer container);
//    }

//    public class BlobService : IBlobService
//    {
//        private readonly BlobServiceClient _blobServiceClient;

//        public BlobService(BlobServiceClient blobServiceClient)
//        {
//            _blobServiceClient = blobServiceClient;
//        }

//        public async Task<bool> DoesBlobExists(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            return await blobClient.ExistsAsync();
//        }

//        public async Task UploadFileBlobAsync(byte[] content, string fileName, BlobContainer container)
//        {
//            using (Stream stream = new MemoryStream(content))
//            {
//                var containerClient = GetContainerClient(container);
//                var blobClient = containerClient.GetBlobClient(fileName);
//                await blobClient.UploadAsync(stream);
//            }
//        }

//        public async Task<byte[]> GetFileBlobAsync(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            if (await blobClient.ExistsAsync())
//            {
//                var response = await blobClient.DownloadAsync();

//                using (MemoryStream stream = new MemoryStream())
//                {
//                    response.Value.Content.CopyTo(stream);

//                    return stream.ToArray();
//                }
//            }
//            else
//            {
//                return null;
//            }
//        }

//        private BlobContainerClient GetContainerClient(BlobContainer container)
//        {
//            var containerClient = _blobServiceClient.GetBlobContainerClient(container.ToString());
//            containerClient.CreateIfNotExists(PublicAccessType.Blob);
//            return containerClient;
//        }

//        public async Task DeleteFromBlob(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);
//            if (await blobClient.ExistsAsync())
//            {
//                await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
//            }
//        }
//    }

//    public enum BlobContainer
//    {
//        images,
//        documents,
//        publiccontent
//    }

//}//using Azure.Storage.Blobs;
//using Azure.Storage.Blobs.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RunTest
//{
//    public interface IBlobService
//    {
//        Task UploadFileBlobAsync(byte[] content, string fileName, BlobContainer container);
//        Task<byte[]> GetFileBlobAsync(string fileName, BlobContainer container);
//        Task<bool> DoesBlobExists(string fileName, BlobContainer container);
//        Task DeleteFromBlob(string fileName, BlobContainer container);
//    }

//    public class BlobService : IBlobService
//    {
//        private readonly BlobServiceClient _blobServiceClient;

//        public BlobService(BlobServiceClient blobServiceClient)
//        {
//            _blobServiceClient = blobServiceClient;
//        }

//        public async Task<bool> DoesBlobExists(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            return await blobClient.ExistsAsync();
//        }

//        public async Task UploadFileBlobAsync(byte[] content, string fileName, BlobContainer container)
//        {
//            using (Stream stream = new MemoryStream(content))
//            {
//                var containerClient = GetContainerClient(container);
//                var blobClient = containerClient.GetBlobClient(fileName);
//                await blobClient.UploadAsync(stream);
//            }
//        }

//        public async Task<byte[]> GetFileBlobAsync(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            if (await blobClient.ExistsAsync())
//            {
//                var response = await blobClient.DownloadAsync();

//                using (MemoryStream stream = new MemoryStream())
//                {
//                    response.Value.Content.CopyTo(stream);

//                    return stream.ToArray();
//                }
//            }
//            else
//            {
//                return null;
//            }
//        }

//        private BlobContainerClient GetContainerClient(BlobContainer container)
//        {
//            var containerClient = _blobServiceClient.GetBlobContainerClient(container.ToString());
//            containerClient.CreateIfNotExists(PublicAccessType.Blob);
//            return containerClient;
//        }

//        public async Task DeleteFromBlob(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);
//            if (await blobClient.ExistsAsync())
//            {
//                await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
//            }
//        }
//    }

//    public enum BlobContainer
//    {
//        images,
//        documents,
//        publiccontent
//    }

//}//using Azure.Storage.Blobs;
//using Azure.Storage.Blobs.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RunTest
//{
//    public interface IBlobService
//    {
//        Task UploadFileBlobAsync(byte[] content, string fileName, BlobContainer container);
//        Task<byte[]> GetFileBlobAsync(string fileName, BlobContainer container);
//        Task<bool> DoesBlobExists(string fileName, BlobContainer container);
//        Task DeleteFromBlob(string fileName, BlobContainer container);
//    }

//    public class BlobService : IBlobService
//    {
//        private readonly BlobServiceClient _blobServiceClient;

//        public BlobService(BlobServiceClient blobServiceClient)
//        {
//            _blobServiceClient = blobServiceClient;
//        }

//        public async Task<bool> DoesBlobExists(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            return await blobClient.ExistsAsync();
//        }

//        public async Task UploadFileBlobAsync(byte[] content, string fileName, BlobContainer container)
//        {
//            using (Stream stream = new MemoryStream(content))
//            {
//                var containerClient = GetContainerClient(container);
//                var blobClient = containerClient.GetBlobClient(fileName);
//                await blobClient.UploadAsync(stream);
//            }
//        }

//        public async Task<byte[]> GetFileBlobAsync(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            if (await blobClient.ExistsAsync())
//            {
//                var response = await blobClient.DownloadAsync();

//                using (MemoryStream stream = new MemoryStream())
//                {
//                    response.Value.Content.CopyTo(stream);

//                    return stream.ToArray();
//                }
//            }
//            else
//            {
//                return null;
//            }
//        }

//        private BlobContainerClient GetContainerClient(BlobContainer container)
//        {
//            var containerClient = _blobServiceClient.GetBlobContainerClient(container.ToString());
//            containerClient.CreateIfNotExists(PublicAccessType.Blob);
//            return containerClient;
//        }

//        public async Task DeleteFromBlob(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);
//            if (await blobClient.ExistsAsync())
//            {
//                await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
//            }
//        }
//    }

//    public enum BlobContainer
//    {
//        images,
//        documents,
//        publiccontent
//    }

//}//using Azure.Storage.Blobs;
//using Azure.Storage.Blobs.Models;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RunTest
//{
//    public interface IBlobService
//    {
//        Task UploadFileBlobAsync(byte[] content, string fileName, BlobContainer container);
//        Task<byte[]> GetFileBlobAsync(string fileName, BlobContainer container);
//        Task<bool> DoesBlobExists(string fileName, BlobContainer container);
//        Task DeleteFromBlob(string fileName, BlobContainer container);
//    }

//    public class BlobService : IBlobService
//    {
//        private readonly BlobServiceClient _blobServiceClient;

//        public BlobService(BlobServiceClient blobServiceClient)
//        {
//            _blobServiceClient = blobServiceClient;
//        }

//        public async Task<bool> DoesBlobExists(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            return await blobClient.ExistsAsync();
//        }

//        public async Task UploadFileBlobAsync(byte[] content, string fileName, BlobContainer container)
//        {
//            using (Stream stream = new MemoryStream(content))
//            {
//                var containerClient = GetContainerClient(container);
//                var blobClient = containerClient.GetBlobClient(fileName);
//                await blobClient.UploadAsync(stream);
//            }
//        }

//        public async Task<byte[]> GetFileBlobAsync(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            if (await blobClient.ExistsAsync())
//            {
//                var response = await blobClient.DownloadAsync();

//                using (MemoryStream stream = new MemoryStream())
//                {
//                    response.Value.Content.CopyTo(stream);

//                    return stream.ToArray();
//                }
//            }
//            else
//            {
//                return null;
//            }
//        }

//        private BlobContainerClient GetContainerClient(BlobContainer container)
//        {
//            var containerClient = _blobServiceClient.GetBlobContainerClient(container.ToString());
//            containerClient.CreateIfNotExists(PublicAccessType.Blob);
//            return containerClient;
//        }

//        public async Task DeleteFromBlob(string fileName, BlobContainer container)
//        {
//            var containerClient = GetContainerClient(container);
//            var blobClient = containerClient.GetBlobClient(fileName);
//            if (await blobClient.ExistsAsync())
//            {
//                await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
//            }
//        }
//    }

//    public enum BlobContainer
//    {
//        images,
//        documents,
//        publiccontent
//    }

//}
