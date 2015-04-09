using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBronzeAge.Central
{
    public struct GameInfo
    {
        public string[] characterIds;

        public GameInfo(string[] characterids)
        {
            characterIds = characterids;
        }
    }
}
