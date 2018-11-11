﻿using app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class KorisnikDetaljiVM
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string JMBG { get; set; }
        public string Lokacija { get; set; }

        public List<AktivneUsluge> ListaAktivnihUsluga { get; set; }



    }
}