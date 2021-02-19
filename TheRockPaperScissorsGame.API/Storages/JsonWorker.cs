using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TheRockPaperScissorsGame.API.Storages
{
    public class JsonWorker<T> // can add a restriction like where T:class
    {
        static SemaphoreSlim _lockSlim = new SemaphoreSlim(1, 1);
        private readonly string _path;

        public JsonWorker(string path)
        {
            _path = path;
        }

        public async Task<List<T>> ReadDataFromFile()
        {
            await _lockSlim.WaitAsync();
            try
            {
                if (!File.Exists(_path))
                {
                    throw new FileNotFoundException();
                }

                var json = await File.ReadAllTextAsync(_path);

                var data = JsonSerializer.Deserialize<List<T>>(json);

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

        public async Task WriteDataIntoFile(T obj)
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

