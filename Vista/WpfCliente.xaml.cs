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

//Cacheo simple
using System.Runtime.Caching; //Libreria para el uso de Cache
using System.Windows.Threading; //libreria de uso de Hilos
using System.Runtime.Serialization.Formatters.Binary; //Serializar en un formato binario el cliente y dejarlo en un archivo
using System.IO;//Manejar Archivos

namespace Vista
{
    /// <summary>
    /// Lógica de interacción para WpfCliente.xaml
    /// </summary>
    public partial class WpfCliente : MetroWindow
    {
        //Creación de objeto TIMER
        DispatcherTimer dt = new DispatcherTimer();
        int x = 0; //contar veces que guardo cache
        //Creación de objeto donde almacena el Cache
        private ObjectCache cache = MemoryCache.Default;


        public WpfCliente()
        {
            InitializeComponent();
            cboActividadEmpresa.ItemsSource = new ActividadEmpresa().ReadAll();
            cboTipoEmpresa.ItemsSource = new TipoEmpresa().ReadAll();
            //Antes verificamos si existio un cache
            //verificarCache(); //Cache simple

            //Verificar si existe archivo binario
            verificarArchivoBinario(); 

            //Definir el tiempo para el objeto TIMER
            dt.Interval = TimeSpan.FromMinutes(1); //Se ejecuta cada dias segundos
            //Definición del metodo que se genera cada 10 segundos
            dt.Tick += dtTiempo;
            dt.Start(); //inicia el timer
        }


        private async void verificarArchivoBinario() //Archivo binario
        {
            string ruta = @"c:/Copias/ArchivoBin.bin";
            if (File.Exists(ruta))
            {
                MessageDialogResult resul = await this.ShowMessageAsync("Informacion",
                    "Hay una copia de datos en el cache ¿Desea Recuperarlos?",
                    MessageDialogStyle.AffirmativeAndNegative);
                if (resul == MessageDialogResult.Affirmative)
                {
                    Stream stream = File.OpenRead(ruta);
                    BinaryFormatter formato = new BinaryFormatter();
                    ClienteCache cli = (ClienteCache)formato.Deserialize(stream);
                    llenarDatosCache(cli);
                    stream.Close();
                }
                else
                {
                    File.Delete(ruta);
                }
            }
            
        }


        private async void verificarCache() //Cache simple
        {
            ClienteCache cli = (ClienteCache)cache["cliente"];
            if (cli==null)
            {
                this.Title = "No hay nada en el cache";
            }
            else
            {
                MessageDialogResult resul = await this.ShowMessageAsync("Informacion",
                    "Hay una copia de datos en el cache ¿Desea Recuperarlos?",
                    MessageDialogStyle.AffirmativeAndNegative);
                if (resul==MessageDialogResult.Affirmative)
                {
                    llenarDatosCache(cli);
                }
            }
        }


        private void llenarDatosCache(ClienteCache cli)
        {
            txtRut.Text = cli.RutCliente;
            txtRazonSocial.Text = cli.RazonSocial;
            txtNombre.Text = cli.NombreContacto;
            txtTelefono.Text = cli.Telefono;
            txtMail.Text = cli.MailContacto;
            txtDireccion.Text = cli.Direccion;
            ActividadEmpresa act = new ActividadEmpresa() { IdActividadEmpresa = cli.IdActividadEmpresa };
            act.Read();
            cboActividadEmpresa.Text = act.Descripcion;
            TipoEmpresa tip = new TipoEmpresa() { IdTipoEmpresa = cli.IdTipoEmpresa };
            tip.Read();
            cboTipoEmpresa.Text = tip.Descripcion;
        }


        private void dtTiempo(object sender, EventArgs e)
        {
            x++;
            //grabarCache();
            grabarClienteBIN();
        }

        private void grabarCache()
        {
            this.Title = "Almaceno Cache :" + x;
            /////////////////////////////////
            //Proceso almacenar datos
            ClienteCache cli = new ClienteCache();
            cli.RutCliente = txtRut.Text;
            cli.RazonSocial = txtRazonSocial.Text;
            cli.NombreContacto = txtNombre.Text;
            cli.Telefono = txtTelefono.Text;
            cli.MailContacto = txtMail.Text;
            cli.Direccion = txtDireccion.Text;
            cli.IdActividadEmpresa = ((ActividadEmpresa)cboActividadEmpresa.SelectedItem).IdActividadEmpresa;
            cli.IdTipoEmpresa = ((TipoEmpresa)cboTipoEmpresa.SelectedItem).IdTipoEmpresa;
            //Creación de una politica de acceso al cache
            CacheItemPolicy politica = new CacheItemPolicy();
            politica.Priority = CacheItemPriority.Default;
            politica.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(100);
            //Almacenar el cache
            cache.Set("cliente", cli, politica);
        }

        private void grabarClienteBIN()
        {
            this.Title = "Almaceno Archivo Binario :" + x;
            /////////////////////////////////
            //Proceso almacenar datos
            ClienteCache cli = new ClienteCache();
            cli.RutCliente = txtRut.Text;
            cli.RazonSocial = txtRazonSocial.Text;
            cli.NombreContacto = txtNombre.Text;
            cli.Telefono = txtTelefono.Text;
            cli.MailContacto = txtMail.Text;
            cli.Direccion = txtDireccion.Text;
            if (cboActividadEmpresa.SelectedIndex!=-1)
            {
                cli.IdActividadEmpresa = ((ActividadEmpresa)cboActividadEmpresa.SelectedItem).IdActividadEmpresa;
            }
            else
            {
                cli.IdActividadEmpresa = -1;
            }
            if (cboTipoEmpresa.SelectedIndex!=-1)
            {
                cli.IdTipoEmpresa = ((TipoEmpresa)cboTipoEmpresa.SelectedItem).IdTipoEmpresa;
            }
            else
            {
                cli.IdTipoEmpresa = -1;
            }
            string ruta = @"c:/Copias/ArchivoBin.bin";
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
            Stream grabar_Archivo = File.Create(ruta);
            BinaryFormatter serializador = new BinaryFormatter();
            serializador.Serialize(grabar_Archivo, cli);
            grabar_Archivo.Close();
            
        }


