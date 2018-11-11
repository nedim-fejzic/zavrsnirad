using app.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.Areas.admin.ViewModels
{
    public class SmetnjeDodajVM
    {
        // korisnik id

        public int? AktivnaUslugaId { get; set; }
        public List<AktivneUsluge> ListaAktivnihUsluga { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public DateTime? DatumUocavanja { get; set; }

        public int SmetnjeStatusId { get; set; }
        public virtual SmetnjeStatus SmetnjeStatus { get; set; }
        public List<SmetnjeStatus> ListaSmetnjaStatus { get; set; }


        [Display(Name = "Opis smetnji: ")]
        [Required(ErrorMessage = "Molimo opišite smetnje koje imate!")]
        public string Opis { get; set; }







    }
}