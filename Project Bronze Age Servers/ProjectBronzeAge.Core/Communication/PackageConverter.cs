using NetSerializer;
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
        private static Serializer serializer = new Serializer(new Type[] { typeof(ClientDataAuthPackage), typeof(ClientDataChangePackage), typeof(ClientPlayRequestPackage), typeof(ServerDataAuthPackage), typeof(ServerDataChangePackage) });

        public static byte[] ConvertToByteArray<T>(T package) where T : IPackage
        {
            using (var mStream = new MemoryStream())
            {
                serializer.Serialize(mStream, package);
                return mStream.ToArray();
            }
        }

        public static T ConvertFromByteArray<T>(byte[] data) where T : IPackage
        {
            using (var mStream = new MemoryStream(data))
            {
                return (T)serializer.Deserialize(mStream);
            }
        }
    }
}
