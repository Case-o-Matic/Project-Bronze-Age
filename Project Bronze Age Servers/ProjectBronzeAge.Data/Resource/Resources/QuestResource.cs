using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class QuestResource : IResourceID<Quest>
    {
        public string name, description;

        public QuestTaskType taskType;
        public QuestEventType eventType;

        public int enemyActorId;
        public int collectionItemId, collectionCount;

        public int resourceId
        {
            get;
            set;
        }

        public Quest Create()
        {
            return new Quest() { name = name, description = description, taskType = taskType, eventType = eventType, collecionItemId = collectionItemId, collectionCount = collectionCount, enemyActorId = enemyActorId };
        }
    }
}
