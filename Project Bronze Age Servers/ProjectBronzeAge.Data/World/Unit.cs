using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public abstract class Unit : INetworkID
    {
        public int networkId
        {
            get;
            private set;
        }

        public virtual void Start()
        {

        }
        public virtual void Update(float deltatime)
        {

        }
    }
}
