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
        Contrato contrato;
        double valorBaseEvento;
        double valorAsistente;
        double valorPersonalAdicional;


        public WpfContrato()
        {
            InitializeComponent();
            cboTipoEvento.ItemsSource = new TipoEvento().ReadAll();
            limpiar();
        }

        public void limpiar()
        {
            contrato = new Contrato() { Asistentes = 0, PersonalAdicional = 0 };

            valorBaseEvento = 0;
            valorAsistente = 0;
            valorPersonalAdicional = 0;
            txtNumero.Text = DateTime.Now.ToString("yyyyMMddHHmm");
            DateTime hoy = DateTime.Now;
            lblFechaHoy.Content = hoy.ToString("dd/MM/yyyy  HH:mm");
            txtVigencia.Text = "";
            txtRut.Text = "";
            txtRazonSocial.Text = "Razon Social";
            cboTipoEvento.SelectedIndex = -1;
            cboModalidadServicio.SelectedIndex = -1;
            txtAsistentes.Text = "0";
            txtPersonal.Text = "0";
            txtPersonalAdicional.Text = "0";
            txtObservaciones.Text = "";
            txtBaseEvento.Text = "0";
            txtValorAsistente.Text = "0";
            txtValorPersonalAdicional.Text = "0";
            txtTotal.Text = "0";

        }

        private void cboTipoEvento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboTipoEvento.SelectedIndex !=-1)
            {
                cboModalidadServicio.ItemsSource = new ModalidadServicio().ReadAll().Where(x => x.IdTipoEvento == ((TipoEvento)cboTipoEvento.SelectedItem).IdTipoEvento);
            }
        }

        private void cboModalidadServicio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboModalidadServicio.SelectedItem != null)
            {
                txtPersonal.Text = (((ModalidadServicio)cboModalidadServicio.SelectedItem).PersonalBase).ToString();
                valorBaseEvento = ((ModalidadServicio)cboModalidadServicio.SelectedItem).ValorBase;
                txtBaseEvento.Text = valorBaseEvento.ToString();
                contrato.CalcularValorEvento(valorBaseEvento, valorAsistente, valorPersonalAdicional);
                txtTotal.Text = contrato.ValorTotalContrato.ToString();
            }
            else
            {
                txtPersonal.Text = "0";
                valorBaseEvento = 0;
                txtBaseEvento.Text = valorBaseEvento.ToString();
                contrato.CalcularValorEvento(valorBaseEvento, valorAsistente, valorPersonalAdicional);
                txtTotal.Text = contrato.ValorTotalContrato.ToString();
            }
        }

        private void txtAsistentes_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (0 < int.Parse(txtAsistentes.Text))
                {
                    contrato.Asistentes = int.Parse(txtAsistentes.Text);
                    if (contrato.Asistentes >= 1 && contrato.Asistentes <= 20)
                    {
                        valorAsistente = 3;
                        txtValorAsistente.Text = valorAsistente.ToString();
                    }
                    else if (contrato.Asistentes >= 21 && contrato.Asistentes <= 50)
                    {
                        valorAsistente = 5;
                        txtValorAsistente.Text = valorAsistente.ToString();
                    }
                    else
                    {
                        if (0 < Math.Truncate((double.Parse(txtAsistentes.Text)-50)/20))
                        {
                            valorAsistente = 5 + 2*Math.Truncate((double.Parse(txtAsistentes.Text) - 50) / 20);
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
                    contrato.Asistentes = 0;
                    txtAsistentes.Text = contrato.Asistentes.ToString();
                    valorAsistente = 0;
                    txtValorAsistente.Text = valorAsistente.ToString();
                }
                contrato.CalcularValorEvento(valorBaseEvento, valorAsistente, valorPersonalAdicional);
                txtTotal.Text = contrato.ValorTotalContrato.ToString();
            }
            catch (Exception)
            {
                contrato.Asistentes = 0;
                txtAsistentes.Text = contrato.Asistentes.ToString();
                valorAsistente = 0;
                txtValorAsistente.Text = valorAsistente.ToString();
                contrato.CalcularValorEvento(valorBaseEvento, valorAsistente, valorPersonalAdicional);
                txtTotal.Text = contrato.ValorTotalContrato.ToString();
            }
        }


        private void txtPersonalAdicional_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (0 < int.Parse(txtPersonalAdicional.Text))
                {
                    contrato.PersonalAdicional = int.Parse(txtPersonalAdicional.Text);
                    if (contrato.PersonalAdicional==2)
                    {
                        valorPersonalAdicional = 2;
                        txtValorPersonalAdicional.Text = valorPersonalAdicional.ToString();
                    }
                    else if (contrato.PersonalAdicional == 3)
                    {
                        valorPersonalAdicional = 3;
                        txtValorPersonalAdicional.Text = valorPersonalAdicional.ToString();
                    }
                    else
                    {
                        valorPersonalAdicional = 3 + 0.5*(contrato.PersonalAdicional-3);
                        txtValorPersonalAdicional.Text = valorPersonalAdicional.ToString();
                    }
                }
                else
                {
                    contrato.PersonalAdicional = 0;
                    txtPersonalAdicional.Text = contrato.PersonalAdicional.ToString();
                    valorPersonalAdicional = 0;
                    txtValorPersonalAdicional.Text = valorPersonalAdicional.ToString();
                }
                contrato.CalcularValorEvento(valorBaseEvento, valorAsistente, valorPersonalAdicional);
                txtTotal.Text = contrato.ValorTotalContrato.ToString();
            }
            catch (Exception)
            {
                contrato.PersonalAdicional = 0;
                txtPersonalAdicional.Text = contrato.PersonalAdicional.ToString();
                valorPersonalAdicional = 0;
                txtValorPersonalAdicional.Text = valorPersonalAdicional.ToString();
                contrato.CalcularValorEvento(valorBaseEvento, valorAsistente, valorPersonalAdicional);
                txtTotal.Text = contrato.ValorTotalContrato.ToString();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }

        private void btnComprobar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente cli = new Cliente() { RutCliente = txtRut.Text };
                bool resp = cli.Read();
                if (resp)
                {
                    txtRazonSocial.Text = cli.RazonSocial;
                }
                else
                {
                    MessageBox.Show("Cliente no existe");
                    txtRut.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnListaCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WpfListaCliente ventana = new WpfListaCliente(this);
                ventana.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
