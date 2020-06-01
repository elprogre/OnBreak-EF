using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Biblioteca.Negocio
{
    public class ActividadEmpresa
    {
        public int IdActividadEmpresa { get; set; }
        public string Descripcion { get; set; }

        public ActividadEmpresa()
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
                DALC.ActividadEmpresa act = 
                    bdd.ActividadEmpresa.First(a => a.IdActividadEmpresa == IdActividadEmpresa);
                this.IdActividadEmpresa = act.IdActividadEmpresa;
                this.Descripcion = act.Descripcion;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Negocio.ActividadEmpresa> ReadAll()
        {
            try
            {
                List<Negocio.ActividadEmpresa> lista_clase_actividad = new List<ActividadEmpresa>();
                List<DALC.ActividadEmpresa> lista_actividad = bdd.ActividadEmpresa.ToList();
                foreach (DALC.ActividadEmpresa item in lista_actividad)
                {
                    Negocio.ActividadEmpresa act = new ActividadEmpresa();
                    act.IdActividadEmpresa = item.IdActividadEmpresa;
                    act.Descripcion = item.Descripcion;
                    lista_clase_actividad.Add(act);
                }
                return lista_clase_actividad;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
