using Fresh.Windows.Core.Services.Interfaces;
using Newtonsoft.Json;
using PCLStorage;
using System.Threading.Tasks;
using Fresh.Windows.Core.Models;
using System;

namespace Fresh.Windows.Core.Services
{
    public class PCLStorageService : IStorageService
    {
        private readonly IFolder localStorage;

        public PCLStorageService()
        {
            localStorage = FileSystem.Current.LocalStorage;
        }

        public async Task<bool> HasKey(string key)
        {
            ExistenceCheckResult file = await localStorage.CheckExistsAsync(string.Format("{0}.json", key));

            return file == ExistenceCheckResult.FileExists;
        }

        public async Task Save<T>(string key, T entity)
        {
            key = string.Format("{0}.json", key);

            var file = await localStorage.CreateFileAsync(key, CreationCollisionOption.ReplaceExisting);

            await file.WriteAllTextAsync(JsonConvert.SerializeObject(entity));
        }

        public async Task<T> Get<T>(string key)
        {
            var file = await localStorage.GetFileAsync(string.Format("{0}.json", key));

            return JsonConvert.DeserializeObject<T>(await file.ReadAllTextAsync());
        }

        public Task<User> GetUserAsync()
        {
            throw new NotImplementedException();
        }
    }
}
