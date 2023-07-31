using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace SpinalPlay
{
    public class SaveLoadProgressService : IService
    {
        private readonly string filePath;
        private string jsonString;

        public SaveLoadProgressService(string fileName)
        {
            filePath = Application.persistentDataPath + $"/{fileName}.save";
        }

        public T Load<T>(T defaultLoadData)
        {
            if (File.Exists(filePath))
            {
                jsonString = File.ReadAllText(filePath);
                T loadData = JsonConvert.DeserializeObject<T>(jsonString);
                return loadData;
            }
            else
            {
                return defaultLoadData;
            }
        }

        public void Save(object saveData)
        {
            string json = JsonConvert.SerializeObject(saveData);
            File.WriteAllText(filePath, json);

        }
    }
}