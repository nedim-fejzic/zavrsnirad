using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.ViewModels
{
    public class HomeLozinkaVM
    {
        public int Id { get; set; }
        public string ImePrezime { get; set; }

        public string poruka { get; set; }

        public string StariPassword { get; set; }

        [Required(ErrorMessage = "Obavezno unesite staru lozinku!")]
        [DataType(DataType.Password)]
        public string StaraLozinka { get; set; }

        [Required(ErrorMessage = "Obavezno unesite lozinku!")]
        [RegularExpression("((?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,20})", ErrorMessage = "Lozinka mora sadržavati barem jedan broj, malo i veliko slovo! Veličina lozinke mora biti u rasponu 6-20 karaktera.")]
        [DataType(DataType.Password)]
        public string NovaLozinka { get; set; }

        [Required(ErrorMessage = "Molimo ponovite unesenu lozinku!")]
        [Compare(nameof(NovaLozinka), ErrorMessage = "Lozinke ne odgovaraju")]
        [DataType(DataType.Password)]
        public string NovaLozinkaPotvrda { get; set; }

    }
}