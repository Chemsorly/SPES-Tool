using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SPES_App
{
    public class SPES_DocumentReferencer
    {
        /// <summary>
        /// contains a mapping for [Filename,Moduletype]
        /// </summary>
        Dictionary<String, String> ShapeAssignments = new Dictionary<String, String>();

        public void AddAssignment(String pFilename, String pType)
        {
            if(ShapeAssignments.Any(t => t.Key == pFilename))
                Console.WriteLine($"{pFilename} already exists in ShapeAssignments");

            ShapeAssignments.Add(pFilename, pType);
        }

        public String GetTypeFromFile(String pFile)
        {
            return ShapeAssignments.FirstOrDefault(t => t.Key == pFile).Value;
        }

        public void LoadConfigFromFile(String pPath)
        {
            if (File.Exists(pPath))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<Entry>));
                using (FileStream fs = new FileStream(pPath, FileMode.OpenOrCreate))
                {
                    List<Entry> entries = (List<Entry>) deserializer.Deserialize(fs);
                    fs.Close();
                    foreach (var entry in entries)
                        this.AddAssignment(entry.Key, entry.Value);
                }
            }
        }

        public void SaveConfigToFile(String pPath)
        {
            List<Entry> entries = new List<Entry>(ShapeAssignments.Count);
            foreach (String key in ShapeAssignments.Keys)
            {
                entries.Add(new Entry(key, ShapeAssignments[key]));
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<Entry>));
            using (FileStream fs = new FileStream(pPath,FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, entries);
                fs.Close();
            }
        }

        public class Entry
        {
            public String Key;
            public String Value;
            public Entry()
            {
            }

            public Entry(String key, String value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}
