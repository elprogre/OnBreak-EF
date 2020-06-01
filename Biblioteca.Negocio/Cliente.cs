using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Biblioteca.Negocio
{
    public class Cliente : IMetodos
    {
        private string _rutcliente;
        public string RutCliente
        {
            get { return _rutcliente; }
            set
            {
                if (value.ToString().Length == 10)
                {
                    _rutcliente = value;
                }
                else
                {
                    throw new Exception("El rut debe tener 10 digitos");
                }
            }
        }

        private string _razonsocial;

        public string RazonSocial
        {
            get { return _razonsocial; }
            set
            {
                if (value.Trim().Length >= 2)
                {
                    _razonsocial = value;
                }
                else
                {
                    throw new Exception("La razon social es muy corta");
                }
            }
        }



        private string _nombrecontacto;

        public string NombreContacto
        {
            get { return _nombrecontacto; }
            set
            {
                if (value.Trim().Length>=3)
                {
                    _nombrecontacto = value;
                }
                else
                {
                    throw new Exception("El nombre del contacto es muy corto");
                }
            }
        }

        private string _mailcontacto;

        public string MailContacto
        {
            get { return _mailcontacto; }
            set
            {
                if (value.Trim().Length>=5)
                {
                    _mailcontacto = value;
                }
                else
                {
                    throw new Exception("El mail del contacto es muy corto");
                }
            }
        }

        private string _direccion;

        public string Direccion
        {
            get { return _direccion; }
            set
            {
                if (value.Trim().Length>0)
                {
                    if (value.Trim().Length >= 3)
                    {
                        _direccion = value;
                    }
                    else
                    {
                        throw new Exception("La direccion del contacto es muy corta");
                    }
                }
                else
                {
                    throw new Exception("Le falto llenar el campo Dirección");
                }
            }
        }

        private string _telefono;

        public string Telefono
        {
            get { return _telefono; }
            set
            {
                if (value.Trim().Length>=7)
                {
                    _telefono = value;
                }
                else
                {
                    throw new Exception("El numero de telefono del contacto es muy corto");
                }
            }
        }

        public int IdActividadEmpresa { get; set; }

        public int IdTipoEmpresa { get; set; }

        public Cliente()
        {

        }

        OnBreakEntities bdd = new OnBreakEntities();

        public bool Create()
        {
            try
            {
                DALC.Cliente cli = new DALC.Cliente();
                /*cli.RutCliente = this.RutCliente;
                cli.RazonSocial = this.RazonSocial;
                cli.NombreContacto = this.NombreContacto;
                cli.MailContacto = this.MailContacto;
                cli.Direccion = this.Direccion;
                cli.Telefono = this.Telefono;
                cli.IdActividadEmpresa = this.IdActividadEmpresa;
                cli.IdTipoEmpresa = this.IdTipoEmpresa;*/
                CommonBC.Syncronize(this,cli);
                bdd.Cliente.Add(cli);
                bdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Read()
        {
            try
            {
                DALC.Cliente cli = bdd.Cliente.First(c=>c.RutCliente.Equals(RutCliente));
                CommonBC.Syncronize(cli,this);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update()
        {
            try
            {
                DALC.Cliente cli = bdd.Cliente.First(c => c.RutCliente.Equals(this.RutCliente));
                CommonBC.Syncronize(this, cli);
                bdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete()
        {
            try
            {
                DALC.Cliente cli = 
                    bdd.Cliente.First(c => c.RutCliente.Equals(this.RutCliente));
                bdd.Cliente.Remove(cli);
                bdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Cliente> ReadAll()
        {
            try
            {
                List<Cliente> lista_clase_cliente = new List<Cliente>();
                List<DALC.Cliente> lista_cliente = bdd.Cliente.ToList();
                foreach (DALC.Cliente item in lista_cliente)
                {
                    Cliente cli = new Cliente();
                    CommonBC.Syncronize(item,cli);
                    lista_clase_cliente.Add(cli);
                }
                return lista_clase_cliente;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Cliente> ReadAllByRut()
        {
            try
            {
                return ReadAll().Where(c=> c.RutCliente.Equals(RutCliente)).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Cliente> ReadAllByTipoEmpresa()
        {
            try
            {
                return ReadAll().Where(c => c.IdTipoEmpresa == IdTipoEmpresa).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Cliente> ReadAllByActividad()
        {
            try
            {
                return ReadAll().Where(c => c.IdActividadEmpresa == IdActividadEmpresa).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
