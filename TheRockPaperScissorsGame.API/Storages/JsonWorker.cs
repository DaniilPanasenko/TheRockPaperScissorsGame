using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace TheRockPaperScissorsGame.API.Storages
{
    public class JsonWorker<T> where T : class
    {
        static SemaphoreSlim _lockSlim = new SemaphoreSlim(1, 1);
        private readonly string _path;

        public JsonWorker(string path)
        {
            _path = path;
        }

        public async Task<List<T>> ReadDataFromFileAsync()
        {
            await _lockSlim.WaitAsync();
            try
            {
                if (!File.Exists(_path))
                {
                    // For test
                    #region Very important thought!!!!
                    // Or maybe should do this way... 

                    //Like to create a base, if somebody else will download our project,
                    //he wouldn't have this file, but if the person, who will download
                    //our project, NEED to have the file with all data we need to create
                    //the file in the VS and set setting like content and copy if newer or copy everytime

                    //then these files will added in the rar 
                    #endregion

                    File.Create(_path).Close();
                    // For test!

                    //throw new FileNotFoundException();
                }

                var json = await File.ReadAllTextAsync(_path);

                List<T> data;

                if (json.Length == 0)
                {
                    data = new List<T>();
                }
                else
                {
                    data = JsonSerializer.Deserialize<List<T>>(json);
                }

                return data;
            }
            finally
            {
                _lockSlim.Release();
            }
        }

        public async Task WriteDataIntoFileAsync(List<T> listObjects)
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
                //    Converters =
                //{
                //  new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                //}
                };

                var json = JsonSerializer.Serialize(listObjects, options);
                await File.WriteAllTextAsync(_path, json);
            }
            finally
            {
                _lockSlim.Release();
            }
        }
    }
}

