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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BibliotecaControles
{
    /// <summary>
    /// Lógica de interacción para UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            dtgFecha.DisplayDateStart = DateTime.Now;
            dtgFecha.DisplayDateEnd = DateTime.Now.AddMonths(6);
            for (int i = 0; i < 24; i++)
            {
                cboHora.Items.Add(i);
            }
            for (int i = 0; i < 60; i++)
            {
                cboMinutos.Items.Add(i);
            }
        }
        public DateTime RecuperarFechaHora()
        {
            try
            {
                int anno = ((DateTime)dtgFecha.SelectedDate).Year;
                int mes = ((DateTime)dtgFecha.SelectedDate).Month;
                int dia = ((DateTime)dtgFecha.SelectedDate).Day;
                int hora = int.Parse(cboHora.SelectedValue.ToString());
                int minuto = int.Parse(cboMinutos.SelectedValue.ToString());
                DateTime fyh = new DateTime(anno,mes,dia,hora,minuto,0);
                return fyh;
            }
            catch (Exception)
            {
                throw new ArgumentException("Error en recuperar datos");
            }
        }

        public void VerFechaYHora(DateTime fyh)
        {
            try
            {

            }
            catch (Exception)
            {
                throw new ArgumentException("No se puede visualizar la fecha y hora");
            }
            dtgFecha.Text = fyh.ToString("dd/MM/yyyy");
            string hora = fyh.ToString("HH");
            string minu = fyh.ToString("mm");
            cboHora.Text = hora;
            cboMinutos.Text = minu;
        }

        public void LimpiarControl()
        {
            dtgFecha.SelectedDate=DateTime.Now;
            cboHora.SelectedIndex = 0;
            cboMinutos.SelectedIndex = 0;
        }

    }
}
