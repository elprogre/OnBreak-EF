using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WS_UF
{
    public class ClaseUF
    {
        public string version { get; set; }
        public string autor { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string unidad_medida { get; set; }
        public List<serie> serie { get; set; }

    }
    public class serie
    {
        public string fecha { get; set; }
        public string valor { get; set; }

        public serie()
        {

        }
    }

}