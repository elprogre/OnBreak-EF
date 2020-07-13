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
using MahApps.Metro;
using System.Windows.Threading;
using System.IO;


namespace Vista
{
    /// <summary>
    /// Lógica de interacción para WpfContrato.xaml
    /// </summary>
    public partial class WpfContrato : MetroWindow
    {
        DispatcherTimer dt = new DispatcherTimer();
        private static sw sw = new sw();

        public WpfContrato()
        {
            InitializeComponent();
            ServiceReferenceUF.Service1Client WS = new ServiceReferenceUF.Service1Client();
            txtValorUF.Text = WS.UF().ToString();
            cboTipoEvento.ItemsSource = new TipoEvento().ReadAll();
            limpiar();
            verificarArchivoBinario();
            dt.Interval = TimeSpan.FromMinutes(5); //cada 5min
            dt.Tick += dtTiempo;
            dt.Start(); //inicia el timer
            if (sw.contraste == 1)
            {
                ThemeManager.ChangeAppTheme(this, "BaseDark");
            }
        }

        private async void verificarArchivoBinario()
        {
            string rutaContrato = @"Contrato.bin";
            string rutaEvento= @"Evento.bin";
            if (File.Exists(rutaContrato))
            {
                MessageDialogResult resul = await this.ShowMessageAsync("Informacion",
                    "Hay una copia de datos en el cache ¿Desea Recuperarlos?",
                    MessageDialogStyle.AffirmativeAndNegative);
                if (resul == MessageDialogResult.Affirmative)
                {
                    Memento memento = new Memento();
                    ContratoSalvar cont = new ContratoSalvar();
                    cont.Restaurar(memento);
                    LlenarContratoCache(cont);

                    if (File.Exists(rutaEvento))
                    {
                        EventoSalvar ev;
                        if (cont.IdTipoEvento == 10)
                        {
                            ev = new CoffeeBreakCache();

                        }
                        else if (cont.IdTipoEvento == 20)
                        {
                            ev = new CocktailCache();
                        }
                        else
                        {
                            ev = new CenasCache();
                        }
                        ev.RestaurarEventoCache(memento);
                        LlenarEventoCache(ev,cont.IdTipoEvento);
                        File.Delete(rutaEvento);
                    }

                    File.Delete(rutaContrato);


                }
                else
                {
                    File.Delete(rutaContrato);
                    File.Delete(rutaEvento);
                }
            }

        }

        private void LlenarEventoCache(EventoSalvar ev,int idTipoEventos)
        {
            EventoSalvar eventoCache = ev;
            if (idTipoEventos == 10)
            {

                if (((CoffeeBreakCache)eventoCache).Vegetariana)
                {
                    rbtVegetariana.IsChecked = true;
                }
                else
                {
                    rbtMixta.IsChecked = true;
                }
            }
            else if (idTipoEventos == 20)
            {
                chkCocktailAmbientacion.IsChecked = ((CocktailCache)eventoCache).Ambientacion;
                chkCocktailMusicaAmbiental.IsChecked = ((CocktailCache)eventoCache).MusicaAmbiental;
                chkCocktailMusicaCliente.IsChecked = ((CocktailCache)eventoCache).MusicaCliente;
                if (((CocktailCache)eventoCache).Ambientacion == false)
                {
                    cboCocktailTipoAmbientacion.SelectedIndex = -1;
                }
                else
                {
                    TipoAmbientacion ta = new TipoAmbientacion() { idTipoAmbientacion = ((CocktailCache)eventoCache).IdTipoAmbientacion };
                    ta.Read();
                    cboCocktailTipoAmbientacion.Text = ta.Descripcion;
                }

            }
            else if (idTipoEventos == 30)
            {
                TipoAmbientacion ta = new TipoAmbientacion() { idTipoAmbientacion = ((CenasCache)eventoCache).IdTipoAmbientacion };
                ta.Read();
                cboCenasTipoAmbientacion.Text = ta.Descripcion;
                chkCenasMusicaAmbiental.IsChecked = ((CenasCache)eventoCache).MusicaAmbiental;
                if (((CenasCache)eventoCache).LocalOnBreak)
                {
                    rbtLocalOnBreak.IsChecked = true;
                }
                else
                {
                    rbtOtroLocal.IsChecked = true;
                    if (((CenasCache)eventoCache).OtroLocalOnBreak)
                    {
                        rbtOtroOnbreak.IsChecked = true;
                        txtValorArriendoLocal.Text = (((CenasCache)eventoCache).ValorArriendo).ToString();
                    }
                    else
                    {
                        rbtOtroCliente.IsChecked = true;
                    }
                }
            }
        }

