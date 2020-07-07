using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Biblioteca.Negocio
{
    public class TipoAmbientacion
    {
        public int idTipoAmbientacion { get; set; }
        public string Descripcion { get; set; }
        public TipoAmbientacion()
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
                DALC.TipoAmbientacion ta = bdd.TipoAmbientacion.Find(this.idTipoAmbientacion);
                this.Descripcion = ta.Descripcion;
                return true;
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                return false;
            }
        }

        public List<TipoAmbientacion> ReadAll()
        {
            try
            {
                List<TipoAmbientacion> lista_clase = new List<TipoAmbientacion>();
                foreach (var item in bdd.TipoAmbientacion.ToList())
                {
                    TipoAmbientacion ta = new TipoAmbientacion();
                    ta.idTipoAmbientacion = item.IdTipoAmbientacion;
                    ta.Descripcion = item.Descripcion;
                    lista_clase.Add(ta);
                }
                return lista_clase;
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                return null;
            }
        }

    }
}
