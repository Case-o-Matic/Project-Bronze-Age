using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public interface IResourceID<T>
    {
        int resourceId { get; set; }
        T Clone();
    }
}
