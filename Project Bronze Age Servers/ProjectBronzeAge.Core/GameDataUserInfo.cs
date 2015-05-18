using System;
using ProtoBuf;
using System.IO;

namespace ProjectBronzeAge.Core
{
    public class GameDataUserInfo
    {
        public GameDataUserCharacterInfo[] characters;
        public GameDataUserAccountInfo account;

        public GameDataUserInfo(GameDataUserCharacterInfo[] characters, GameDataUserAccountInfo account)
        {
            this.characters = characters;
            this.account = account;
        }
        public GameDataUserInfo()
        { }

        public class GameDataUserCharacterInfo
        {
            public string name;

            public string currentWorldInstance;
            public float posX, posY, posZ;
            public string[] abilities, items;

            public bool genderStyle; // True = male
            public int hairStyleIndex, bodyStyleIndex;

            public int currentLevel, currentXp;

            public GameDataUserCharacterInfo(string name, string currentworldinstance, float posx, float posy, float posz, string[] abilities, string[] items, bool genderstyle, int hairstyleindex, int bodystyleindex,  int currentlevel, int currentxp)
            {
                this.name = name;
                this.currentWorldInstance = currentworldinstance;
                this.posX = posx;
                this.posY = posy;
                this.posZ = posz;
                this.abilities = abilities;
                this.items = items;
                this.genderStyle = genderstyle;
                this.hairStyleIndex = hairstyleindex;
                this.bodyStyleIndex = bodystyleindex;
                this.currentLevel = currentlevel;
                this.currentXp = currentxp;
            }

            public static byte[] ToBytes(GameDataUserCharacterInfo character)
            {
                using (var mstream = new MemoryStream())
                {
                    Serializer.Serialize<GameDataUserCharacterInfo>(mstream, character);
                    return mstream.ToArray();
                }
            }
            public static GameDataUserCharacterInfo FromBytes(byte[] bytes)
            {
                using (var mstream = new MemoryStream(bytes))
                {
                    return Serializer.Deserialize<GameDataUserCharacterInfo>(mstream);
                }
            }
        }
        public class GameDataUserAccountInfo
        {
            public GameUserAccountInfoRankType playerType;
            public int maxAvailableCharacterSlots;

            public GameDataUserAccountInfo(GameUserAccountInfoRankType playertype, int maxavailablecharacterslots)
            {
                playerType = playertype;
                maxAvailableCharacterSlots = maxavailablecharacterslots;
            }

            public enum GameUserAccountInfoRankType
            {
                Normal = 1,
                GameMaster = 2,
                Administrator = 3
            }
        }

        public static byte[] ToBytes(GameDataUserInfo gamedatauserinfo)
        {
            using (var mstream = new MemoryStream())
            {
                Serializer.Serialize<GameDataUserInfo>(mstream, gamedatauserinfo);
                return mstream.ToArray();
            }
        }
        public static GameDataUserInfo FromBytes(byte[] bytes)
        {
            using (var mstream = new MemoryStream(bytes))
            {
                return Serializer.Deserialize<GameDataUserInfo>(mstream);
            }
        }
    }
}
