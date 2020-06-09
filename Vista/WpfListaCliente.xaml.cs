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

namespace Vista
{
    /// <summary>
    /// Lógica de interacción para WpfListaCliente.xaml
    /// </summary>
    public partial class WpfListaCliente : MetroWindow
    {
        object objeto;
        public WpfListaCliente(object ventana_origen)
        {
            InitializeComponent();
            objeto = ventana_origen;
            cboTipoEmpresa.ItemsSource = new TipoEmpresa().ReadAll();
            cboActividadEmpresa.ItemsSource = new ActividadEmpresa().ReadAll();

            dtgCliente.ItemsSource = new Cliente().ReadAll();

            if (objeto.GetType()==typeof(MainWindow))
            {
                btnTraspasar.Visibility = Visibility.Hidden;
            }

        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            dtgCliente.ItemsSource = new Cliente().ReadAll();
            txtRut.Clear();
            cboActividadEmpresa.SelectedIndex = -1;
            cboTipoEmpresa.SelectedIndex = -1;
            FlyFiltros.IsOpen = false;
        }


        private async void btnFiltrarRut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgCliente.ItemsSource = new Cliente() { RutCliente = txtRut.Text }.ReadAllByRut();
                FlyFiltros.IsOpen = false;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }


        private async void btnFiltrarActividadEmpresa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgCliente.ItemsSource = new Cliente() { IdActividadEmpresa = ((ActividadEmpresa)cboActividadEmpresa.SelectedItem).IdActividadEmpresa }
                    .ReadAllByActividad();
                FlyFiltros.IsOpen = false;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }


        private async void btnFiltrarTipoEmpresa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgCliente.ItemsSource = new Cliente() { IdTipoEmpresa = ((TipoEmpresa)cboTipoEmpresa.SelectedItem).IdTipoEmpresa }
                    .ReadAllByTipoEmpresa();
                FlyFiltros.IsOpen = false;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }


        private async void btnTraspasar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente.ListaCliente lc = (Cliente.ListaCliente)dtgCliente.SelectedItem;
                Cliente cli = new Cliente() { RutCliente=lc.Rut };
                cli.Read();
                if (objeto.GetType()==typeof(WpfCliente))
                {
                    ((WpfCliente)objeto).llenar(cli);
                }
                else if (objeto.GetType() == typeof(WpfContrato))
                {
                    ((WpfContrato)objeto).txtRut.Text = cli.RutCliente;
                    ((WpfContrato)objeto).txtRazonSocial.Text = cli.RazonSocial;
                }
                else if (objeto.GetType() == typeof(WpfListaContrato))
                {
                    ((WpfListaContrato)objeto).txtRut.Text = cli.RutCliente;
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            FlyFiltros.IsOpen = true;
        }
    }
}
