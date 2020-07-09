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

        public WpfContrato()
        {
            InitializeComponent();
            cboTipoEvento.ItemsSource = new TipoEvento().ReadAll();
            limpiar();
        }

        public void limpiar()
        {
            gpbEvento.Visibility = Visibility.Visible;
            gpbCoffeBreak.Visibility = Visibility.Hidden;
            gpbCocktail.Visibility = Visibility.Hidden;
            gpbCenas.Visibility = Visibility.Hidden;
            cboModalidadServicio.ItemsSource = null;
            txtNumero.Text = DateTime.Now.ToString("yyyyMMddHHmm");
            ctrFechaHoraInicio.LimpiarControl();
            ctrFechaHoraFin.LimpiarControl();
            txtFechaCreacion.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            txtFechaTermino.Text = "";
            txtVigencia.Text = "";
            txtRut.Text = "";
            txtRazonSocial.Text = "Razon Social";
            cboTipoEvento.SelectedIndex = -1;
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

        }
        

        private void cboTipoEvento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboTipoEvento.SelectedIndex !=-1)
            {
                txtValorArriendoLocal.Text = "0"; //por un error, al cambiar el txtvalorArriendo con el boton limpiar se ejecutaba el metodo CrearObjetoEvento sin un tipo de evento
                gpbEvento.Visibility = Visibility.Hidden;
                gpbCoffeBreak.Visibility = Visibility.Hidden;
                gpbCocktail.Visibility = Visibility.Hidden;
                gpbCenas.Visibility = Visibility.Hidden;
                TipoEvento te = (TipoEvento)cboTipoEvento.SelectedItem;
                cboModalidadServicio.ItemsSource = new ModalidadServicio()
                    .ReadAll().Where(x => x.IdTipoEvento == (te).IdTipoEvento);


                if (te.IdTipoEvento == 10)
                {
                    gpbCoffeBreak.Visibility = Visibility.Visible;
                }
                if (te.IdTipoEvento==20)
                {
                    gpbCocktail.Visibility = Visibility.Visible;
                    cboCocktailTipoAmbientacion.ItemsSource = new TipoAmbientacion().ReadAll();
                    
                }
                else if (te.IdTipoEvento == 30)
                {
                    gpbCenas.Visibility = Visibility.Visible;
                    cboCenasTipoAmbientacion.ItemsSource = new TipoAmbientacion().ReadAll();
                }

                Evento ev;
                switch (te.Descripcion)
                {
                    case "Cocktail":
                        ev = new Cocktail();
                        break;
                    case "Coffee Break":
                        ev = new CoffeeBreak();
                        break;
                    case "Cenas":
                        ev = new Cenas();
                        break;
                    default:
                        ev = null;
                        break;
                }

                mostrarCalculosPantalla();

            }
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

        /*
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
        */



        /*
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
        */


        /*
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
        */


        /*
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
        */

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

        private void chkCocktailAmbientacion_Checked(object sender, RoutedEventArgs e)
        {
            cboCocktailTipoAmbientacion.IsEnabled = true;
        }

        private void chkCocktailAmbientacion_Unchecked(object sender, RoutedEventArgs e)
        {
            cboCocktailTipoAmbientacion.SelectedIndex = -1;
            mostrarCalculosPantalla();
            cboCocktailTipoAmbientacion.IsEnabled = false;
        }

        private void chkCocktailMusicaAmbiental_Checked(object sender, RoutedEventArgs e)
        {
            mostrarCalculosPantalla();
            chkCocktailMusicaCliente.IsEnabled = true;
        }

        private void chkCocktailMusicaAmbiental_Unchecked(object sender, RoutedEventArgs e)
        {
            mostrarCalculosPantalla();
            chkCocktailMusicaCliente.IsChecked = false;
            chkCocktailMusicaCliente.IsEnabled = false;
        }

        private void rbtOtroLocal_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                rbtOtroOnbreak.Visibility = Visibility.Visible;
                rbtOtroCliente.Visibility = Visibility.Visible;
                label5.Visibility = Visibility.Hidden;
                label6.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Visibility = Visibility.Hidden;
                txtComision.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Text = "0";
                rbtOtroOnbreak.IsChecked = true;
            }
            catch (Exception)
            {

            }
        }

        private void rbtOtroOnbreak_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                label5.Visibility = Visibility.Visible;
                label6.Visibility = Visibility.Visible;
                txtValorArriendoLocal.Visibility = Visibility.Visible;
                txtComision.Visibility = Visibility.Visible;
                txtValorArriendoLocal.Text = "0";
                mostrarCalculosPantalla();
            }
            catch (Exception)
            {

            }
        }

        private void rbtOtroCliente_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                label5.Visibility = Visibility.Hidden;
                label6.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Visibility = Visibility.Hidden;
                txtComision.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Text = "0";
            }
            catch (Exception)
            {

            }
        }

        private void rbtLocalOnBreak_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                rbtOtroOnbreak.Visibility = Visibility.Hidden;
                rbtOtroCliente.Visibility = Visibility.Hidden;
                label5.Visibility = Visibility.Hidden;
                label6.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Visibility = Visibility.Hidden;
                txtComision.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Text = "0";
                rbtOtroOnbreak.IsChecked = false;
                mostrarCalculosPantalla();
            }
            catch (Exception)
            {

            }
        }

        private void txtValorArriendoLocal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtComision.Text = (int.Parse(txtValorArriendoLocal.Text) * 0.05).ToString();
                mostrarCalculosPantalla();
            }
            catch (Exception)
            {
                txtValorArriendoLocal.Text = "0";
                mostrarCalculosPantalla();
            }
        }

        private void cboModalidadServicio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboModalidadServicio.SelectedItem!=null)
            {
                ModalidadServicio mo = new ModalidadServicio();
                mo = (ModalidadServicio)cboModalidadServicio.SelectedItem;
                txtPersonal.Text =""+mo.PersonalBase;
                mostrarCalculosPantalla();
            }
            else
            {
                txtPersonal.Text = "0";
            }
        }

        public Evento crearObjetoEvento()
        {
            Evento ev;
            Contrato cont = new Contrato();
            TipoEvento te = (TipoEvento)cboTipoEvento.SelectedItem;
            ModalidadServicio mo = new ModalidadServicio();
            if (cboModalidadServicio.SelectedIndex == -1) // En caso de modificar los datos del evento antes de escoger una modalidad
            {
                mo.IdModalidad = "0";
            }
            else
            {
                mo = (ModalidadServicio)cboModalidadServicio.SelectedItem;
            }
            cont.IdModalidad = mo.IdModalidad;
            cont.PersonalAdicional = int.Parse(txtPersonalAdicional.Text);
            cont.Asistentes = int.Parse(txtAsistentes.Text);
            switch (te.Descripcion)
            {
                case "Cocktail": //Se instancia un Cocktail con los datos disponibles
                    TipoAmbientacion tipoa = new TipoAmbientacion();
                    if (cboCocktailTipoAmbientacion.SelectedIndex==-1)
                    {
                        tipoa.idTipoAmbientacion = 30;
                    }
                    else
                    {
                        tipoa = (TipoAmbientacion)cboCocktailTipoAmbientacion.SelectedItem;
                    }
                    bool ambientacion = chkCocktailAmbientacion.IsChecked == true ? true : false;
                    bool musica_ambiental_cocktail = chkCocktailMusicaAmbiental.IsChecked == true ? true : false;
                    bool musica_cliente = chkCocktailMusicaCliente.IsChecked == true ? true : false;
                    ev = new Cocktail()
                    {
                         Numero=txtNumero.Text, IdTipoAmbientacion=tipoa.idTipoAmbientacion,Ambientacion=ambientacion,
                         MusicaAmbiental=musica_ambiental_cocktail,MusicaCliente=musica_cliente
                    };
                    ev.TipoContrato(cont);
                    break;

                case "Coffee Break"://Se instancia un CoffeeBreak con los datos disponibles
                    bool vegetariana = rbtVegetariana.IsChecked == true ? true : false;
                    ev = new CoffeeBreak()
                    {
                        Numero=txtNumero.Text, Vegetariana=vegetariana
                    };
                    ev.TipoContrato(cont);
                    break;

                case "Cenas"://Se instancia Cenas con los datos disponibles
                    TipoAmbientacion tipoa2 = new TipoAmbientacion();
                    if (cboCenasTipoAmbientacion.SelectedIndex == -1)
                    {
                        tipoa2.idTipoAmbientacion = 30;
                    }
                    else
                    {
                        tipoa2 = (TipoAmbientacion)cboCenasTipoAmbientacion.SelectedItem;
                    }
                    bool musica_ambiental_cenas = chkCenasMusicaAmbiental.IsChecked == true ? true : false;
                    bool localOnbreak = rbtLocalOnBreak.IsChecked == true ? true : false ;
                    bool otroLocalOnbreak = rbtOtroOnbreak.IsChecked == true ? true : false;
                    int valoraArriendo=int.Parse(txtValorArriendoLocal.Text);
                    ev = new Cenas()
                    {
                        Numero=txtNumero.Text, IdTipoAmbientacion=tipoa2.idTipoAmbientacion, MusicaAmbiental=musica_ambiental_cenas, LocalOnBreak=localOnbreak,
                        OtroLocalOnBreak=otroLocalOnbreak, ValorArriendo=valoraArriendo
                    };
                    ev.TipoContrato(cont);
                    break;

                default:
                    ev = null;
                    break;
            }

            return ev;
        }

        public void mostrarCalculosPantalla()
        {
            Evento ev = crearObjetoEvento();
            txtBaseEvento.Text = "" + ev.ValorBase();
            txtValorAsistente.Text = "" + ev.RecargoAsistentes();
            txtValorPersonalAdicional.Text = "" + ev.RecargoPersonalAdicional();
        }


        private void cboCocktailTipoAmbientacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mostrarCalculosPantalla();
        }

        private void rbtLocalOnBreak_Unchecked(object sender, RoutedEventArgs e)
        {
            mostrarCalculosPantalla();
        }

        private void cboCenasTipoAmbientacion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mostrarCalculosPantalla();
        }

        private void chkCenasMusicaAmbiental_Checked(object sender, RoutedEventArgs e)
        {
            mostrarCalculosPantalla();
        }

        private void chkCenasMusicaAmbiental_Unchecked(object sender, RoutedEventArgs e)
        {
            mostrarCalculosPantalla();
        }

        private void rbtOtroOnbreak_Unchecked(object sender, RoutedEventArgs e)
        {
            mostrarCalculosPantalla();
        }

        private void txtAsistentes_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                mostrarCalculosPantalla();
            }
            catch (Exception)
            {
                txtAsistentes.Text = "0";
            }
        }

        private void txtPersonalAdicional_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                mostrarCalculosPantalla();
            }
            catch (Exception)
            {
                txtPersonalAdicional.Text = "0";
            }
        }
    }
}
