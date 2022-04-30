using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StoringDataExamples.Business.JsonStorage
{
    public static class JsonFileHelper
    {
        public static async Task<T> ReadAsync<T>(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            var items = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(stream);
            return items;
        }

        public static async Task WriteAsync<T>(string filePath, T items)
        {
            var json = JsonConvert.SerializeObject(items, Formatting.Indented);
            using StreamWriter file = new(filePath);
            await file.WriteAsync(json);
        }
    }
}
