using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TheRockPaperScissorsGame.API.Storages
{
    public class JsonWorker
    {
        static SemaphoreSlim _lockSlim = new SemaphoreSlim(1, 1);
        private readonly string _path;

        public JsonWorker(string path)
        {
            _path = path;
        }

        public async Task<T> ReadDataFromFile<T>()
        {
            await _lockSlim.WaitAsync();
            try
            {
                if (!File.Exists(_path))
                {
                    throw new FileNotFoundException();
                }

                var json = await File.ReadAllTextAsync(_path);

                var data = JsonSerializer.Deserialize<T>(json);

                if (data == null)
                {
                    throw new ArgumentNullException();
                }

                return data;
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        public async Task WriteDataIntoFile<T>(T obj)
        {
            await _lockSlim.WaitAsync();
            try
            {
                if (!File.Exists(_path))
                {
                    File.Create(_path).Close();
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(obj, options);
                await File.AppendAllTextAsync(_path, json);
            }
            finally
            {
                _lockSlim.Release();
            }
        }
    }
}

