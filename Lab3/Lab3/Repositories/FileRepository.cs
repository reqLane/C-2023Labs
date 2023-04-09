using Lab3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab3.Repositories
{
    internal class FileRepository
    {
        private static readonly string BaseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PersonsStorage");

        public FileRepository()
        {
            if (!Directory.Exists(BaseFolder))
                Directory.CreateDirectory(BaseFolder);
        }

        public async Task AddOrUpdateAsync(Person obj)
        {
            var stringObj = JsonSerializer.Serialize(obj);

            using (StreamWriter sw = new StreamWriter(Path.Combine(BaseFolder, obj.Email), false))
            {
                await sw.WriteAsync(stringObj);
            }
        }

        public async Task<List<Person>> GetAllAsync()
        {
            var res = new List<Person>();
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string stringObj = "";

                using (StreamReader sw = new StreamReader(file))
                {
                    stringObj = await sw.ReadToEndAsync();
                }

                Person? deserialized = JsonSerializer.Deserialize<Person>(stringObj);

                if (deserialized != null)
                    res.Add(deserialized);
            }

            return res;
        }
    }
}
