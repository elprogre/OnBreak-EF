using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Vista
{
    [Serializable()]
    public class ClienteCache
    {
        private string _rutcliente;
        public string RutCliente
        {
            get { return _rutcliente; }
            set
            {
                _rutcliente = value;
            }
        }

        private string _razonsocial;

        public string RazonSocial
        {
            get { return _razonsocial; }
            set
            {
                _razonsocial = value;
            }
        }



        private string _nombrecontacto;

        public string NombreContacto
        {
            get { return _nombrecontacto; }
            set
            {
                _nombrecontacto = value;
            }
        }

        private string _mailcontacto;

        public string MailContacto
        {
            get { return _mailcontacto; }
            set
            {
                _mailcontacto = value;
            }
        }

        private string _direccion;

        public string Direccion
        {
            get { return _direccion; }
            set
            {
                _direccion = value;
            }
        }

        private string _telefono;

        public string Telefono
        {
            get { return _telefono; }
            set
            {
                _telefono = value;
            }
        }

        public int IdActividadEmpresa { get; set; }

        public int IdTipoEmpresa { get; set; }

        public ClienteCache()
        {
            RutCliente = "000";
            RazonSocial = "Nada";
            NombreContacto = "Nada";
            MailContacto = "Nada";
            Direccion = "Nada";
            Telefono = "Nada";
            IdActividadEmpresa = 1;
            IdTipoEmpresa = 1;

        }

    }
}
