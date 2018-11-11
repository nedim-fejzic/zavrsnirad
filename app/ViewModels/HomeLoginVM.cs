using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace app.ViewModels
{
    public class HomeLoginVM
    {
        [Required(ErrorMessage = "Unesite korisničko ime!")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Unesite lozinku!")]
        [RegularExpression("((?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,20})", ErrorMessage = "Lozinka mora sadržavati barem jedan broj, malo i veliko slovo! Veličina lozinke mora biti u rasponu 6-20 karaktera.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public string poruka { get; set; }

    }
}