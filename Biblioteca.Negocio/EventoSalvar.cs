using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio
{
    [Serializable]
    public abstract class EventoSalvar
    {

        public EventoSalvar()
        {

        }

        public void RestaurarEventoCache(Memento memento)
        {
            EventoSalvar eve = memento.RecuperarEventoCache();
            CommonBC.Syncronize(eve, this);
        }

        public Memento CrearMementoEventoCache()
        {
            Memento memento = new Memento();
            memento.SalvarEventoCache(this);
            return memento;
        }

        
    }
}
