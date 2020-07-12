using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace WS_UF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class Service1 : IService1
    {
        public int suma(int p1, int p2)
        {
            return p1+p2;
        }

        public double UF()
        {
            ClaseUF datos;
            HttpWebRequest request =
                (HttpWebRequest) WebRequest.Create(@"https://mindicador.cl/api/uf");
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader stream_reader = new StreamReader(stream);
            var json = stream_reader.ReadToEnd();
            datos = JsonConvert.DeserializeObject<ClaseUF>(json);

            string uf = "";
            foreach (serie item in datos.serie)
            {
                uf = item.valor;
                break;
            }
            uf = uf.Replace(".",",");
            return double.Parse(uf);
        }
    }
}
