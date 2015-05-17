using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public class Quest : Unit, IResourceID<Quest>
    {
        public string name, description;
        public QuestTaskType taskType;
        public QuestEventType eventType;
    
        public int resourceId
        {
            get;
            set;
        }

        public Quest Clone()
        {
 	        return new Quest() { name = name, description = description, taskType = taskType, eventType = eventType };
        }
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
