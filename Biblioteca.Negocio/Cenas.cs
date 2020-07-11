using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Biblioteca.Negocio
{
    public class Cenas : Evento
    {
        public string Numero { get; set; }
        public int IdTipoAmbientacion { get; set; }
        public bool MusicaAmbiental { get; set; }
        public bool LocalOnBreak { get; set; }
        public bool OtroLocalOnBreak { get; set; }
        public double ValorArriendo { get; set; }
        public Cenas()
        {

        }

        OnBreakEntities bdd = new OnBreakEntities();

        public bool Create()
        {
            try
            {
                DALC.Cenas c = new DALC.Cenas();
                CommonBC.Syncronize(this, c);
                bdd.Cenas.Add(c);
                bdd.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                return false;
            }
        }

        public bool Update()
        {
            try
            {
                DALC.Cenas c = bdd.Cenas.Find(this.Numero);
                Contrato con = new Contrato() { Numero = this.Numero };
                con.Read();
                if (con.Realizado)
                {
                    CommonBC.Syncronize(this, c);
                    bdd.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

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
                DALC.Cenas c = bdd.Cenas.Find(this.Numero);
                bdd.Cenas.Remove(c);
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
                DALC.Cenas c = bdd.Cenas.Find(this.Numero);
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
                recargo = 1.5*ra;
            }
            else if (ra >= 21 && ra <= 50)
            {
                recargo = 1.2*ra;
            }
            else if (ra > 50)
            {
                recargo = 1 * ra;
            }
            return recargo;
        }

        public override double RecargoPersonalAdicional()
        {
            double recargo = 0;
            int pa = base.cont.PersonalAdicional;
            if (pa == 2)
            {
                recargo = 3;
            }
            else if (pa == 3)
            {
                recargo = 4;
            }
            else if (pa == 4)
            {
                recargo = 5;
            }
            else if (pa > 4)
            {
                recargo = 5 + ((pa - 4) * 0.5);
            }
            return recargo;
        }

        public override double RecargoExtras()
        {
            double ambientacion = 0;
            double musica_ambiental = 0;
            double local_recargo = 0;
            TipoAmbientacion ta = new TipoAmbientacion();
            ta.idTipoAmbientacion = this.IdTipoAmbientacion;
            ta.Read();
            if (ta.Descripcion == null) //posiblemente se cambie
            {
                ambientacion = 0;
            }
            else if (ta.Descripcion.Equals("Básica"))
            {
                ambientacion = 3;
            }
            else if (ta.Descripcion.Equals("Personalizada"))
            {
                ambientacion = 5;
            }
            if (this.MusicaAmbiental)
            {
                musica_ambiental = 1.5;
            }
            if (this.LocalOnBreak)
            {
                local_recargo = 9;
            }
            if (this.OtroLocalOnBreak)
            {
                local_recargo = this.ValorArriendo * 0.05; //Falta pasar de peso a euro aca?
            }
            return ambientacion + musica_ambiental + local_recargo;
        }
    }
}
