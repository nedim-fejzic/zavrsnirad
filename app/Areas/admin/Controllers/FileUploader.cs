using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace app.Areas.admin.Controllers
{
    public class FileUploader
    {
        static double PDVstopa = 17;

        public static string ApsolutnaPutanja(string adress, string name)
        {
            return System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/" + adress  + "/ "), name);
        }
        public static string RelativnaPutanja(string adress, string name)
        {
            return "~/" + adress + "/" + name;
        }



        public static string UploadFile(string folder, HttpPostedFileBase file)
        {

            string nazivFajla = file.FileName;
            // podrzani formati slika su jpg i png
            string putanjaZaSnimanje = ApsolutnaPutanja(folder, file.FileName);
            string putanjaZaBaze = RelativnaPutanja(folder, file.FileName);

            // provjeriti da li file sa istim nazivom vec postoji u file sistemu
            // ako postoji preimenuje fajl i snima ga
            if (System.IO.File.Exists(ApsolutnaPutanja(folder, file.FileName)))
            {
                // fajl vec postoji
                // nabavi unikatni naziv
                bool temp = false;
                int brojac = 1;
                string bezexstenzije = string.Concat(nazivFajla.TakeWhile((c) => c != '.'));
                string extenzija = nazivFajla.Substring(nazivFajla.LastIndexOf('.'));

                while (!temp)
                {
                    // dok postoji fajl
                    // slika.jpg
                    // slika(1).jpg
                    // slika(2).jpg

                    nazivFajla = bezexstenzije + "(" + brojac + ")" + extenzija;
                    if (!System.IO.File.Exists(ApsolutnaPutanja(folder, nazivFajla)))
                    {
                        temp = true;
                    }
                    brojac++;
                }

                file.SaveAs(ApsolutnaPutanja(folder, nazivFajla));
                putanjaZaBaze = RelativnaPutanja(folder, nazivFajla);
            }
            else
            {
                              
                file.SaveAs(ApsolutnaPutanja(folder, nazivFajla));
            }


            return putanjaZaBaze;
        }

        public static void ObrisiFile(string putanja)
        {
            // proslijedjena je relativna putanja, koja se nalazi u bazi

            System.IO.File.Delete(System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath(putanja)));

        }





        public static double CijenaSaPdf(float cijena)
        {

            return PDVstopa * cijena / 100 + cijena;
        }
        public static double CijenaSaPdf(double cijena)
        {

            return PDVstopa * cijena / 100 + cijena;
        }
        public static double CijenaSaPdf(int cijena)
        {

            return PDVstopa * cijena/100 + cijena;
        }


    }
}