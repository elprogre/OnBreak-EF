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
    /// Lógica de interacción para WpfListaContrato.xaml
    /// </summary>
    public partial class WpfListaContrato : Window
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
            cboModalidadServicio.ItemsSource = new ModalidadServicio().ReadAll().Where(x => x.IdTipoEvento == ((TipoEvento)cboTipoEvento.SelectedItem).IdTipoEvento);
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


    }
}
