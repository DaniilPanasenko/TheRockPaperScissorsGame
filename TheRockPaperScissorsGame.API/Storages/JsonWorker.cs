using System;
using System.IO;
using System.Text.Json;

namespace TheRockPaperScissorsGame.API.Storages
{
    public class JsonWorker
    {
        private readonly string _path;

        public JsonWorker(string path)
        {
            _path = path;
        }

        public T ReadDataFromFile<T>()
        {
            if (!File.Exists(_path))
            {
                throw new FileNotFoundException();
            }

            var json = File.ReadAllText(_path);
            var data = JsonSerializer.Deserialize<T>(json);

            if (data == null)
            {
                throw new ArgumentNullException();
            }

            return data;
        }

        // hm.. obj seems to be a bad name
        public void WriteDataIntoFile<T>(T obj)
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
            File.WriteAllText(_path, json);
        }
    }
}
