using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ProjectBronzeAge.Core.Communication
{
    public static class PackageConverter
    {
        private static BinaryFormatter binaryFormatter = new BinaryFormatter();

        public static byte[] ConvertToByteArray<T>(T package) where T : IPackage
        {
            // Initialize streams
            MemoryStream stream = new MemoryStream();
            // Run the conversion
            binaryFormatter.Serialize(stream, package);

            // Return the array
            return stream.ToArray();
        }

        public static T ConvertFromByteArray<T>(byte[] data) where T : IPackage
        {
            // Initialize streams
            MemoryStream stream = new MemoryStream(data);
            // Return instance
            return (T)binaryFormatter.Deserialize(stream);
        }
    }
}
