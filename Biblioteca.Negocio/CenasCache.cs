using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio
{
    [Serializable]
    public class CenasCache : EventoSalvar
    {
        public string Numero { get; set; }
        public int IdTipoAmbientacion { get; set; }
        public bool MusicaAmbiental { get; set; }
        public bool LocalOnBreak { get; set; }
        public bool OtroLocalOnBreak { get; set; }
        public double ValorArriendo { get; set; }
        public CenasCache()
        {

        }
    }
}
