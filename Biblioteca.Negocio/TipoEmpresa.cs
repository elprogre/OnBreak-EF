using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Biblioteca.Negocio
{
    public class TipoEmpresa
    {
        public int IdTipoEmpresa { get; set; }
        public string Descripcion { get; set; }

        public TipoEmpresa()
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
                DALC.TipoEmpresa tip =
                bdd.TipoEmpresa.First(t => t.IdTipoEmpresa == IdTipoEmpresa);
                this.IdTipoEmpresa = tip.IdTipoEmpresa;
                this.Descripcion = tip.Descripcion;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Negocio.TipoEmpresa> ReadAll()
        {
            try
            {
                List<Negocio.TipoEmpresa> lista_clase_tipoe = new List<TipoEmpresa>();
                List<DALC.TipoEmpresa> lista_tipoe = bdd.TipoEmpresa.ToList();
                foreach (DALC.TipoEmpresa item in lista_tipoe)
                {
                    Negocio.TipoEmpresa tipoe= new TipoEmpresa();
                    tipoe.IdTipoEmpresa = item.IdTipoEmpresa;
                    tipoe.Descripcion = item.Descripcion;
                    lista_clase_tipoe.Add(tipoe);
                }
                return lista_clase_tipoe;
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}
