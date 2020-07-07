using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Biblioteca.Negocio
{
    [Serializable]
    public class ContratoSalvar 
    {
        public string Numero { get; set; }

        public DateTime Creacion { get; set; }

        public DateTime Termino { get; set; }

        public string RutCliente { get; set; }

        public string IdModalidad { get; set; }

        public int IdTipoEvento { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraTermino { get; set; }

        public int Asistentes { get; set; }

        public int PersonalAdicional { get; set; }

        public bool Realizado { get; set; }

        public double ValorTotalContrato { get; set; }

        public string Observaciones { get; set; }

        public ContratoSalvar()
        {

        }

        public void Restaurar(Memento memento)
        {
            ContratoSalvar cont = memento.Recuperar();
            CommonBC.Syncronize(cont, this);
        }

        public Memento CrearMemento()
        {
            Memento memento = new Memento();
            memento.Salvar(this);
            return memento;
        }
        
    }
}
