namespace Abc.ATrak
{
    using System.IO;
    using Amazon.S3;
    using Amazon.S3.Model;
    using System;
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
        private readonly AmazonS3 client = null;

        /// <summary>
        /// Bucket
        /// </summary>
        private readonly string bucket = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes an S3 Object
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="bucket">Bucket</param>
        /// <param name="relativePath">Relative Path</param>
        /// <param name="etag">ETag</param>
        public S3(AmazonS3 client, string bucket, string relativePath, string etag = null)
        {
            this.client = client;
            this.bucket = bucket;
            this.RelativePath = relativePath;
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

            using (var response = client.GetObject(request))
            {
                return ReadFully(response.ResponseStream);
            }
        }

        /// <summary>
        /// Read Fully
        /// </summary>
        /// <param name="input">Input Stream</param>
        /// <returns>Bytes</returns>
        public static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[128 * 1024];
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
        /// Check to see if item exists
        /// </summary>
        /// <returns>Exists</returns>
        public bool Exists()
        {
            try
            {
                var request = new GetObjectMetadataRequest()
                {
                    BucketName = bucket,
                    Key = this.RelativePath,
                };

                using (var response = client.GetObjectMetadata(request))
                {
                    this.ContentType = response.ContentType;
                    this.MD5 = System.Convert.ToBase64String(StringToByteArray(response.ETag.Replace("\"", string.Empty)));
                }

                return true;
            }

            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;

                throw;
            }
        }

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
            using (var stream = new MemoryStream(storageItem.GetData()))
            {
                var request = new PutObjectRequest()
                {
                    BucketName = bucket,
                    InputStream = stream,
                    Key = storageItem.RelativePath,
                    CannedACL = S3CannedACL.Private,
                    Timeout = 3600000,
                    MD5Digest = storageItem.MD5,
                    ContentType = storageItem.ContentType,
                };

                using (var response = client.PutObject(request))
                {
                }
            }
        }

        /// <summary>
        /// To String
        /// </summary>
        /// <returns>String reprensentation of object</returns>
        public override string ToString()
        {
            return string.Format("{0}", this.Path);
        }
        #endregion
    }
}