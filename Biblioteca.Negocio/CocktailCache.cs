using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio
{
    [Serializable]
    public class CocktailCache : EventoSalvar
    {
        public string Numero { get; set; }
        public int IdTipoAmbientacion { get; set; }
        public bool Ambientacion { get; set; }
        public bool MusicaAmbiental { get; set; }
        public bool MusicaCliente { get; set; }
        public CocktailCache()
        {

        }
    }
}
