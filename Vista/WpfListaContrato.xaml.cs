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

            if (objeto.GetType() == typeof(MainWindow))
            {
                btnTraspasar.Visibility = Visibility.Hidden;
                cboTipoEvento.ItemsSource = new TipoEvento().ReadAll();
                cboTipoEvento.Items.Refresh();

            }
        }


    }
}
