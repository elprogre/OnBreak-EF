using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Negocio
{
    public abstract class Evento
    {
        protected Contrato cont;

        public void TipoContrato(Contrato cont)
        {
            this.cont = cont;
        }
        public abstract double ValorBase();
        public abstract double RecargoAsistentes();
        public abstract double RecargoPersonalAdicional();
        public abstract double RecargoExtras();
    }
}
