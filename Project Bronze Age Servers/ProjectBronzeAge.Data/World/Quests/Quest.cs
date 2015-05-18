using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class Quest : Unit
    {
        public string name, description;
        public QuestTaskType taskType;
        public QuestEventType eventType;

        public int collecionItemId, collectionCount;
        public int enemyActorId;
    }

    public enum QuestTaskType
    {
        CollectItems,
        KillEnemies
        // Add more
    }
    public enum QuestEventType
    {
        Normal,
        Season,
        Daily,
    }
}
