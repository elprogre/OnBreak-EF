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
    /// Lógica de interacción para WpfContrato.xaml
    /// </summary>
    public partial class WpfContrato : Window
    {
        public WpfContrato()
        {
            InitializeComponent();
            cboTipoEvento.ItemsSource = new TipoEvento().ReadAll();
            txtNumero.Text = DateTime.Now.ToString("yyyyMMddHHmmss");
            DateTime hoy = DateTime.Now;
            lblFechaHoy.Content = hoy.ToString("dd/MM/yyyy");
        }

    }
}
