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
    /// Lógica de interacción para WpfListaCliente.xaml
    /// </summary>
    public partial class WpfListaCliente : Window
    {
        Cliente lista = new Cliente();
        object objeto;
        public WpfListaCliente(object ventana_origen)
        {
            InitializeComponent();
            objeto = ventana_origen;
            cboTipoEmpresa.ItemsSource = new TipoEmpresa().ReadAll();
            cboTipoEmpresa.Items.Refresh();
            cboActividadEmpresa.ItemsSource = new ActividadEmpresa().ReadAll();
            cboActividadEmpresa.Items.Refresh();
            dtgCliente.ItemsSource = lista.ReadAll();
            dtgCliente.Items.Refresh();
            if (objeto.GetType()==typeof(MainWindow))
            {
                btnTraspasar.Visibility = Visibility.Hidden;
            }

        }
    }
}
