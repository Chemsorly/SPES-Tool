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
        Dictionary<String, Type> ShapeAssignments = new Dictionary<string, Type>();

        public void AddAssignment(String pFilename, Type pType)
        {
            ShapeAssignments.Add(pFilename, pType);
        }

        public Type GetTypeFromFile(String pFile)
        {
            return ShapeAssignments.FirstOrDefault(t => t.Key == pFile).Value;
        }

        public void LoadConfigFromFile(String pPath)
        {
            if (File.Exists(pPath))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Dictionary<String, Type>));
                using (FileStream fs = new FileStream(pPath, FileMode.OpenOrCreate))
                {
                    ShapeAssignments = (Dictionary<String, Type>) deserializer.Deserialize(fs);
                    fs.Close();
                }
            }
        }

        public void SaveConfigToFile(String pPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<String, Type>));
            using (FileStream fs = new FileStream(pPath,FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs,ShapeAssignments);
                fs.Close();
            }
        }
    }
}
