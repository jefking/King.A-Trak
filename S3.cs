namespace Abc.ATrak
{
    using Amazon.S3;
    using Amazon.S3.Model;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Amazon S3 Storage Item
    /// </summary>
    public class S3 : IStorageItem
    {
        #region Members
        /// <summary>
        /// Amazon S3
        /// </summary>
        private readonly IAmazonS3 client = null;

        /// <summary>
        /// S3 Bucket
        /// </summary>
        private readonly string bucket = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the S3
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="bucket">Bucket</param>
        /// <param name="relativePath">Relative Path</param>
        /// <param name="etag">ETag</param>
        public S3(IAmazonS3 client, string bucket, string relativePath, string etag = null)
        {
            this.client = client;
            this.bucket = bucket;
            this.RelativePath = relativePath.Replace('\\', '/');
            if (!string.IsNullOrWhiteSpace(etag))
            {
                this.MD5 = System.Convert.ToBase64String(StringToByteArray(etag.Replace("\"", string.Empty)));
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the MD5 Hash
        /// </summary>
        public string MD5
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Content Type
        /// </summary>
        public string ContentType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Path
        /// </summary>
        public string Path
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Relative Path
        /// </summary>
        public string RelativePath
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Read Fully
        /// </summary>
        /// <param name="input">Input Stream</param>
        /// <returns>Bytes</returns>
        public static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[64 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Get Data
        /// </summary>
        /// <returns>Data for object</returns>
        public byte[] GetData()
        {
            var request = new GetObjectRequest()
            {
                BucketName = this.bucket,
                Key = this.RelativePath,
            };

            using (var response = this.client.GetObject(request))
            {
                return ReadFully(response.ResponseStream);
            }
        }

        /// <summary>
        /// Check to see if item exists
        /// </summary>
        /// <returns>Exists</returns>
        public bool Exists()
        {
            try
            {
                var request = new GetObjectMetadataRequest()
                {
                    BucketName = this.bucket,
                    Key = this.RelativePath,
                };

                var response = this.client.GetObjectMetadata(request);
                this.ContentType = response.Headers.ContentType;
                this.MD5 = System.Convert.ToBase64String(StringToByteArray(response.ETag.Replace("\"", string.Empty)));

                return true;
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound || ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// String To Byte[]
        /// </summary>
        /// <param name="hex">Hexidecimal Value</param>
        /// <returns>Bytes</returns>
        internal static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// Save Storage Item
        /// </summary>
        /// <param name="storageItem">Storage Item</param>
        /// <param name="exists">Exists</param>
        public void Save(IStorageItem storageItem, bool exists = false)
        {
            var path = storageItem.RelativePath.Replace('\\', '/');
            var request = new PutObjectRequest()
            {
                BucketName = this.bucket,
                Key = path,
                CannedACL = S3CannedACL.Private,
                MD5Digest = storageItem.MD5,
                ContentType = storageItem.ContentType,
            };

            using (var stream = new MemoryStream(storageItem.GetData()))
            {
                request.InputStream = stream;
                var response = this.client.PutObject(request);
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        public void Delete()
        {
            var request = new DeleteBucketRequest()
            {
                BucketName = this.bucket,
            };

            var response = this.client.DeleteBucket(request);
            Trace.Write(string.Format("{0} deleted.", this.bucket));
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String reprensentation of object</returns>
        public override string ToString()
        {
            return string.Format("{0}", this.RelativePath);
        }
        #endregion
    }
}