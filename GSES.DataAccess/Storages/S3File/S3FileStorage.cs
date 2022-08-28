using Amazon.S3;
using GSES.DataAccess.Consts;
using GSES.DataAccess.Entities;
using GSES.DataAccess.Storages.Bases;

namespace GSES.DataAccess.Storages.S3File
{
    public class S3FileStorage : IStorage
    {
        public S3FileStorage(IAmazonS3 s3Client)
        {
            s3Client.EnsureBucketExistsAsync(AWSConsts.DataBucketName);
            this.subscribers = new S3File<Subscriber>(s3Client);
        }

        private S3File<Subscriber> subscribers;

        public ITable<Subscriber> Subscribers
        {
            get => this.subscribers;
            set => this.subscribers = (S3File<Subscriber>)value;
        }
    }
}
