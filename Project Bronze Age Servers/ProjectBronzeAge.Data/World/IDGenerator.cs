using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public static class IDGenerator
    {
        private static int currentIdNumber;

        public static int NewId()
        {
            if (currentIdNumber > 21474836)
                currentIdNumber = 0;
            currentIdNumber += 1;
            return currentIdNumber;
        }
    }
}
