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
    /// Lógica de interacción para WpfListaContrato.xaml
    /// </summary>
    public partial class WpfListaContrato : MetroWindow
    {
        private static sw sw = new sw();
        object objeto;
        public WpfListaContrato(object ventana_origen)
        {
            InitializeComponent();
            objeto = ventana_origen;
            cboTipoEvento.ItemsSource = new TipoEvento().ReadAll();
            dtgContrato.ItemsSource = new Contrato().ReadAll();

            if (objeto.GetType() == typeof(MainWindow))
            {
                btnTraspasar.Visibility = Visibility.Hidden;
            }
            if (sw.contraste == 1)
            {
                ThemeManager.ChangeAppTheme(this, "BaseDark");
            }
        }

        private void cboTipoEvento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cboModalidadServicio.ItemsSource = new ModalidadServicio().ReadAll().Where(x => x.IdTipoEvento == ((TipoEvento)cboTipoEvento.SelectedItem).IdTipoEvento);
            }
            catch (Exception)
            {
                cboModalidadServicio.ItemsSource = null;
            }
            
        }

        private async void btnFiltrarNroContrato_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgContrato.ItemsSource = new Contrato() {Numero=txtNroContrato.Text }.ReadAllByNumeroContrato();
                flyfiltros.IsOpen = false;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private async void btnFiltrarRut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgContrato.ItemsSource = new Contrato() { RutCliente = txtRut.Text }.ReadAllByRut();
                flyfiltros.IsOpen = false;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private async void btnFiltrarTipoEvento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgContrato.ItemsSource = new Contrato() { IdTipoEvento = ((TipoEvento)cboTipoEvento.SelectedItem).IdTipoEvento }
                    .ReadAllByTipo();
                flyfiltros.IsOpen = false;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private async void btnModalidadServicio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgContrato.ItemsSource = new Contrato() { IdModalidad = ((ModalidadServicio)cboModalidadServicio.SelectedItem).IdModalidad }
                    .ReadAllByModalidad();
                flyfiltros.IsOpen = false;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private async void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgContrato.ItemsSource = new Contrato().ReadAll();
                cboTipoEvento.SelectedIndex = -1;
                cboModalidadServicio.SelectedIndex = -1;
                txtNroContrato.Clear();
                txtRut.Clear();
                flyfiltros.IsOpen = false;
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private void btnBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            WpfListaCliente ventana = new WpfListaCliente(this);
            ventana.Show();

        }

        private async void btnTraspasar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Contrato.ListaContrato lc = (Contrato.ListaContrato)dtgContrato.SelectedItem;
                Contrato coni = new Contrato() { Numero = lc.Numero };
                coni.Read();
                if (objeto.GetType() == typeof(WpfContrato))
                {
                    ((WpfContrato)objeto).llenar(coni);
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private void btnFiltros_Click(object sender, RoutedEventArgs e)
        {
            flyfiltros.IsOpen = true;
        }
    }
}
