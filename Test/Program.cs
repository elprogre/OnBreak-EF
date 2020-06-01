using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblioteca.Negocio;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                foreach (ActividadEmpresa item in new ActividadEmpresa().ReadAll())
                {
                    Console.WriteLine("IdActividadEmpresa:"+item.IdActividadEmpresa+" Descripcion:"+item.Descripcion);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
            Console.ReadKey();
        }
    }
}
