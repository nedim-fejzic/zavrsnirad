using app.Areas.admin.ViewModels;
using app.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace app.Areas.admin.Controllers
{
    [AuthUposlenik]
    public class KorisnikController : Controller
    {
        private MojKontekst db = new MojKontekst();


        // GET: admin/Korisnik
        public ActionResult Index()
        {


            return RedirectToAction("Search");



            //prikaz korisnika
            //KorisnikIndexVM model = new KorisnikIndexVM();

            //model.KorisniciRedovi = db.KorisnikDbSet
            // .Select(x => new KorisnikRed
            // {
            //     Id = x.Id,
            //     ImePrezime = x.Ime + " " + x.Prezime,
            //     JMBG = x.JMBG,
            //     Opcina = x.Opcina.NazivOpcine


            // }).ToList();




            //return View(model);

        }

        public ActionResult Dodaj()
        {
            KorisnikDodajVM model = new KorisnikDodajVM();
            model.ListaOpcina = db.OpcinaDbSet.ToList();

            return View(model);

        }
        [HttpPost]
        public ActionResult Dodaj(KorisnikDodajVM model)
        {



            if (!ModelState.IsValid)
            {


                model.ListaOpcina = db.OpcinaDbSet.ToList();

                return View(model);
            }

            foreach (var k in db.KorisnikDbSet.ToList())
            {
                if (k.JMBG == model.JMBG)
                {

                    ModelState.AddModelError("JMBG", "Korisnik sa istim JMBG vec postoji!");
                    model.ListaOpcina = db.OpcinaDbSet.ToList();

                    return View(model);
                }
            }


            Korisnik v = new Korisnik()
            {
                Adresa = model.Adresa,
                DatumIzmjene = DateTime.Now,
                DatumUnosa = DateTime.Now,
                Email = model.Email,
                Ime = model.Ime,
                JMBG = model.JMBG,
                Naselje = model.Naselje,
                OpcinaId = model.OpcinaId,
                Prezime = model.Prezime,
                Telefon = model.Telefon
            };

            db.KorisnikDbSet.Add(v);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult Uredi(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (db.KorisnikDbSet.Find(id) == null)
            {
                return HttpNotFound();
            }

            KorisnikUrediVM model = db.KorisnikDbSet
              .Where(x => x.Id == id)
              .Select(f => new KorisnikUrediVM
              {
                  Id = f.Id,
                  ListaOpcina = db.OpcinaDbSet.ToList(),
                  Adresa = f.Adresa,
                  Email = f.Email,
                  JMBG = f.JMBG,
                  Ime = f.Ime,
                  Naselje = f.Naselje,
                  Prezime = f.Prezime,
                  Telefon = f.Telefon,
                  OpcinaId = f.OpcinaId

              }).Single();


            return View(model);

        }
        [HttpPost]
        public ActionResult Uredi(KorisnikUrediVM model)
        {

            if (!ModelState.IsValid)
            {
                model.ListaOpcina = db.OpcinaDbSet.ToList();
                return View(model);
            }

            Korisnik o = db.KorisnikDbSet.Find(model.Id);

            if (o == null)
                return HttpNotFound();

            o.Ime = model.Ime;
            o.Prezime = model.Prezime;
            o.Telefon = model.Telefon;
            o.JMBG = model.JMBG;
            o.Naselje = model.Naselje;
            o.OpcinaId = model.OpcinaId;
            o.Adresa = model.Adresa;
            o.DatumIzmjene = DateTime.Now;
            o.Email = model.Email;




            db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult Zahtjev(int? ZahtjevId)
        {

            if (ZahtjevId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zahtjev z = db.ZahtjevDbSet.Find(ZahtjevId);
            if (z == null)
            {
                return HttpNotFound();
            }


            if (z.JMBG == null)
            {
                TempData["Naslov"] = "Greška!";
                TempData["Message"] = "Korisnik nije unio JMBG!";
                TempData["code"] = "error";



                return RedirectToAction("Details", "Zahtjevi", new { id = z.Id });
            }

            foreach (var k in db.KorisnikDbSet.ToList())
            {
                if (k.JMBG == z.JMBG)
                {
                    TempData["Message"] = "Već postoji korisnik sa navedenim JMBG";
                    TempData["code"] = "error";



                    // mozda proslijediti ID i pitati da otvori tog korisnika

                    return RedirectToAction("Details", "Zahtjevi", new { id = z.Id, area = "Admin" });

                }
            }


            Korisnik v = new Korisnik()
            {
                Adresa = z.NazivUlice,
                DatumIzmjene = DateTime.Now,
                DatumUnosa = DateTime.Now,
                Email = z.Email,
                Ime = z.Ime,
                JMBG = z.JMBG,
                Naselje = z.Naselje,
                OpcinaId = z.OpcinaId,
                Prezime = z.Prezime,
                Telefon = z.Telefon,
            };

            v.Username = (v.Ime + "." + v.Prezime).ToLower();
            v.Password =  "Pw" +  v.JMBG;
            

            db.KorisnikDbSet.Add(v);
            db.SaveChanges();


            return RedirectToAction("Detalji", "Korisnik", new { id = v.Id, area = "Admin" });
        }





        public ActionResult Detalji(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Korisnik k = db.KorisnikDbSet.Find(id);

            if (k == null)
            {
                return HttpNotFound();
            }

            KorisnikDetaljiVM model = db.KorisnikDbSet
              .Where(x => x.Id == id)
              .Select(f => new KorisnikDetaljiVM
              {
                  Id = f.Id,
                  JMBG = f.JMBG,
                  Ime = f.Ime,
                  Prezime = f.Prezime,
                  Lokacija = f.Opcina.NazivOpcine + " " + f.Naselje + " " + f.Adresa,
                  ListaAktivnihUsluga = db.AktivneUslugeDbSet.Where(q => q.KorisnikId == f.Id).ToList(),
                  ListaRacuna = db.RacuniDbSet.Where(q => q.KorisnikId == f.Id).ToList(),
                  ListaSmetnji = db.SmetnjeDbSet.Where(q => q.KorisnikId == f.Id).ToList(),

              }).Single();

            model.ListaZahtjeva = db.ZahtjevDbSet.Where(q => q.JMBG == k.JMBG).ToList();



            return View(model);

        }


        public ActionResult Search(int? page, string jmbg, string Sifra, string ImePrezime)
        {


            KorisnikSearchVM model = new KorisnikSearchVM();

            var users = db.KorisnikDbSet.OrderByDescending(x => x.DatumIzmjene).ToList();

            if (!string.IsNullOrEmpty(jmbg))
                users = users.Where(i => i.JMBG == jmbg).ToList();
            if (!string.IsNullOrEmpty(ImePrezime))
                users = users.Where(i => i.Ime.Contains(ImePrezime) || i.Prezime.Contains(ImePrezime)).ToList();

            if (!string.IsNullOrEmpty(Sifra))
            {
                int a = Convert.ToInt32(Sifra);

                if (a>0 && a<99999)
                    users = users.Where(i => i.Id == a).ToList();
            }


            model.ListaRezultata = users.ToPagedList(page ?? 1, 10);


            return View(model);


        }

        [HttpGet]
        public ActionResult Lozinka(int? id)
        {
            // ako je null potrebno je izabrati korisnika
            // ako nije null to je korisnik
            KorisnikLozinkaVM model = new KorisnikLozinkaVM();



            var user = db.KorisnikDbSet.Find(id);

            if (user != null)
            {
                model.KorisnikId = user.Id;
            }

            model.ListaKorisnika = db.KorisnikDbSet.ToList();


            Random rnd = new Random();
            string newPassword = Membership.GeneratePassword(9, 0);
            newPassword = Regex.Replace(newPassword, @"[^a-zA-Z0-9]", m => rnd.Next(0, 10).ToString());

            model.Lozinka = newPassword;

            return View(model);

        }
        [HttpPost]
        public ActionResult Lozinka(KorisnikLozinkaVM model)
        {

            Korisnik k = db.KorisnikDbSet.Find(model.KorisnikId);

            k.Password = model.Lozinka;
            db.SaveChanges();


            Korisnik ka = db.KorisnikDbSet.Find(model.KorisnikId);

            TempData["code"] = "info";
            TempData["Message"] = "Lozinka je uspješno promjenjena. ";

            return RedirectToAction("Printaj", model);

            }


        public ActionResult Printaj(KorisnikLozinkaVM model)
        {

            Korisnik k = db.KorisnikDbSet.Find(model.KorisnikId);
            var upl = db.UposlenikDbSet.Find(Convert.ToInt32(Session["logiran_uposlenik"]));
            string uposlenik = upl.Ime + " " + upl.Prezime;

            string slova = "11111 - ----- šđžčć  -----     - Testing of letters    \u010c,\u0106,\u0160,\u017d,\u0110";

            int ukupnokolona = 3;


            PdfPTable pdftabela = new PdfPTable(2);
            PdfPTable tabela2 = new PdfPTable(2);
            PdfPTable tabela3 = new PdfPTable(3);


            PdfPCell celija;

            string FONT = "c:/Windows/Fonts/arial.ttf";
            Font font = FontFactory.GetFont(FONT, BaseFont.IDENTITY_H, true);

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document();

                document.SetPageSize(PageSize.A4);
                document.SetMargins(50f, 50f, 20f, 20f);

                pdftabela.WidthPercentage = 100;
                pdftabela.HorizontalAlignment = Element.ALIGN_LEFT;

                tabela2.WidthPercentage = 70;
                tabela2.HorizontalAlignment = Element.ALIGN_LEFT;

                tabela3.WidthPercentage = 100;
                tabela3.HorizontalAlignment = Element.ALIGN_LEFT;



                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();

                /////////////////////////////////////////////////////////////////////////////////////////////////
                /////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////


                //string fontpath = Server.MapPath("~/Content/fonts/");

                //BaseFont customfont = BaseFont.CreateFont(fontpath + "temp.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                //Font font = new Font(customfont, 12);

                BaseFont nf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, false);



                Font fontsadrzaj = new Font(nf, 11);
                Font font8 = new Font(nf, 8);
                Font font8bold = new Font(bf, 8);

                Font font11 = new Font(nf, 11);
                Font font11bold = new Font(bf, 11);

                Font font16 = new Font(nf, 16);
                Font font16Bold = new Font(bf, 16);



                string putanja = Server.MapPath("~/Slike/");
                Image header = Image.GetInstance(putanja + "header.png");

                //////////////////////////////////////////////////////////////////////////

                header.ScaleToFit(PageSize.A4.Width - 90f, 80f);
                header.SpacingBefore = 10f;
                header.SpacingAfter = 10f;
                header.Alignment = Element.ALIGN_LEFT;
                document.Add(header);

                //////////////////////////////////////////////////////////////////////////

                pdftabela.SetWidths(new float[] { 160f, 100f });
                tabela2.SetWidths(new float[] { 20, 80 });
                tabela3.SetWidths(new float[] { 40, 60, 40 });

                #region header
                //celija = new PdfPCell(new Phrase("firmino adam mališ", fontnaslov));
                //celija.Colspan = ukupnokolona;
                //celija.HorizontalAlignment = Element.ALIGN_CENTER;
                //celija.Border = 0;
                //celija.BackgroundColor = BaseColor.WHITE;
                //celija.ExtraParagraphSpace = 0;
                //pdftabela.AddCell(celija);
                //pdftabela.CompleteRow();

                //celija = new PdfPCell(new Phrase("222 firma 222", fontnaslov));
                //celija.Colspan = ukupnokolona;
                //celija.HorizontalAlignment = Element.ALIGN_CENTER;
                //celija.Border = 0;
                //celija.BackgroundColor = BaseColor.WHITE;
                //celija.ExtraParagraphSpace = 0;
                //celija.PaddingBottom = 10;

                //pdftabela.AddCell(celija);
                //pdftabela.CompleteRow();
                #endregion




                #region tijelo

                /////////////////////////////////////////////////////////////

                //celija = new PdfPCell(new Phrase("#", fontnaslov));
                //celija.HorizontalAlignment = Element.ALIGN_CENTER;
                //celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                //celija.BackgroundColor = BaseColor.LIGHT_GRAY;
                //pdftabela.AddCell(celija);
                pdftabela.SpacingBefore = 50f;

                var siva = new BaseColor(220, 220, 220);


                celija = new PdfPCell(new Phrase("Zahtjev za prikaz korisničkih podataka: ", font16));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                pdftabela.AddCell(celija);

                celija = new PdfPCell(new Phrase("Datum izdavanja: " + DateTime.Now.ToString("dd.MM.yyyy"), font11));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                pdftabela.AddCell(celija);


                tabela2.SpacingBefore = 50f;


                celija = new PdfPCell(new Phrase("Korisnik: ", font16));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 50f;
                tabela2.AddCell(celija);



                celija = new PdfPCell(new Phrase(k.Ime + " " + k.Prezime, font16Bold));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = siva;
                celija.BorderColor = BaseColor.WHITE;
                celija.MinimumHeight = 50f;
                celija.PaddingLeft = 20;
                //celija.Border = Rectangle.NO_BORDER;
                tabela2.AddCell(celija);

                tabela2.CompleteRow();






                celija = new PdfPCell(new Phrase("Lozinka: ", font16));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.Border = Rectangle.NO_BORDER;
                celija.BackgroundColor = BaseColor.WHITE;

                celija.MinimumHeight = 50f;

                tabela2.AddCell(celija);

                celija = new PdfPCell(new Phrase(model.Lozinka, font16Bold));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = siva;
                celija.BorderColor = BaseColor.WHITE;
                celija.PaddingLeft = 20;
                //celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 50f;

                tabela2.AddCell(celija);


                tabela2.CompleteRow();





                #endregion


                document.Add(pdftabela);
                document.Add(tabela2);

                Paragraph p1 = new Paragraph("Napomena:", font11bold);

                Paragraph p2 = new Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore " +
                    "et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat." +
                    " Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidata" +
                    "t non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", font11);

                p1.SpacingBefore = 50;





                tabela3.SpacingBefore = 70f;


                celija = new PdfPCell(new Phrase("Primio: ", font11));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 50f;
                tabela3.AddCell(celija);

                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                celija = new PdfPCell(new Phrase("Izdao: ", font11));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.MinimumHeight = 50f;
                celija.PaddingLeft = 20;
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                tabela3.CompleteRow();



                celija = new PdfPCell(new Phrase("__________________", font11));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 30f;
                tabela3.AddCell(celija);

                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                celija = new PdfPCell(new Phrase("__________________", font11));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.MinimumHeight = 30f;
                celija.PaddingLeft = 20;
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                tabela3.CompleteRow();

                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                celija = new PdfPCell(new Phrase(uposlenik, font11));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.PaddingLeft = 20;
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                tabela3.CompleteRow();








                document.Add(p1);
                document.Add(p2);
                document.Add(tabela3);


                document.Close();

                byte[] bytes = ms.ToArray();
                ms.Close();



               


                return File(bytes, "application/pdf");

            }




        }

    }


     
    
}