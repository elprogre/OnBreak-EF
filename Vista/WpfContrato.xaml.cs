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
        Contrato contrato = new Contrato();
        double asistentes = 0;
        double valorBaseEvento = 0;
        double valorAsistente = 0;

        public WpfContrato()
        {
            InitializeComponent();
            cboTipoEvento.ItemsSource = new TipoEvento().ReadAll();
            txtNumero.Text = DateTime.Now.ToString("yyyyMMddHHmm");
            DateTime hoy = DateTime.Now;
            lblFechaHoy.Content = hoy.ToString("dd/MM/yyyy  HH:mm");
            txtRut.Focus();
            
        }

        private void cboTipoEvento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboModalidadServicio.ItemsSource = new ModalidadServicio().ReadAll().Where(x => x.IdTipoEvento==((TipoEvento)cboTipoEvento.SelectedItem).IdTipoEvento);
        }

        private void cboModalidadServicio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboModalidadServicio.SelectedItem != null)
            {
                txtPersonal.Text = (((ModalidadServicio)cboModalidadServicio.SelectedItem).PersonalBase).ToString();
                valorBaseEvento = ((ModalidadServicio)cboModalidadServicio.SelectedItem).ValorBase;
                txtBaseEvento.Text = valorBaseEvento.ToString();
            }
            else
            {
                txtPersonal.Text = "0";
                valorBaseEvento = 0;
                txtBaseEvento.Text = valorBaseEvento.ToString();
            }
        }

        private void txtAsistentes_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (0 < double.Parse(txtAsistentes.Text))
                {
                    asistentes = double.Parse(txtAsistentes.Text);
                    if (asistentes>=1 && asistentes<=20)
                    {
                        valorAsistente = 3;
                        txtValorAsistente.Text = valorAsistente.ToString();
                    }
                    else if (asistentes >= 21 && asistentes <= 50)
                    {
                        valorAsistente = 5;
                        txtValorAsistente.Text = valorAsistente.ToString();
                    }
                    else
                    {
                        if (0 < Math.Truncate((double.Parse(txtAsistentes.Text)-50)/20))
                        {
                            valorAsistente = 5 + Math.Truncate((double.Parse(txtAsistentes.Text) - 50) / 20);
                            txtValorAsistente.Text = valorAsistente.ToString();
                        }
                        else
                        {
                            valorAsistente = 5;
                            txtValorAsistente.Text = valorAsistente.ToString();
                        }
                    }
                }
                else
                {
                    asistentes = 0;
                    txtAsistentes.Text = asistentes.ToString();
                    valorAsistente = 0;
                    txtValorAsistente.Text = valorAsistente.ToString();
                }
            }
            catch (Exception)
            {
                asistentes = 0;
                txtAsistentes.Text = asistentes.ToString();
                valorAsistente = 0;
                txtValorAsistente.Text = valorAsistente.ToString();
            }
        }


    }
}