        private void LlenarContratoCache(ContratoSalvar cont)
        {
            txtNumero.Text = cont.Numero;
            txtRut.Text = cont.RutCliente;
            if (cont.IdTipoEvento==-1)
            {
                cboTipoEvento.SelectedIndex = -1;
                cboModalidadServicio.ItemsSource = null;
            }
            else
            {
                TipoEvento te = new TipoEvento() { IdTipoEvento = cont.IdTipoEvento };
                te.Read();
                cboTipoEvento.Text = te.Descripcion;
                if (cont.IdModalidad.Equals("-1"))
                {
                    cboModalidadServicio.SelectedIndex = -1;
                }
                else
                {
                    ModalidadServicio mo = new ModalidadServicio() { IdModalidad = cont.IdModalidad };
                    mo.Read();
                    cboModalidadServicio.Text = mo.Nombre.Trim();
                }
            }
            txtAsistentes.Text = cont.Asistentes.ToString();
            txtPersonalAdicional.Text = cont.PersonalAdicional.ToString();
            txtObservaciones.Text = cont.Observaciones.ToString();
            ctrFechaHoraInicio.VerFechaYHora(cont.FechaHoraInicio);
            ctrFechaHoraFin.VerFechaYHora(cont.FechaHoraTermino);
            txtFechaCreacion.Text = cont.Creacion.ToString("dd/MM/yyyy HH:mm");
            txtFechaTermino.Text = cont.Termino.ToString("dd/MM/yyyy HH:mm");

        }

        private void dtTiempo(object sender, EventArgs e)
        {
            grabarArchivoBin();
        }

