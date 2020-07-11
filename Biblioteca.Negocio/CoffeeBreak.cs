using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Biblioteca.Negocio
{
    public class CoffeeBreak : Evento
    {
        public string Numero { get; set; }
        public bool Vegetariana { get; set; }
        public CoffeeBreak()
        {

        }

        OnBreakEntities bdd = new OnBreakEntities();

        public bool Create()
        {
            try
            {
                DALC.CoffeeBreak c = new DALC.CoffeeBreak();
                CommonBC.Syncronize(this, c);
                bdd.CoffeeBreak.Add(c);
                bdd.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                return false;
            }
        }

        public bool Delete()
        {
            try
            {
                DALC.CoffeeBreak c = bdd.CoffeeBreak.Find(this.Numero);
                bdd.CoffeeBreak.Remove(c);
                bdd.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                return false;
            }
        }

        public bool Read()
        {
            try
            {
                DALC.CoffeeBreak c = bdd.CoffeeBreak.Find(this.Numero);
                CommonBC.Syncronize(c, this);
                bdd.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                return false;
            }
        }

        public override double ValorBase()
        {
            string modalidad = base.cont.IdModalidad;
            ModalidadServicio m = new ModalidadServicio();
            m.IdModalidad = modalidad;
            m.Read();
            double valor_base = m.ValorBase;
            return valor_base;
        }

        public override double RecargoAsistentes()
        {
            double recargo = 0;
            int ra = base.cont.Asistentes;
            if (ra >= 1 && ra <= 20)
            {
                recargo = 3;
            }
            else if (ra >= 21 && ra <= 50)
            {
                recargo = 5;
            }
            else if (ra > 50)
            {
                recargo = 5 + (((ra - 50) / 20) * 2);
            }
            return recargo;
        }

        public override double RecargoPersonalAdicional()
        {
            double recargo = 0;
            int pa = base.cont.PersonalAdicional;
            if (pa == 2)
            {
                recargo = 2;
            }
            else if (pa == 3)
            {
                recargo = 3;
            }
            else if (pa > 3)
            {
                recargo = 3 + ((pa - 3) * 0.5);
            }
            return recargo;
        }

        public override double RecargoExtras()
        {
            return 0;
        }
    }
}
