using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio
{
    interface IMetodos //es para ahorrar tiempo
    {
        bool Create();
        bool Read();
        bool Update();
        bool Delete();

    }
}