        private void grabarArchivoBin()
        {
            Memento memento = new Memento();
            ContratoSalvar cont = new ContratoSalvar();
            cont.Numero = txtNumero.Text;
            cont.RutCliente = txtRut.Text;
            cont.Creacion = DateTime.ParseExact(txtFechaCreacion.Text, "dd-MM-yyyy HH:mm", null);
            cont.Termino = DateTime.ParseExact(txtFechaTermino.Text, "dd-MM-yyyy HH:mm", null); ;
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
            if (txtVigencia.Text=="Si")
            {
                cont.Realizado = true;
            }
            else if (txtVigencia.Text == "No")
            {
                cont.Realizado = false;
            }

            cont.Observaciones = txtObservaciones.Text;
            memento.SalvarContratoCache(cont);
            if (cboTipoEvento.SelectedIndex>=0)
            {
                EventoSalvar ev;
                if (cont.IdTipoEvento==10)
                {
                    bool respVegetariana = rbtVegetariana.IsChecked == true ? true : false;
                    ev = new CoffeeBreakCache()
                    {
                        Numero = txtNumero.Text,
                        Vegetariana = respVegetariana
                    };
                }
                else if (cont.IdTipoEvento==20)
                {
                    bool respAmbientacionCock = chkCocktailAmbientacion.IsChecked == true ? true : false;
                    TipoAmbientacion ta = new TipoAmbientacion();
                    if (respAmbientacionCock)
                    {
                        ta = (TipoAmbientacion)cboCocktailTipoAmbientacion.SelectedItem;
                    }
                    else
                    {
                        ta.idTipoAmbientacion = 30;
                    }
                    bool respMusicaCock = chkCocktailMusicaAmbiental.IsChecked == true ? true : false;
                    bool respMusicaClienteCock = chkCocktailMusicaCliente.IsChecked == true ? true : false;
                    ev = new CocktailCache()
                    {
                        Numero = txtNumero.Text,
                        Ambientacion = respAmbientacionCock,
                        IdTipoAmbientacion = ta.idTipoAmbientacion,
                        MusicaAmbiental = respMusicaCock,
                        MusicaCliente = respMusicaClienteCock
                    };
                }
                else
                {
                    TipoAmbientacion ta2 = new TipoAmbientacion();
                    if (cboCenasTipoAmbientacion.SelectedIndex == -1)
                    {
                        ta2.idTipoAmbientacion = 30;
                    }
                    else
                    {
                        ta2 = (TipoAmbientacion)cboCenasTipoAmbientacion.SelectedItem;
                    }
                    bool musica_ambiental_cenas = chkCenasMusicaAmbiental.IsChecked == true ? true : false;
                    bool localOnbreak = rbtLocalOnBreak.IsChecked == true ? true : false;
                    bool otroLocalOnbreak = rbtOtroOnbreak.IsChecked == true ? true : false;
                    int valoraArriendo = int.Parse(txtValorArriendoLocal.Text);
                    ev = new CenasCache()
                    {
                        Numero = txtNumero.Text,
                        IdTipoAmbientacion = ta2.idTipoAmbientacion,
                        MusicaAmbiental = musica_ambiental_cenas,
                        LocalOnBreak = localOnbreak,
                        OtroLocalOnBreak = otroLocalOnbreak,
                        ValorArriendo = valoraArriendo
                    };
                }
                memento.SalvarEventoCache(ev);
            }
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
            txtFechaTermino.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
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
            txtValorExtra.Text = "0";
            txtTotal.Text = "0";
        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
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
                    cboCocktailTipoAmbientacion.ItemsSource = new TipoAmbientacion().ReadAll().Where(x => x.idTipoAmbientacion<=20);
                    
                }
                else if (te.IdTipoEvento == 30)
                {
                    gpbCenas.Visibility = Visibility.Visible;
                    cboCenasTipoAmbientacion.ItemsSource = new TipoAmbientacion().ReadAll().Where(x => x.idTipoAmbientacion <= 20);
                }
                mostrarCalculosPantalla();
            }
        }


        private void ctrFechaHoraFin_LostFocus(object sender, RoutedEventArgs e)
        {
            ctrFechaHoraFin.RecuperarFechaHora();
            txtFechaTermino.Text = ctrFechaHoraFin.RecuperarFechaHora().ToString("dd/MM/yyyy HH:mm");
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
                Logger.mensaje(ex.Message);
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
                Contrato contrato = new Contrato();
                contrato.Numero = txtNumero.Text;
                bool respCliente = new Cliente() { RutCliente = txtRut.Text }.Read();
                contrato.RutCliente = txtRut.Text;
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
                
                if (cboTipoEvento.SelectedIndex>=0) //necesario
                {
                    contrato.IdTipoEvento =((TipoEvento)cboTipoEvento.SelectedItem).IdTipoEvento;
                }
                else
                {
                    throw new Exception("Falta el campo Tipo Evento");
                }
                if (cboModalidadServicio.SelectedIndex >= 0) //necesario
                {
                    contrato.IdModalidad = ((ModalidadServicio)cboModalidadServicio.SelectedItem).IdModalidad;
                }
                else
                {
                    throw new Exception("Falta el campo Modalidad Servicio");
                }
                contrato.Asistentes = int.Parse(txtAsistentes.Text);
                contrato.PersonalAdicional = int.Parse(txtPersonalAdicional.Text);
                contrato.Realizado = true;
                contrato.Observaciones = txtObservaciones.Text;
                Evento evento = crearObjetoEvento();
                contrato.ValorTotalContrato = evento.ValorBase() + evento.RecargoAsistentes() + evento.RecargoPersonalAdicional() + evento.RecargoExtras();

                bool respContrato = contrato.Create();

                if (contrato.IdTipoEvento==10)
                {
                    CoffeeBreak coff = ((CoffeeBreak)evento);
                    coff.Create();
                }
                else if (contrato.IdTipoEvento == 20)
                {
                    Cocktail cock = ((Cocktail)evento);
                    cock.Create();
                }
                else if (contrato.IdTipoEvento == 30)
                {
                    Cenas cena = ((Cenas)evento);
                    if (((Cenas)evento).IdTipoAmbientacion==30)
                    {
                        throw new Exception("El campo 'Tipo De Ambientacion' es obligatorio");
                    }
                    cena.Create();
                }

                await this.ShowMessageAsync("Contrato:", respContrato ? "Contrato Guardado" : "Contrato NO Guardo");
                if (respContrato)
                {
                    limpiar();
                }

            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
                Logger.mensaje(ex.Message);
            }
        }


        public void llenar(Contrato cont)
        {
            txtNumero.Text = cont.Numero;
            txtRut.Text = cont.RutCliente;
            Cliente cli = new Cliente() { RutCliente = txtRut.Text };
            cli.Read();
            txtRazonSocial.Text = cli.RazonSocial;
            if (cont.Realizado)
            {
                txtVigencia.Text = "Si";
            }
            else
            {
                txtVigencia.Text = "No";
            }
            TipoEvento te = new TipoEvento() { IdTipoEvento = cont.IdTipoEvento };
            te.Read();
            cboTipoEvento.Text = te.Descripcion;
            ModalidadServicio ms = new ModalidadServicio() { IdModalidad = cont.IdModalidad };
            ms.Read();
            cboModalidadServicio.Text = ms.Nombre.Trim();
            txtPersonal.Text = ms.PersonalBase.ToString();
            txtAsistentes.Text = cont.Asistentes.ToString();
            txtPersonalAdicional.Text = cont.PersonalAdicional.ToString();
            txtObservaciones.Text = cont.Observaciones;
            txtFechaCreacion.Text = cont.Creacion.ToString("dd/MM/yyyy HH:mm");
            txtFechaTermino.Text = cont.Termino.ToString("dd/MM/yyyy HH:mm");
            ctrFechaHoraInicio.VerFechaYHora(cont.FechaHoraInicio);
            ctrFechaHoraFin.VerFechaYHora(cont.FechaHoraTermino);
            Evento evento;
            if (te.IdTipoEvento == 10)
            {
                evento = new CoffeeBreak() { Numero = cont.Numero };
                ((CoffeeBreak)evento).Read();
                if (((CoffeeBreak)evento).Vegetariana)
                {
                    rbtVegetariana.IsChecked = true;
                }
                else
                {
                    rbtMixta.IsChecked = true;
                }
            }
            else if (te.IdTipoEvento == 20)
            {
                evento = new Cocktail() { Numero = cont.Numero };
                ((Cocktail)evento).Read();
                chkCocktailAmbientacion.IsChecked = ((Cocktail)evento).Ambientacion;
                chkCocktailMusicaAmbiental.IsChecked = ((Cocktail)evento).MusicaAmbiental;
                chkCocktailMusicaCliente.IsChecked = ((Cocktail)evento).MusicaCliente;
                if (((Cocktail)evento).Ambientacion == false)
                {
                    cboCocktailTipoAmbientacion.SelectedIndex = -1;
                }
                else
                {
                    TipoAmbientacion ta = new TipoAmbientacion() { idTipoAmbientacion = ((Cocktail)evento).IdTipoAmbientacion };
                    ta.Read();
                    cboCocktailTipoAmbientacion.Text = ta.Descripcion;
                }

            }
            else if (te.IdTipoEvento == 30)
            {
                evento = new Cenas() { Numero = cont.Numero };
                ((Cenas)evento).Read();
                TipoAmbientacion ta = new TipoAmbientacion() { idTipoAmbientacion = ((Cenas)evento).IdTipoAmbientacion };
                ta.Read();
                cboCenasTipoAmbientacion.Text = ta.Descripcion;
                chkCenasMusicaAmbiental.IsChecked = ((Cenas)evento).MusicaAmbiental;
                if (((Cenas)evento).LocalOnBreak)
                {
                    rbtLocalOnBreak.IsChecked = true;
                }
                else
                {
                    rbtOtroLocal.IsChecked = true;
                    if (((Cenas)evento).OtroLocalOnBreak)
                    {
                        rbtOtroOnbreak.IsChecked = true;
                        txtValorArriendoLocal.Text = (((Cenas)evento).ValorArriendo).ToString();
                    }
                    else
                    {
                        rbtOtroCliente.IsChecked = true;
                    }
                }
            }

        }


        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Contrato contrato = new Contrato() { Numero=txtNumero.Text };
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
                Logger.mensaje(ex.Message);
            }
        }



        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Contrato contrato = new Contrato();
                contrato.Numero = txtNumero.Text;
                bool respCliente = new Cliente() { RutCliente = txtRut.Text }.Read();
                contrato.RutCliente = txtRut.Text;
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
                contrato.Termino = DateTime.ParseExact(txtFechaTermino.Text, "dd-MM-yyyy HH:mm", null);
                contrato.FechaHoraInicio = ctrFechaHoraInicio.RecuperarFechaHora();
                contrato.FechaHoraTermino = ctrFechaHoraFin.RecuperarFechaHora();
                
                if (cboTipoEvento.SelectedIndex>=0) //necesario
                {
                    contrato.IdTipoEvento =((TipoEvento)cboTipoEvento.SelectedItem).IdTipoEvento;
                }
                else
                {
                    throw new Exception("Falta el campo Tipo Evento");
                }
                if (cboModalidadServicio.SelectedIndex >= 0) //necesario
                {
                    contrato.IdModalidad = ((ModalidadServicio)cboModalidadServicio.SelectedItem).IdModalidad;
                }
                else
                {
                    throw new Exception("Falta el campo Modalidad Servicio");
                }
                contrato.Asistentes = int.Parse(txtAsistentes.Text);
                contrato.PersonalAdicional = int.Parse(txtPersonalAdicional.Text);
                if (txtVigencia.Text.Equals("Si"))
                {
                    contrato.Realizado = true;
                }
                else
                {
                    contrato.Realizado = false;
                }

                contrato.Observaciones = txtObservaciones.Text;
                Evento evento = crearObjetoEvento();
                contrato.ValorTotalContrato = evento.ValorBase() + evento.RecargoAsistentes() + evento.RecargoPersonalAdicional() + evento.RecargoExtras();

                bool resp = contrato.Update();
                if (contrato.IdTipoEvento==10)
                {
                    CoffeeBreak coff = ((CoffeeBreak)evento);
                    coff.Update();
                }
                else if (contrato.IdTipoEvento == 20)
                {
                    Cocktail cock = ((Cocktail)evento);
                    cock.Update();
                }
                else if (contrato.IdTipoEvento == 30)
                {
                    Cenas cena = ((Cenas)evento);
                    cena.Update();
                }

                await this.ShowMessageAsync("Actualizar:", resp ? "Contrato Actualizo" : "Contrato NO Actualizo");
                if (txtVigencia.Text.Equals("No"))
                {
                    await this.ShowMessageAsync("VIGENCIA TERMINADA:", "No se puede ACTUALIZAR un contrato con su vigencia terminada");
                }
                if (resp)
                {
                    limpiar();
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
                Logger.mensaje(ex.Message);
            }
        }



        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageDialogResult resultado = await this.ShowMessageAsync("Terminar:", "Desea Terminar la vigencia de este contrato?", MessageDialogStyle.AffirmativeAndNegative);
                if (resultado == MessageDialogResult.Affirmative)
                {
                    Contrato contrato= new Contrato() { Numero = txtNumero.Text };
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
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("ERROR:", ex.Message);
                Logger.mensaje(ex.Message);
            }
        }

        private void btnListaContrato_Click(object sender, RoutedEventArgs e)
        {
            WpfListaContrato ventana = new WpfListaContrato(this);
            ventana.Show();
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
                labelPequeño.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Visibility = Visibility.Hidden;
                txtComision.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Text = "0";
                rbtOtroOnbreak.IsChecked = true;
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
            }
        }

        private void rbtOtroOnbreak_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                label5.Visibility = Visibility.Visible;
                label6.Visibility = Visibility.Visible;
                labelPequeño.Visibility = Visibility.Visible;
                txtValorArriendoLocal.Visibility = Visibility.Visible;
                txtComision.Visibility = Visibility.Visible;
                txtValorArriendoLocal.Text = "0";
                mostrarCalculosPantalla();
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
            }
        }

        private void rbtOtroCliente_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                label5.Visibility = Visibility.Hidden;
                label6.Visibility = Visibility.Hidden;
                labelPequeño.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Visibility = Visibility.Hidden;
                txtComision.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Text = "0";
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
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
                labelPequeño.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Visibility = Visibility.Hidden;
                txtComision.Visibility = Visibility.Hidden;
                txtValorArriendoLocal.Text = "0";
                rbtOtroOnbreak.IsChecked = false;
                mostrarCalculosPantalla();
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
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
            if (te!=null)
            {
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
                        if (cboCocktailTipoAmbientacion.SelectedIndex == -1)
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
                            Numero = txtNumero.Text,
                            IdTipoAmbientacion = tipoa.idTipoAmbientacion,
                            Ambientacion = ambientacion,
                            MusicaAmbiental = musica_ambiental_cocktail,
                            MusicaCliente = musica_cliente
                        };
                        ev.TipoContrato(cont);
                        break;

                    case "Coffee Break"://Se instancia un CoffeeBreak con los datos disponibles
                        bool vegetariana = rbtVegetariana.IsChecked == true ? true : false;
                        ev = new CoffeeBreak()
                        {
                            Numero = txtNumero.Text,
                            Vegetariana = vegetariana
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
                        bool localOnbreak = rbtLocalOnBreak.IsChecked == true ? true : false;
                        bool otroLocalOnbreak = rbtOtroOnbreak.IsChecked == true ? true : false;
                        int valoraArriendo = int.Parse(txtValorArriendoLocal.Text);
                        ev = new Cenas()
                        {
                            Numero = txtNumero.Text,
                            IdTipoAmbientacion = tipoa2.idTipoAmbientacion,
                            MusicaAmbiental = musica_ambiental_cenas,
                            LocalOnBreak = localOnbreak,
                            OtroLocalOnBreak = otroLocalOnbreak,
                            ValorArriendo = valoraArriendo
                        };
                        ev.TipoContrato(cont);
                        break;

                    default:
                        ev = null;
                        break;
                }

                return ev;
            }
            return null;
            
        }

        public void mostrarCalculosPantalla()
        {
            Evento ev = crearObjetoEvento();
            if (ev!=null)
            {
                txtBaseEvento.Text = "" + ev.ValorBase();
                txtValorAsistente.Text = "" + ev.RecargoAsistentes();
                txtValorPersonalAdicional.Text = "" + ev.RecargoPersonalAdicional();
                txtValorExtra.Text = "" + ev.RecargoExtras();
                txtTotal.Text = (ev.ValorBase() + ev.RecargoAsistentes() + ev.RecargoPersonalAdicional() + ev.RecargoExtras()).ToString();
            }
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

        private async void btnRespaldo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                grabarArchivoBin();
                await this.ShowMessageAsync("Copia de seguridad:", "Guardado Exitosamente");
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                await this.ShowMessageAsync("Respaldo:", "Error al guardar la copia de seguridad");
            }
        }

        private void txtBaseEvento_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtBaseEventoCLP.Text = (double.Parse(txtBaseEvento.Text) * double.Parse(txtValorUF.Text)).ToString("0,0.0");
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
            }
        }

        private void txtValorAsistente_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtValorAsistenteCLP.Text = (double.Parse(txtValorAsistente.Text) * double.Parse(txtValorUF.Text)).ToString("0,0.0");
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
            }
        }

        private void txtValorPersonalAdicional_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtValorPersonalAdicionalCLP.Text = (double.Parse(txtValorPersonalAdicional.Text) * double.Parse(txtValorUF.Text)).ToString("0,0.0");
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
            }
        }

        private void txtValorExtra_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtValorExtraCLP.Text = (double.Parse(txtValorExtra.Text) * double.Parse(txtValorUF.Text)).ToString("0,0.0");
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
            }
        }

        private void txtTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtTotalCLP.Text = (double.Parse(txtTotal.Text) * double.Parse(txtValorUF.Text)).ToString("0,0.0");
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
            }
        }

    }
}
