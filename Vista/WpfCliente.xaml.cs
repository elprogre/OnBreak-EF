using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Biblioteca.Negocio;

namespace Vista
{
    /// <summary>
    /// Lógica de interacción para WpfCliente.xaml
    /// </summary>
    public partial class WpfCliente : Window
    {
        public WpfCliente()
        {
            InitializeComponent();
            cboActividadEmpresa.ItemsSource = new ActividadEmpresa().ReadAll();
            cboActividadEmpresa.Items.Refresh();
            cboTipoEmpresa.ItemsSource = new TipoEmpresa().ReadAll();
            cboTipoEmpresa.Items.Refresh();
        }


        public void limpiar()
        {
            txtRut.Clear();
            txtRazonSocial.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            txtMail.Clear();
            txtDireccion.Clear();
            cboActividadEmpresa.SelectedIndex = -1;
            cboTipoEmpresa.SelectedIndex = -1;
        }


        public void llenar(Cliente cli)
        {
            txtRut.Text = cli.RutCliente;
            txtRazonSocial.Text = cli.RazonSocial;
            txtNombre.Text = cli.NombreContacto;
            txtTelefono.Text = cli.Telefono;
            txtMail.Text = cli.MailContacto;
            txtDireccion.Text = cli.Direccion;
            ActividadEmpresa act = new ActividadEmpresa() { IdActividadEmpresa = cli.IdActividadEmpresa };
                act.Read();
            cboActividadEmpresa.Text = act.Descripcion;
            TipoEmpresa tip = new TipoEmpresa() { IdTipoEmpresa = cli.IdTipoEmpresa };
                tip.Read();
            cboTipoEmpresa.Text = tip.Descripcion;
        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
            txtRut.Focus();
        }


        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente cli = new Cliente();
                cli.RutCliente = txtRut.Text;
                cli.RazonSocial = txtRazonSocial.Text;
                cli.NombreContacto = txtNombre.Text;
                cli.Telefono = txtTelefono.Text;
                cli.MailContacto = txtMail.Text;
                cli.Direccion = txtDireccion.Text;
                if (cboActividadEmpresa.SelectedIndex>=0)
                {
                    cli.IdActividadEmpresa = ((ActividadEmpresa)cboActividadEmpresa.SelectedItem).IdActividadEmpresa;
                }
                else
                {
                    throw new Exception("Falta llenar el campo Actividad Empresa");
                }
                if (cboTipoEmpresa.SelectedIndex>=0)
                {
                    cli.IdTipoEmpresa = ((TipoEmpresa)cboTipoEmpresa.SelectedItem).IdTipoEmpresa;
                }
                else
                {
                    throw new Exception("Falta llenar el campo Tipo de Empresa");
                }

                bool resp = cli.Create();
                MessageBox.Show(resp ? "Guardo" : "No Guardo");
                limpiar();
                txtRut.Focus();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente cli = new Cliente();
                cli.RutCliente = txtRut.Text;
                bool resp = cli.Read();
                if (resp)
                {
                    llenar(cli);
                }
                else
                {
                    throw new Exception("Cliente no encontrado");
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
