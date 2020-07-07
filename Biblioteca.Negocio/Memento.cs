using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Biblioteca.Negocio
{
    public class Memento
    {
        public void Salvar(ContratoSalvar cont)
        {
            BinaryFormatter formato = new BinaryFormatter();
            Stream stream = File.Create(@"C:\Copias\Contrato.bin");
            formato.Serialize(stream, cont);
            stream.Close();
        }

        public ContratoSalvar Recuperar()
        {
            BinaryFormatter formato = new BinaryFormatter();
            Stream stream = File.OpenRead("Contrato.bin");
            ContratoSalvar cont = (ContratoSalvar)formato.Deserialize(stream);
            stream.Close();
            return cont;
        }
    }
}
