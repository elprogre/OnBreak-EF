using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Biblioteca.Negocio
{
    public class TipoEvento
    {
        public int IdTipoEvento { get; set; }
        public string Descripcion { get; set; }

        public TipoEvento()
        {

        }

        public override string ToString()
        {
            return Descripcion;
        }

        OnBreakEntities bdd = new OnBreakEntities();

        public bool Read()
        {
            try
            {
                DALC.TipoEvento tipoev = 
                    bdd.TipoEvento.First(te => te.IdTipoEvento == this.IdTipoEvento);
                this.IdTipoEvento = tipoev.IdTipoEvento;
                this.Descripcion = tipoev.Descripcion;
                return true;
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                return false;
            }
        }

        public List<Negocio.TipoEvento> ReadAll()
        {
            try
            {
                List<Negocio.TipoEvento> lista_clase_tipoev = new List<Negocio.TipoEvento>();
                List<DALC.TipoEvento> lista_tipoev = bdd.TipoEvento.ToList();
                foreach (DALC.TipoEvento item in lista_tipoev)
                {
                    Negocio.TipoEvento tipoev = new TipoEvento();
                    tipoev.IdTipoEvento = item.IdTipoEvento;
                    tipoev.Descripcion = item.Descripcion;
                    lista_clase_tipoev.Add(tipoev);
                }
                return lista_clase_tipoev;
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                return null;
            }
        }

    }
}
