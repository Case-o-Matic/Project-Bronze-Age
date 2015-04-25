using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectBronzeAge.Core
{
    public class GameUserMessage
    {
        public GameUserMessageType type;
        public object data;

        public GameUserMessage(GameUserMessageType type, object data)
        {
            this.type = type;
            this.data = data;
        }

        public static byte[] ToBytes(GameUserMessage message)
        {
            using (var mStream = new MemoryStream())
            {
                Serializer.Serialize<GameUserMessage>(mStream, message);
                return mStream.ToArray();
            }
        }
        public static GameUserMessage ToMessage(byte[] bytes)
        {
            using (var mStream = new MemoryStream(bytes))
            {
                return Serializer.Deserialize<GameUserMessage>(mStream);
            }
        }

        public enum GameUserMessageType
        {
            Login = 1,
            Logout = 2,
            GetUserData = 3,
            SetUserData = 4
        }
    }
}