        public void limpiar()
        {
            txtRut.Clear();
            txtRazonSocial.Clear();
            txtNombre.Clear();
            txtTelefono.Clear();
            txtMail.Clear();
            txtDireccion.Clear();
            cboActividadEmpresa.SelectedIndex = -1;
            cboTipoEmpresa.SelectedIndex = -1;
        }


        public void llenar(Cliente cli)
        {
            txtRut.Text = cli.RutCliente;
            txtRazonSocial.Text = cli.RazonSocial;
            txtNombre.Text = cli.NombreContacto;
            txtTelefono.Text = cli.Telefono;
            txtMail.Text = cli.MailContacto;
            txtDireccion.Text = cli.Direccion;
            ActividadEmpresa act = new ActividadEmpresa() { IdActividadEmpresa = cli.IdActividadEmpresa };
                act.Read();
            cboActividadEmpresa.Text = act.Descripcion;
            TipoEmpresa tip = new TipoEmpresa() { IdTipoEmpresa = cli.IdTipoEmpresa };
                tip.Read();
            cboTipoEmpresa.Text = tip.Descripcion;
        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            limpiar();
            txtRut.Focus();
        }


        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente cli = new Cliente();
                cli.RutCliente = txtRut.Text;
                cli.RazonSocial = txtRazonSocial.Text;
                cli.NombreContacto = txtNombre.Text;
                cli.Telefono = txtTelefono.Text;
                cli.MailContacto = txtMail.Text;
                cli.Direccion = txtDireccion.Text;
                if (cboActividadEmpresa.SelectedIndex>=0)
                {
                    cli.IdActividadEmpresa = ((ActividadEmpresa)cboActividadEmpresa.SelectedItem).IdActividadEmpresa;
                }
                else
                {
                    throw new Exception("Falta llenar el campo Actividad Empresa");
                }
                if (cboTipoEmpresa.SelectedIndex>=0)
                {
                    cli.IdTipoEmpresa = ((TipoEmpresa)cboTipoEmpresa.SelectedItem).IdTipoEmpresa;
                }
                else
                {
                    throw new Exception("Falta llenar el campo Tipo de Empresa");
                }

                bool resp = cli.Create();
                await this.ShowMessageAsync("Guardar:",resp ? "Cliente Guardado" : "Cliente NO Guardo");
                limpiar();
                txtRut.Focus();

            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                await this.ShowMessageAsync("ERROR:",ex.Message);
            }
        }


        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente cli = new Cliente();
                cli.RutCliente = txtRut.Text;
                bool resp = cli.Read();
                if (resp)
                {
                    llenar(cli);
                }
                else
                {
                    throw new Exception("Cliente no encontrado");
                }
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }


        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cliente cli = new Cliente();
                cli.RutCliente = txtRut.Text;
                cli.RazonSocial = txtRazonSocial.Text;
                cli.NombreContacto = txtNombre.Text;
                cli.Telefono = txtTelefono.Text;
                cli.MailContacto = txtMail.Text;
                cli.Direccion = txtDireccion.Text;
                if (cboActividadEmpresa.SelectedIndex >= 0)
                {
                    cli.IdActividadEmpresa = ((ActividadEmpresa)cboActividadEmpresa.SelectedItem).IdActividadEmpresa;
                }
                else
                {
                    throw new Exception("Falta llenar el campo Actividad Empresa");
                }
                if (cboTipoEmpresa.SelectedIndex >= 0)
                {
                    cli.IdTipoEmpresa = ((TipoEmpresa)cboTipoEmpresa.SelectedItem).IdTipoEmpresa;
                }
                else
                {
                    throw new Exception("Falta llenar el campo Tipo de Empresa");
                }

                bool resp = cli.Update();
                if (resp)
                {
                    await this.ShowMessageAsync("Actualizar:", "Cliente actualizado");
                    limpiar();
                    txtRut.Focus();
                }
                else
                {
                    throw new Exception("Cliente no existe");
                }

            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }


        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageDialogResult resultado = await this.ShowMessageAsync("Eliminar:", "Desea eliminar al cliente?",MessageDialogStyle.AffirmativeAndNegative);
                if (resultado == MessageDialogResult.Affirmative)
                {
                    bool respuestaContrato = new Contrato() { RutCliente = txtRut.Text }.ReadByRut();
                    if (respuestaContrato)
                    {
                        throw new Exception("No se puede eliminar un CLIENTE vinculado a un CONTRATO");
                    }
                    else
                    {
                        Cliente cli = new Cliente() { RutCliente = txtRut.Text };
                        bool resp = cli.Delete();
                        if (resp)
                        {
                            await this.ShowMessageAsync("Eliminar:", "Cliente Eliminado");
                            limpiar();
                            txtRut.Focus();
                        }
                        else
                        {
                            throw new Exception("Cliente no existe");
                        }
                    }

                }
                else
                {
                    limpiar();
                    txtRut.Focus();
                }
            }
            catch (Exception ex)
            {
                Logger.mensaje(ex.Message);
                await this.ShowMessageAsync("ERROR:", ex.Message);
            }
        }

        private void btnListaCliente_Click(object sender, RoutedEventArgs e)
        {
            WpfListaCliente ventana = new WpfListaCliente(this);
            ventana.Show();
        }

    }
}
