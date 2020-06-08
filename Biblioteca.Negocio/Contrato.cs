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
                CommonBC.Syncronize(con, this);
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
                bdd.Contrato.Remove(con);
                bdd.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Contrato> ReadAll()
        {
            try
            {
                List<DALC.Contrato> lista_con = bdd.Contrato.ToList(); 
                List<Contrato> lista_clase_con = new List<Contrato>();
                foreach (DALC.Contrato item in lista_con)
                {
                    Contrato con = new Contrato();
                    CommonBC.Syncronize(item,con);
                    lista_clase_con.Add(con);
                }
                return lista_clase_con;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Contrato> ReadAllByNumeroContrato()
        {
            try
            {
                return ReadAll().Where(c => c.Numero.Equals(Numero)).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Contrato> ReadAllByRut()
        {
            try
            {
                return ReadAll().Where(c => c.RutCliente.Equals(RutCliente)).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

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
        }

    }
}
