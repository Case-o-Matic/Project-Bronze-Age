﻿using ProjectBronzeAge.Core.Communication;
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

        public bool isStateDirty
        {
            get;
            private set;
        }

        public Vector3 position
        {
            get
            {
                return _position;
            }
            set
            {
                if (!isStatic)
                {
                    _position = value;
                    SetDirtyState(true);
                }
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
                if (!isStatic)
                {
                    _rotation = value;
                    SetDirtyState(true);
                }
            }
        }

        public override void Update(float deltatime)
        {
            if (isStateDirty)
            {
                nextStatePackage.posX = position.x;
                nextStatePackage.posY = position.y;
                nextStatePackage.posZ = position.z;

                nextStatePackage.rotX = rotation.x;
                nextStatePackage.rotY = rotation.y;
                nextStatePackage.rotZ = rotation.z;

                SetDirtyState(false);
            }

            base.Update(deltatime);
        }

        protected void SetDirtyState(bool value)
        {
            isStateDirty = true;
        }
    }
}
