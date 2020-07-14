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

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;
using MahApps.Metro;


namespace Vista
{
    /// <summary>
    /// Lógica de interacción para WpfCliente.xaml
    /// </summary>
    public partial class WpfCliente : MetroWindow
    {
        private static sw sw = new sw();
        public WpfCliente()
        {
            InitializeComponent();
            cboActividadEmpresa.ItemsSource = new ActividadEmpresa().ReadAll();
            cboTipoEmpresa.ItemsSource = new TipoEmpresa().ReadAll();
            if (sw.contraste==1)
            {
                ThemeManager.ChangeAppTheme(this,"BaseDark");
            }

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


        private async void btnCreate_Click(object sender, RoutedEventArgs e)
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
                await this.ShowMessageAsync("Guardar:",resp ? "Cliente Guardado" : "Cliente NO Guardo");
                limpiar();
                txtRut.Focus();

            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                await this.ShowMessageAsync("ERROR:",ex.Message);
            }
        }


        private async void btnRead_Click(object sender, RoutedEventArgs e)
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
                Logger.mensaje(ex.Message);
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }


        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
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
                if (cboActividadEmpresa.SelectedIndex >= 0)
                {
                    cli.IdActividadEmpresa = ((ActividadEmpresa)cboActividadEmpresa.SelectedItem).IdActividadEmpresa;
                }
                else
                {
                    throw new Exception("Falta llenar el campo Actividad Empresa");
                }
                if (cboTipoEmpresa.SelectedIndex >= 0)
                {
                    cli.IdTipoEmpresa = ((TipoEmpresa)cboTipoEmpresa.SelectedItem).IdTipoEmpresa;
                }
                else
                {
                    throw new Exception("Falta llenar el campo Tipo de Empresa");
                }

                bool resp = cli.Update();
                if (resp)
                {
                    await this.ShowMessageAsync("Actualizar:", "Cliente actualizado");
                    limpiar();
                    txtRut.Focus();
                }
                else
                {
                    throw new Exception("Cliente no existe");
                }

            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }


        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageDialogResult resultado = await this.ShowMessageAsync("Eliminar:", "Desea eliminar al cliente?",MessageDialogStyle.AffirmativeAndNegative);
                if (resultado == MessageDialogResult.Affirmative)
                {
                    bool respuestaContrato = new Contrato() { RutCliente = txtRut.Text }.ReadByRut();
                    if (respuestaContrato)
                    {
                        throw new Exception("No se puede eliminar un CLIENTE vinculado a un CONTRATO");
                    }
                    else
                    {
                        Cliente cli = new Cliente() { RutCliente = txtRut.Text };
                        bool resp = cli.Delete();
                        if (resp)
                        {
                            await this.ShowMessageAsync("Eliminar:", "Cliente Eliminado");
                            limpiar();
                            txtRut.Focus();
                        }
                        else
                        {
                            throw new Exception("Cliente no existe");
                        }
                    }

                }
                else
                {
                    limpiar();
                    txtRut.Focus();
                }
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private void btnListaCliente_Click(object sender, RoutedEventArgs e)
        {
            WpfListaCliente ventana = new WpfListaCliente(this);
            ventana.Show();
        }

    }
}
