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
            double ambientacion;
            double musica_ambiental = 0;
            double local_recargo = 0;
            string modalidad = base.cont.IdModalidad;
            ModalidadServicio m = new ModalidadServicio();
            m.IdModalidad = modalidad;
            m.Read();
            double valor_base = m.ValorBase;
            TipoAmbientacion ta = new TipoAmbientacion();
            ta.idTipoAmbientacion = this.IdTipoAmbientacion;
            ta.Read();
            if (ta.Descripcion.Equals("Básica"))
            {
                ambientacion = 2;
            }
            else if (ta.Descripcion.Equals("Personalizada"))
            {
                ambientacion = 5;
            }
            else
            {
                throw new Exception("La ambientación es obligatoria.");
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
                local_recargo = this.ValorArriendo + (this.ValorArriendo * 0.05);
            }
            return valor_base + ambientacion + musica_ambiental + local_recargo;
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
                recargo = ra / 20;
                recargo = Math.Truncate(recargo) * 2;
            }
            return recargo;
        }

        public override double RecargoPersonalAdicional()
        {
            throw new NotImplementedException();
        }
    }
}
