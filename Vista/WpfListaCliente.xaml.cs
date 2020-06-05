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
        object objeto;
        public WpfListaCliente(object ventana_origen)
        {
            InitializeComponent();
            objeto = ventana_origen;
            cboTipoEmpresa.ItemsSource = new TipoEmpresa().ReadAll();
            cboActividadEmpresa.ItemsSource = new ActividadEmpresa().ReadAll();
            var x = from cli in new Cliente().ReadAll()
                    join ae in new ActividadEmpresa().ReadAll()
                    on cli.IdActividadEmpresa equals ae.IdActividadEmpresa
                    join te in new TipoEmpresa().ReadAll()
                    on cli.IdTipoEmpresa equals te.IdTipoEmpresa
                    select new
                    {
                        Rut = cli.RutCliente,
                        RazonSocial = cli.RazonSocial,
                        Nombre = cli.NombreContacto,
                        Mail = cli.MailContacto,
                        Dirección = cli.Direccion,
                        Telefono = cli.Telefono,
                        ActividadEmpresa = ae.Descripcion,
                        TipoEmpresa = te.Descripcion
                    };
            dtgCliente.ItemsSource = x.ToList();

            if (objeto.GetType()==typeof(MainWindow))
            {
                btnTraspasar.Visibility = Visibility.Hidden;
            }

        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            var x = from cli in new Cliente().ReadAll()
                    join ae in new ActividadEmpresa().ReadAll()
                    on cli.IdActividadEmpresa equals ae.IdActividadEmpresa
                    join te in new TipoEmpresa().ReadAll()
                    on cli.IdTipoEmpresa equals te.IdTipoEmpresa
                    select new
                    {
                        Rut = cli.RutCliente,
                        RazonSocial = cli.RazonSocial,
                        Nombre = cli.NombreContacto,
                        Mail = cli.MailContacto,
                        Dirección = cli.Direccion,
                        Telefono = cli.Telefono,
                        ActividadEmpresa = ae.Descripcion,
                        TipoEmpresa = te.Descripcion
                    };
            dtgCliente.ItemsSource = x.ToList();

            txtRut.Clear();
            cboActividadEmpresa.SelectedIndex = -1;
            cboTipoEmpresa.SelectedIndex = -1;
        }


        private void btnFiltrarRut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var x = from cli in new Cliente() {RutCliente=txtRut.Text}.ReadAllByRut()
                        join ae in new ActividadEmpresa().ReadAll()
                        on cli.IdActividadEmpresa equals ae.IdActividadEmpresa
                        join te in new TipoEmpresa().ReadAll()
                        on cli.IdTipoEmpresa equals te.IdTipoEmpresa
                        select new
                        {
                            Rut = cli.RutCliente,
                            RazonSocial = cli.RazonSocial,
                            Nombre = cli.NombreContacto,
                            Mail = cli.MailContacto,
                            Dirección = cli.Direccion,
                            Telefono = cli.Telefono,
                            ActividadEmpresa = ae.Descripcion,
                            TipoEmpresa = te.Descripcion
                        };
                dtgCliente.ItemsSource = x.ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnFiltrarActividadEmpresa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var x = from cli in new Cliente() { IdActividadEmpresa = ((ActividadEmpresa)cboActividadEmpresa.SelectedItem).IdActividadEmpresa }.ReadAllByActividad()
                        join ae in new ActividadEmpresa().ReadAll()
                        on cli.IdActividadEmpresa equals ae.IdActividadEmpresa
                        join te in new TipoEmpresa().ReadAll()
                        on cli.IdTipoEmpresa equals te.IdTipoEmpresa
                        select new
                        {
                            Rut = cli.RutCliente,
                            RazonSocial = cli.RazonSocial,
                            Nombre = cli.NombreContacto,
                            Mail = cli.MailContacto,
                            Dirección = cli.Direccion,
                            Telefono = cli.Telefono,
                            ActividadEmpresa = ae.Descripcion,
                            TipoEmpresa = te.Descripcion
                        };
                dtgCliente.ItemsSource = x.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnFiltrarTipoEmpresa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var x = from cli in new Cliente() { IdTipoEmpresa= ((TipoEmpresa)cboTipoEmpresa.SelectedItem).IdTipoEmpresa }.ReadAllByTipoEmpresa()
                        join ae in new ActividadEmpresa().ReadAll()
                        on cli.IdActividadEmpresa equals ae.IdActividadEmpresa
                        join te in new TipoEmpresa().ReadAll()
                        on cli.IdTipoEmpresa equals te.IdTipoEmpresa
                        select new
                        {
                            Rut = cli.RutCliente,
                            RazonSocial = cli.RazonSocial,
                            Nombre = cli.NombreContacto,
                            Mail = cli.MailContacto,
                            Dirección = cli.Direccion,
                            Telefono = cli.Telefono,
                            ActividadEmpresa = ae.Descripcion,
                            TipoEmpresa = te.Descripcion
                        };
                dtgCliente.ItemsSource = x.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}
