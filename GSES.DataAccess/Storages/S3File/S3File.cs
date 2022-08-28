using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using GSES.DataAccess.Consts;
using GSES.DataAccess.Entities.Bases;
using GSES.DataAccess.Storages.Bases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace GSES.DataAccess.Storages.S3File
{
    public class S3File<T> : ITable<T> where T : BaseEntity
    {
        private const string FileName = nameof(T) + GeneralConsts.JsonExtension;

        private readonly IAmazonS3 s3Client;

        public S3File(IAmazonS3 s3Client)
        {
            this.s3Client = s3Client;
        }

        public async Task AddAsync(T element)
        {
            var existingFiles = await this.s3Client.ListObjectsAsync(AWSConsts.DataBucketName);

            var allTheElements = new List<T>();

            if (existingFiles != null)
            {
                var existingElements = await this.GetListOfElementsFromS3FileAsync();
                allTheElements.AddRange(existingElements);

                await this.DeleteOldVersionOfS3FileAsync();
            }

            if (allTheElements.Contains(element))
            {
                throw new DuplicateNameException(GeneralConsts.DuplicateErrorMessage);
            }

            allTheElements.Add(element);

            await this.PutNewVersionOfFileToS3BucketAsync(allTheElements);
        }

        public Task DeleteAsync(T element)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate)
        {
            var existingFiles = await this.s3Client.ListObjectsAsync(AWSConsts.DataBucketName);
            var items = new List<T>();

            if (existingFiles != null)
            {
                var existingElements = await this.GetListOfElementsFromS3FileAsync();
                items.AddRange(existingElements);
            }

            return items.Where(predicate);
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return this.GetAsync(t => true);
        }

        public Task UpdateAsync(T element)
        {
            throw new NotImplementedException();
        }

        private async Task<IEnumerable<T>> GetListOfElementsFromS3FileAsync()
        {
            var request = new GetObjectRequest
            {
                BucketName = AWSConsts.DataBucketName,
                Key = FileName,
            };

            var dataFile = await this.s3Client.GetObjectAsync(request);

            if (dataFile is null)
            {
                return new List<T>();
            }

            using var dataStreamReader = new StreamReader(dataFile.ResponseStream, Encoding.UTF8);
            var dataFromFile = dataStreamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<IEnumerable<T>>(dataFromFile);
        }

        private async Task DeleteOldVersionOfS3FileAsync()
        {
            var request = new DeleteObjectRequest
            {
                BucketName = AWSConsts.DataBucketName,
                Key = FileName,
            };

            await this.s3Client.DeleteObjectAsync(request);
        }

        private async Task PutNewVersionOfFileToS3BucketAsync(IEnumerable<T> items)
        {
            using var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream);

            var serializedModel = JsonConvert.SerializeObject(items);
            streamWriter.Write(serializedModel);
            streamWriter.Flush();
            stream.Position = 0;

            var transferRequest = new TransferUtilityUploadRequest
            {
                BucketName = AWSConsts.DataBucketName,
                Key = FileName,
                InputStream = stream,
            };

            using var transferUtility = new TransferUtility(this.s3Client);
            await transferUtility.UploadAsync(transferRequest);
        }
    }
}
