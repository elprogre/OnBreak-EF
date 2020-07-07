using System;
using System.IO;

namespace Biblioteca.Negocio
{
    public class Logger
    {
        public static void mensaje(string message)
        {
            message = DateTime.Now + " | " + message + Environment.NewLine;
            File.AppendAllText("Log.txt", message);
        }
    }
}