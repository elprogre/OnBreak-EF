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
    /// Lógica de interacción para WpfContrato.xaml
    /// </summary>
    public partial class WpfContrato : MetroWindow
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
            contrato = new Contrato() { Asistentes = 0, PersonalAdicional = 0, ValorTotalContrato=0};

            valorBaseEvento = 0;
            valorAsistente = 0;
            valorPersonalAdicional = 0;
            txtNumero.Text = DateTime.Now.ToString("yyyyMMddHHmm");
            DateTime hoy = DateTime.Now;
            lblFechaHoy.Content = hoy.ToString("dd/MM/yyyy");
            ctrFechaHoraInicio.LimpiarControl();
            ctrFechaHoraFin.LimpiarControl();
            txtFechaCreacion.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            txtFechaTermino.Text = "";
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

        public void llenar(Contrato cont)
        {
            contrato = cont;
            txtNumero.Text = cont.Numero;
            string vigencia;
            if (cont.Realizado)
            {
                vigencia = "Si";
            }
            else
            {
                vigencia = "No";
            }
            txtVigencia.Text = vigencia;
            txtRut.Text = cont.RutCliente;
            Cliente cli = new Cliente() { RutCliente = txtRut.Text };
            cli.Read();
            txtRazonSocial.Text = cli.RazonSocial;
            TipoEvento te = new TipoEvento() { IdTipoEvento = cont.IdTipoEvento };
            te.Read();
            cboTipoEvento.Text = te.Descripcion;
            ModalidadServicio ms = new ModalidadServicio() { IdModalidad = cont.IdModalidad };
            ms.Read();
            cboModalidadServicio.Text = ms.Nombre.Trim();
            ctrFechaHoraInicio.VerFechaYHora(cont.FechaHoraInicio);
            ctrFechaHoraFin.VerFechaYHora(cont.FechaHoraTermino);
            txtFechaCreacion.Text = cont.Creacion.ToString("dd/MM/yyyy HH:mm");
            txtFechaTermino.Text= cont.Termino.ToString("dd/MM/yyyy HH:mm");
            txtAsistentes.Text = cont.Asistentes.ToString();
            calcularValorAsistente();
            txtPersonalAdicional.Text = cont.PersonalAdicional.ToString();
            calcularValorPersonalAdicional();
            txtObservaciones.Text = cont.Observaciones;
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

        public void calcularValorAsistente()
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
                        if (0 < Math.Truncate((double.Parse(txtAsistentes.Text) - 50) / 20))
                        {
                            valorAsistente = 5 + 2 * Math.Truncate((double.Parse(txtAsistentes.Text) - 50) / 20);
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

        public void calcularValorPersonalAdicional()
        {
            try
            {
                if (0 < int.Parse(txtPersonalAdicional.Text))
                {
                    contrato.PersonalAdicional = int.Parse(txtPersonalAdicional.Text);
                    if (contrato.PersonalAdicional == 2)
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
                        valorPersonalAdicional = 3 + 0.5 * (contrato.PersonalAdicional - 3);
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


        private void txtAsistentes_LostFocus(object sender, RoutedEventArgs e)
        {
            calcularValorAsistente();
        }


        private void txtPersonalAdicional_LostFocus(object sender, RoutedEventArgs e)
        {
            calcularValorPersonalAdicional();
        }

        private void ctrFechaHoraFin_LostFocus(object sender, RoutedEventArgs e)
        {
            ctrFechaHoraFin.RecuperarFechaHora();
            txtFechaTermino.Text = ctrFechaHoraFin.RecuperarFechaHora().ToString("dd/MM/yyyy HH:mm");
        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
        }


        private async void btnComprobar_Click(object sender, RoutedEventArgs e)
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
                    await this.ShowMessageAsync("Comprobar:", "Cliente no existe");
                    txtRazonSocial.Text = "Razon Social";
                    txtRut.Clear();
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }


        private void btnListaCliente_Click(object sender, RoutedEventArgs e)
        {
            WpfListaCliente ventana = new WpfListaCliente(this);
            ventana.Show();
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contrato.Numero = txtNumero.Text;
                bool respCliente = new Cliente() { RutCliente = txtRut.Text }.Read();
                if (respCliente)
                {
                    contrato.RutCliente = txtRut.Text;
                }
                else
                {
                    throw new Exception("Rut del cliente no existe");
                }
                if (ctrFechaHoraInicio.RecuperarFechaHora()>ctrFechaHoraFin.RecuperarFechaHora())
                {
                    throw new Exception("La FechaHoraInicio no puede ser mayor a la FechaHoraTermino");
                }
                contrato.Creacion = DateTime.ParseExact(txtFechaCreacion.Text, "dd-MM-yyyy HH:mm", null); 
                contrato.Termino = ctrFechaHoraFin.RecuperarFechaHora();
                contrato.FechaHoraInicio = ctrFechaHoraInicio.RecuperarFechaHora();
                contrato.FechaHoraTermino = ctrFechaHoraFin.RecuperarFechaHora();
                
                if (cboTipoEvento.SelectedIndex>=0)
                {
                    contrato.IdTipoEvento =((TipoEvento)cboTipoEvento.SelectedItem).IdTipoEvento;
                }
                else
                {
                    throw new Exception("Falta el campo Tipo Evento");
                }
                if (cboModalidadServicio.SelectedIndex >= 0)
                {
                    contrato.IdModalidad = ((ModalidadServicio)cboModalidadServicio.SelectedItem).IdModalidad;
                }
                else
                {
                    throw new Exception("Falta el campo Modalidad Servicio");
                }
                //LOS ASISTENTES Y PERSONALES ADICIONALES LOS HACE LA MAQUINA
                contrato.CalcularValorEvento(valorBaseEvento, valorAsistente, valorPersonalAdicional); //calcula el total del evento y lo asigna
                contrato.Realizado = true;
                contrato.Observaciones = txtObservaciones.Text;

                bool respContrato = contrato.Create();
                await this.ShowMessageAsync("Guardar:", respContrato ? "Contrato Guardado" : "Contrato NO Guardo");
                limpiar();

            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                valorBaseEvento = 0;
                valorPersonalAdicional = 0;
                contrato = new Contrato() { Numero=txtNumero.Text };
                bool resp = contrato.Read();
                if (resp)
                {
                    llenar(contrato);

                }
                else
                {
                    await this.ShowMessageAsync("Buscar:", "Contrato NO Encontrado");
                }

            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                contrato.Numero = txtNumero.Text;
                bool respCliente = new Cliente() { RutCliente = txtRut.Text }.Read();
                if (respCliente)
                {
                    contrato.RutCliente = txtRut.Text;
                }
                else
                {
                    throw new Exception("Rut del cliente no existe");
                }
                if (ctrFechaHoraInicio.RecuperarFechaHora() > ctrFechaHoraFin.RecuperarFechaHora())
                {
                    throw new Exception("La FechaHoraInicio no puede ser mayor a la FechaHoraTermino");
                }
                contrato.Creacion = DateTime.ParseExact(txtFechaCreacion.Text, "dd-MM-yyyy HH:mm", null);
                contrato.Termino = ctrFechaHoraFin.RecuperarFechaHora();
                contrato.FechaHoraInicio = ctrFechaHoraInicio.RecuperarFechaHora();
                contrato.FechaHoraTermino = ctrFechaHoraFin.RecuperarFechaHora();

                if (cboTipoEvento.SelectedIndex >= 0)
                {
                    contrato.IdTipoEvento = ((TipoEvento)cboTipoEvento.SelectedItem).IdTipoEvento;
                }
                else
                {
                    throw new Exception("Falta el campo Tipo Evento");
                }
                if (cboModalidadServicio.SelectedIndex >= 0)
                {
                    contrato.IdModalidad = ((ModalidadServicio)cboModalidadServicio.SelectedItem).IdModalidad;
                }
                else
                {
                    throw new Exception("Falta el campo Modalidad Servicio");
                }
                //LOS ASISTENTES Y PERSONALES ADICIONALES LOS HACE LA MAQUINA
                contrato.CalcularValorEvento(valorBaseEvento, valorAsistente, valorPersonalAdicional); //calcula el total del evento y lo asigna
                contrato.Realizado = true;
                contrato.Observaciones = txtObservaciones.Text;

                bool resp = contrato.Update();
                await this.ShowMessageAsync("Actualizar:", resp ? "Contrato Actualizo" : "Contrato NO Actualizo");
                limpiar();
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageDialogResult resultado = await this.ShowMessageAsync("Terminar:", "Desea Terminar la vigencia de este contrato?", MessageDialogStyle.AffirmativeAndNegative);
                if (resultado == MessageDialogResult.Affirmative)
                {
                    contrato = new Contrato() { Numero = txtNumero.Text };
                    bool resp = contrato.Delete();
                    if (resp)
                    {
                        await this.ShowMessageAsync("Terminar:", "Vigencia del contrato TERMINADA");
                        limpiar();
                    }
                    else
                    {
                        throw new Exception("Contrato NO Existe");
                    }
                }
                else
                {
                    limpiar();
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private void btnListaContrato_Click(object sender, RoutedEventArgs e)
        {
            WpfListaContrato ventana = new WpfListaContrato(this);
            ventana.Show();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            Memento memento=new Memento();
            ContratoSalvar cont = new ContratoSalvar();
            cont.Numero = txtNumero.Text;
            cont.RutCliente = txtRut.Text;
            cont.Creacion = DateTime.Now;
            cont.Termino = ctrFechaHoraFin.RecuperarFechaHora();
            cont.FechaHoraInicio = ctrFechaHoraInicio.RecuperarFechaHora();
            cont.FechaHoraTermino = ctrFechaHoraFin.RecuperarFechaHora();

            if (cboTipoEvento.SelectedIndex >= 0)
            {
                cont.IdTipoEvento = ((TipoEvento)cboTipoEvento.SelectedItem).IdTipoEvento;
            }
            else
            {
                cont.IdTipoEvento = -1; //-1 quiere decir null al momento de recuperar
            }
            if (cboModalidadServicio.SelectedIndex >= 0)
            {
                cont.IdModalidad = ((ModalidadServicio)cboModalidadServicio.SelectedItem).IdModalidad;
            }
            else
            {
                cont.IdModalidad = "-1"; //"-1" quiere decir null al momento de recuperar
            }
            cont.Asistentes = int.Parse(txtAsistentes.Text);
            cont.PersonalAdicional = int.Parse(txtPersonalAdicional.Text);
            cont.ValorTotalContrato = 0;
            cont.Realizado = true;
            cont.Observaciones = txtObservaciones.Text;

            memento.Salvar(cont);
            await this.ShowMessageAsync("Copia de seguridad:", "Guardado Exitosamente");

        }
    }
}
