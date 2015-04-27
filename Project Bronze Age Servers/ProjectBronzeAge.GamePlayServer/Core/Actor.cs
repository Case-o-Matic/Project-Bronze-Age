using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.GamePlayServer.Core
{
    public abstract class Actor
    {
        public string pemanentId { get; private set; }
        public Vector3 position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                SetDirty();
            }
        }
        private Vector3 _position;

        public Vector3 rotation // Should a quaternion be used for the rotation?
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                SetDirty();
            }
        }
        private Vector3 _rotation;

        public bool isDirty { get; protected set; }

        protected Actor()
        {
            pemanentId = Guid.NewGuid().ToString();
        }


        public virtual void Update()
        {

        }
        protected void SetDirty()
        {
            isDirty = true;
        }
    }
}
