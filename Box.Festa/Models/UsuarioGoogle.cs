using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Box.Festa.Models
{
    public class UsuarioGoogle
    {

        public string id { get; set; }
        public string email { get; set; }
        public bool verified_email { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string locale { get; set; }

    }
}