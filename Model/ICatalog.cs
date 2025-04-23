using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.Model
{
    public interface ICatalog
    {
        string Name { get; }

        bool IsEditable { get; }
    }
}
