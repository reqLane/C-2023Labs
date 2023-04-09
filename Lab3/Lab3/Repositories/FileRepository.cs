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

        public List<Person> GetAllPersons()
        {
            var res = new List<Person>();
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                string stringObj = "";

                using (StreamReader sw = new StreamReader(file))
                {
                    stringObj = sw.ReadToEnd();
                }

                Person? deserialized = JsonSerializer.Deserialize<Person>(stringObj);

                if (deserialized != null)
                    res.Add(deserialized);
            }

            return res;
        }

        public async Task RewriteConfig(List<Person> newCollection)
        {
            foreach (var file in Directory.EnumerateFiles(BaseFolder))
            {
                File.Delete(file);
            }

            foreach (var person in newCollection)
            {
                string filePath = Path.Combine(BaseFolder, person.Email);
                if (File.Exists(filePath))
                    throw new Exception("Person already exists");

                var stringObj = JsonSerializer.Serialize(person);

                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    await sw.WriteAsync(stringObj);
                }
            }
        }
    }
}
