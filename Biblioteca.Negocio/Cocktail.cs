using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Biblioteca.Negocio
{
    public class Cocktail : Evento
    {
        public string Numero { get; set; }
        public int IdTipoAmbientacion { get; set; }
        public bool Ambientacion { get; set; }
        public bool MusicaAmbiental { get; set; }
        public bool MusicaCliente { get; set; }
        public Cocktail()
        {

        }

        OnBreakEntities bdd = new OnBreakEntities();

        public bool Create()
        {
            try
            {
                DALC.Cocktail c = new DALC.Cocktail();
                CommonBC.Syncronize(this,c);
                bdd.Cocktail.Add(c);
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
                DALC.Cocktail c = bdd.Cocktail.Find(this.Numero);
                bdd.Cocktail.Remove(c);
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
                DALC.Cocktail c = bdd.Cocktail.Find(this.Numero);
                CommonBC.Syncronize(c,this);
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
            double ambientacion = 0;
            double musica_ambiental = 0;
            string modalidad = base.cont.IdModalidad;
            ModalidadServicio m = new ModalidadServicio();
            m.IdModalidad = modalidad;
            m.Read();
            double valor_base = m.ValorBase;
            ////// tipo de ambientacion
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
            ////// musica ambiental
            if (MusicaAmbiental)
            {
                musica_ambiental = 1;
            }
            return ambientacion + musica_ambiental + valor_base;
        }

        public override double RecargoAsistentes()
        {
            double recargo = 0;
            int ra = base.cont.Asistentes;
            if (ra>=1 && ra<=20)
            {
                recargo = 4;
            }
            else if (ra >= 21 && ra <= 50)
            {
                recargo = 6;
            }
            else if (ra > 50)
            {
                recargo = ra / 20;
                recargo = Math.Truncate(recargo)*2;
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
    }
}
