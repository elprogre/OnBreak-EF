using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.DALC;

namespace Biblioteca.Negocio
{
    public class Contrato : IMetodos
    {
        public string Numero { get; set; }

        public DateTime Creacion { get; set; }

        public DateTime Termino { get; set; }

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

        public string IdModalidad { get; set; }

        public int IdTipoEvento { get; set; }

        public DateTime FechaHoraInicio { get; set; }

        public DateTime FechaHoraTermino { get; set; }

        public int Asistentes { get; set; }

        public int PersonalAdicional { get; set; }

        public bool Realizado { get; set; }

        public double ValorTotalContrato { get; set; }

        private string _observaciones;

        public string Observaciones
        {
            get { return _observaciones; }
            set
            {
                if (value.Trim().Length > 0)
                {
                    _observaciones = value;
                }
                else
                {
                    throw new Exception("Falta el campo Observaciones");
                }
            }

        }

        public Contrato()
        {

        }

        public void CalcularValorEvento(double valorBase,double valorAsistente,double valorPersonal)
        {
            ValorTotalContrato = valorBase + valorAsistente + valorPersonal;
        }


        OnBreakEntities bdd = new OnBreakEntities();

        public bool Create()
        {
            try
            {
                DALC.Contrato con = new DALC.Contrato();
                CommonBC.Syncronize(this, con);
                bdd.Contrato.Add(con);
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
                DALC.Contrato con = bdd.Contrato.First(c => c.Numero.Equals(Numero));
                CommonBC.Syncronize(con, this);
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
                DALC.Contrato con = bdd.Contrato.First(c => c.Numero.Equals(Numero));
                CommonBC.Syncronize(this, con);
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
                DALC.Contrato con = bdd.Contrato.First(c => c.Numero.Equals(Numero));
                con.Realizado = false;
                con.Termino = DateTime.Now;
                bdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ListaContrato> ReadAll()
        {
            try
            {
                var x = from con in bdd.Contrato
                        join ms in bdd.ModalidadServicio
                        on con.IdModalidad equals ms.IdModalidad
                        join te in bdd.TipoEvento
                        on ms.IdTipoEvento equals te.IdTipoEvento
                        join cli in bdd.Cliente
                        on con.RutCliente equals cli.RutCliente
                        select new ListaContrato()
                        {
                            Numero=con.Numero,
                            RutCliente=con.RutCliente,
                            RazonSocial=cli.RazonSocial,
                            TipoDeEvento = te.Descripcion,
                            ModalidadDeServicio = ms.Nombre,
                            FechaCreacion =con.Creacion,
                            FechaTermino=con.Termino,
                            FechaHoraInicio=con.FechaHoraInicio,
                            FechaHoraTermino=con.FechaHoraTermino,
                            Asistentes=con.Asistentes,
                            PersonalAdicional=con.PersonalAdicional,
                            Vigencia =
                            con.Realizado == true ? "Si" :
                            "No",
                            ValorTotalEvento = con.ValorTotalContrato,
                            Observaciones=con.Observaciones
                        };
                return x.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ListaContrato> ReadAllByNumeroContrato()
        {
            try
            {
                var x = from con in bdd.Contrato
                        join ms in bdd.ModalidadServicio
                        on con.IdModalidad equals ms.IdModalidad
                        join te in bdd.TipoEvento
                        on ms.IdTipoEvento equals te.IdTipoEvento
                        join cli in bdd.Cliente
                        on con.RutCliente equals cli.RutCliente
                        where con.Numero==this.Numero
                        select new ListaContrato()
                        {
                            Numero=con.Numero,
                            RutCliente=con.RutCliente,
                            RazonSocial=cli.RazonSocial,
                            TipoDeEvento = te.Descripcion,
                            ModalidadDeServicio = ms.Nombre,
                            FechaCreacion =con.Creacion,
                            FechaTermino=con.Termino,
                            FechaHoraInicio=con.FechaHoraInicio,
                            FechaHoraTermino=con.FechaHoraTermino,
                            Asistentes=con.Asistentes,
                            PersonalAdicional=con.PersonalAdicional,
                            Vigencia =
                            con.Realizado == true ? "Si" :
                            "No",
                            ValorTotalEvento = con.ValorTotalContrato,
                            Observaciones=con.Observaciones
                        };
                return x.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public List<ListaContrato> ReadAllByRut()
        {
            try
            {
                var x = from con in bdd.Contrato
                        join ms in bdd.ModalidadServicio
                        on con.IdModalidad equals ms.IdModalidad
                        join te in bdd.TipoEvento
                        on ms.IdTipoEvento equals te.IdTipoEvento
                        join cli in bdd.Cliente
                        on con.RutCliente equals cli.RutCliente
                        where con.RutCliente==this.RutCliente
                        select new ListaContrato()
                        {
                            Numero=con.Numero,
                            RutCliente=con.RutCliente,
                            RazonSocial=cli.RazonSocial,
                            TipoDeEvento = te.Descripcion,
                            ModalidadDeServicio = ms.Nombre,
                            FechaCreacion =con.Creacion,
                            FechaTermino=con.Termino,
                            FechaHoraInicio=con.FechaHoraInicio,
                            FechaHoraTermino=con.FechaHoraTermino,
                            Asistentes=con.Asistentes,
                            PersonalAdicional=con.PersonalAdicional,
                            Vigencia =
                            con.Realizado == true ? "Si" :
                            "No",
                            ValorTotalEvento = con.ValorTotalContrato,
                            Observaciones=con.Observaciones
                        };
                return x.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        /*
        public List<Contrato> ReadAllByTipo()
        {
            try
            {
                return ReadAll().Where(c => c.IdTipoEvento == IdTipoEvento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }*/

        public class ListaContrato
        {
            public ListaContrato()
            {
            }
            public string Numero { get; set; }
            public string RutCliente { get; set; }
            public string RazonSocial { get; set; }
            public string TipoDeEvento { get; set; }
            private string _modalidadservicio;

            public string ModalidadDeServicio
            {
                get { return _modalidadservicio; }
                set { _modalidadservicio = value.Trim(); }
            }

            public DateTime FechaCreacion { get; set; }
            public DateTime FechaTermino { get; set; }
            public DateTime FechaHoraInicio { get; set; }
            public DateTime FechaHoraTermino { get; set; }
            public int Asistentes { get; set; }
            public int PersonalAdicional { get; set; }
            public string Vigencia { get; set; }
            public double ValorTotalEvento { get; set; }
            public string Observaciones { get; set; }
        }
    }
}
