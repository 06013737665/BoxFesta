using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Box.Festa.Models
{ 
    public class BasePojo
    {
        [Key]
        public long Id { get; set; }
       
    }
}
