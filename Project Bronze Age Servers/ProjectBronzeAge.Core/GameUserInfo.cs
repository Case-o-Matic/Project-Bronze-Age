using System;

namespace ProjectBronzeAge.Core
{
    public class GameUserInfo
    {
        public string userId;

        public GameUserCharacterInfo[] characters;
        public GameUserAccountInfo account;

        public GameUserInfo(string userid, GameUserCharacterInfo[] characters, GameUserAccountInfo account)
        {
            this.userId = userid;
            this.characters = characters;
            this.account = account;
        }
        public GameUserInfo()
        { }

        public struct GameUserCharacterInfo
        {
            public string name, characterId;
            public int level;

            public GameUserCharacterInfo(string name, string characterid, int level)
            {
                this.name = name;
                this.characterId = characterid;
                this.level = level;
            }
        }
        public struct GameUserAccountInfo
        {
            public GameUserAccountInfoPlayerType playerType;
            public int maxAvailableCharacterSlots;

            public GameUserAccountInfo(GameUserAccountInfoPlayerType playertype, int maxavailablecharacterslots)
            {
                playerType = playertype;
                maxAvailableCharacterSlots = maxavailablecharacterslots;
            }

            public enum GameUserAccountInfoPlayerType
            {
                Normal = 1,
                GameMaster = 2,
                Administrator = 3
            }
        }
    }
}
