using GSES.DataAccess.Consts;
using GSES.DataAccess.Entities.Bases;
using GSES.DataAccess.Storages.Bases;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using F = System.IO.File;

namespace GSES.DataAccess.Storages.File
{
    public class File<T> : ITable<T> where T : BaseEntity
    {
        private readonly static string FileName = typeof(T) + GeneralConsts.JsonExtension;

        private readonly IConfiguration _configuration;

        private string FolderPath => this._configuration[FileConsts.FilePathConfig];

        private string FullPath => this.FolderPath + FileName;

        public File(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task AddAsync(T element)
        {
            var items = await this.GetAllAsync();
            var list = items.ToList();

            if (list.Contains(element))
            {
                throw new DuplicateNameException(GeneralConsts.DuplicateErrorMessage);
            }

            list.Add(element);
            var jsonModel = JsonConvert.SerializeObject(list);
            EnsureFolderExists(this.FolderPath);

            using var fileStream = new FileStream(this.FullPath, FileMode.OpenOrCreate, FileAccess.Write);
            using var streamWriter = new StreamWriter(fileStream);

            await streamWriter.WriteAsync(jsonModel);
        }

        public Task DeleteAsync(T element)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> GetAllAsync() => this.GetAsync(e => true);

        public async Task<IEnumerable<T>> GetAsync(Func<T, bool> predicate)
        {
            if (!F.Exists(this.FullPath))
            {
                return Array.Empty<T>();
            }

            var serializedElements = await F.ReadAllTextAsync(this.FullPath);
            var elements = JsonConvert.DeserializeObject<IEnumerable<T>>(serializedElements).Where(predicate);

            return elements;
        }

        public Task UpdateAsync(T element)
        {
            throw new NotImplementedException();
        }

        public static void EnsureFolderExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
