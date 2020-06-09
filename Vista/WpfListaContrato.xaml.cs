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
    /// Lógica de interacción para WpfListaContrato.xaml
    /// </summary>
    public partial class WpfListaContrato : MetroWindow
    {
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

        private void btnFiltrarNroContrato_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgContrato.ItemsSource = new Contrato() {Numero=txtNroContrato.Text }.ReadAllByNumeroContrato();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFiltrarRut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgContrato.ItemsSource = new Contrato() { RutCliente = txtRut.Text }.ReadAllByRut();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFiltrarTipoEvento_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgContrato.ItemsSource = new Contrato() { IdTipoEvento = ((TipoEvento)cboTipoEvento.SelectedItem).IdTipoEvento }
                    .ReadAllByTipo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnModalidadServicio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgContrato.ItemsSource = new Contrato() { IdModalidad = ((ModalidadServicio)cboModalidadServicio.SelectedItem).IdModalidad }
                    .ReadAllByModalidad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dtgContrato.ItemsSource = new Contrato().ReadAll();
                cboTipoEvento.SelectedIndex = -1;
                cboModalidadServicio.SelectedIndex = -1;
                txtNroContrato.Clear();
                txtRut.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBuscarCliente_Click(object sender, RoutedEventArgs e)
        {
            WpfListaCliente ventana = new WpfListaCliente(this);
            ventana.Show();

        }

        private void btnTraspasar_Click(object sender, RoutedEventArgs e)
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
                MessageBox.Show(ex.Message);
            }
        }

    }
}
