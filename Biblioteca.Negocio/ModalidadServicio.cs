using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Biblioteca.Negocio
{
    public class ModalidadServicio
    {
        public string IdModalidad { get; set; }
        public int IdTipoEvento { get; set; }
        public string Nombre { get; set; }
        public double ValorBase { get; set; }
        public int PersonalBase { get; set; }

        public ModalidadServicio()
        {

        }

        OnBreakEntities bdd = new OnBreakEntities();


        //falta verificar si esto esta bien///
        public bool Read()
        {
            try
            {
                DALC.ModalidadServicio mod =
                bdd.ModalidadServicio.First(m => m.IdModalidad == IdModalidad);
                this.IdModalidad = mod.IdModalidad;
                this.IdTipoEvento = mod.IdTipoEvento;
                this.Nombre = mod.Nombre;
                this.ValorBase = mod.ValorBase;
                this.PersonalBase = mod.PersonalBase;
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


        public List<Negocio.ModalidadServicio> ReadAll()
        {
            try
            {
                List<DALC.ModalidadServicio> lista_modalidad = bdd.ModalidadServicio.ToList();
                List<Negocio.ModalidadServicio> lista_clase_modalidad = new List<ModalidadServicio>();
                foreach (DALC.ModalidadServicio item in lista_modalidad)
                {
                    Negocio.ModalidadServicio modalidad = new ModalidadServicio();
                    modalidad.IdModalidad = item.IdModalidad;
                    modalidad.IdTipoEvento = item.IdTipoEvento;
                    modalidad.Nombre = item.Nombre;
                    modalidad.ValorBase = item.ValorBase;
                    modalidad.PersonalBase = item.PersonalBase;
                    lista_clase_modalidad.Add(modalidad);
                }
                return lista_clase_modalidad;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
