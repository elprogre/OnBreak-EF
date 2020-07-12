using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio
{
    [Serializable]
    public class CoffeeBreakCache : EventoSalvar
    {
        public string Numero { get; set; }
        public bool Vegetariana { get; set; }
        public CoffeeBreakCache()
        {

        }
    }
}
