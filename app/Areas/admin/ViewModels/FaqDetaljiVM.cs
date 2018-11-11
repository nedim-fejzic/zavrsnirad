using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class FaqDetaljiVM
    {
        public int Id { get; set; }
        public string Pitanje { get; set; }
        public string Odgovor { get; set; }
        public string Vidljivo { get; set; }
        public string Kategorija { get; set; }

    }
}