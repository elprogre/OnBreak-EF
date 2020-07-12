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
        public void SalvarContratoCache(ContratoSalvar cont)
        {
            BinaryFormatter formato = new BinaryFormatter();
            Stream stream = File.Create(@"Contrato.bin");
            formato.Serialize(stream, cont);
            stream.Close();
        }

        public void SalvarEventoCache(EventoSalvar eve)
        {
            BinaryFormatter formato = new BinaryFormatter();
            Stream stream = File.Create(@"Evento.bin");
            formato.Serialize(stream, eve);
            stream.Close();
        }

        public ContratoSalvar RecuperarContratoCache()
        {
            BinaryFormatter formato = new BinaryFormatter();
            Stream stream = File.OpenRead(@"Contrato.bin");
            ContratoSalvar cont = (ContratoSalvar)formato.Deserialize(stream);
            stream.Close();
            return cont;
        }

        public EventoSalvar RecuperarEventoCache()
        {
            BinaryFormatter formato = new BinaryFormatter();
            Stream stream = File.OpenRead(@"Evento.bin");
            EventoSalvar eve = (EventoSalvar)formato.Deserialize(stream);
            stream.Close();
            return eve;
        }
    }
}
