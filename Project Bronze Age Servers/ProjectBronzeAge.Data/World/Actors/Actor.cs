using ProjectBronzeAge.Core.Communication.Play;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public abstract class Actor : Unit
    {
        public bool isStatic;

        public ServerPlayEventPackage nextEventPackage;
        public ServerPlayStatePackage nextStatePackage;

        private Vector3 _position;
        private Vector3 _rotation;

        public Vector3 position
        {
            get
            {
                return _position;
            }
            set
            {
                if(!isStatic)
                    _position = value;
            }
        }
        public Vector3 rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                if(isStatic)
                    _rotation = value;
            }
        }

        public override void Update(float deltatime)
        {
            if (isDirty)
            {
                nextStatePackage.posX = position.x;
                nextStatePackage.posY = position.y;
                nextStatePackage.posZ = position.z;

                nextStatePackage.rotX = rotation.x;
                nextStatePackage.rotY = rotation.y;
                nextStatePackage.rotZ = rotation.z;
            }

            base.Update(deltatime);
        }
    }
}
