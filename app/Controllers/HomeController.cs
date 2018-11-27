using app.Areas.admin.ViewModels;
using app.Models;
using app.ViewModels;
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

namespace app.Controllers
{
    public class HomeController : Controller
    {
        private MojKontekst db = new MojKontekst();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Paketi()
        {
            HomePaketiVM model = new HomePaketiVM();

            model.PaketRedovi = db.PaketDbSet
             .Select(x => new ViewModels.PaketRed
             {
                 Id = x.Id,
                 CijenaSaPDV = x.CijenaSaPdv.ToString(),
                 DownSpeed = x.DownloadSpeed,
                 Flat = x.FlatRate ? "DA" : "NE",
                 Naziv = x.Naziv,
                 SlikaPutanja = x.ImageUrl == null ? "http://localhost:57805/Slike/404.jpg" : x.ImageUrl,
                 UpSpeed = x.UploadSpeed,
                 Kategorija = x.TipUsluga.Naziv,
                 KategorijaId = x.TipUslugaId

             }).ToList();

            model.KategorijeL = db.TipUslugaDbSet.ToList();

            return View(model);
        }

        public ActionResult FAQ()
        {
            HomeFaqVM model = new HomeFaqVM();

            model.FaqRedovi = db.FAQDbSet
                .Where(x=>x.Vidljiv == true)
             .Select(x => new ViewModels.FaqRed
             {
                 Id = x.Id,
                 KategorijaId = x.FAQKategorijaId,
                 Odgovor = x.Odgovor,
                 Pitanje = x.Pitanje

             }).ToList();

            return View(model);
        }

        [AuthKorisnik]
        [HttpGet]
        public ActionResult Promjena(int id)
        {
            HomePromjeniVM model = new HomePromjeniVM();


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AktivneUsluge a = db.AktivneUslugeDbSet.Find(id);
            if (a == null)
            {
                return HttpNotFound();
            }

            model.AktivnaUslugaId = id;
            model.Usluga = a;
            model.ListaNovihUsluga = db.PaketDbSet.ToList();
            model.KorisnikId = model.Usluga.KorisnikId;

            //model.ListaNovihUsluga = db.PaketDbSet.ToList();
            //model.ListaNovihUsluga.Insert(0, new Models.Paket() { Id = 0, Naziv = "Zelim iskljuciti uslugu..." });

            return View(model);
        }
        [HttpPost]
        public ActionResult Promjena(HomePromjeniVM model)
        {
            AktivneUsluge au = db.AktivneUslugeDbSet.Find(model.AktivnaUslugaId);
           

            if (model.IzabranaId == au.Paket.Id)
            {
                ModelState.AddModelError("IzabranaId", "Potrebno je izabrati razlicitu uslugu da bi izvrsitil promjenu!");
            }


            if (!ModelState.IsValid)
            {

                model.Usluga = db.AktivneUslugeDbSet.Find(model.AktivnaUslugaId);
                model.ListaNovihUsluga = db.PaketDbSet.ToList();
                return View(model);
            }


            // napravi novi zahtjev + " - " + model.ListaNovihUsluga.Where(c => c.Id == model.IzabranaId).ToString()
            // pokazi alert

            // napravi novi zahtjev 
            // pokazi alert    + " <br> " + model.ListaRazloga.Where(c => c.Id == model.RazlogId).ToString()
            ZahtjevPromjena z = new ZahtjevPromjena();
            z.KorisnikId = db.AktivneUslugeDbSet.Where(c => c.Id == model.KorisnikId).First().KorisnikId;
            z.AktivneUslugeId = model.AktivnaUslugaId;
            z.Datum = DateTime.Now;
            z.ZahtjevstatusId = 1;
            z.Napomena = model.Napomena;
            z.PaketId = model.IzabranaId;


            db.ZahtjevPromjenaDbSet.Add(z);
            db.SaveChanges();


            

            TempData["Message"] = "Poslali ste zahtjev za promjenu usluge: <b>" + db.AktivneUslugeDbSet.Where(c=>c.Id == model.AktivnaUslugaId).First().Paket.Naziv + " </b><br> u uslugu: <b>" +  db.PaketDbSet.Where(c=>c.Id == model.IzabranaId).First().Naziv + "</b>";
            TempData["code"] = "info";


            return RedirectToAction("Index", "Profil", new { @id = model.KorisnikId });
        }


        [AuthKorisnik]
        [HttpGet]
        public ActionResult Iskljuci(int id)
        {
            HomeIskljuciVM model = new HomeIskljuciVM();


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AktivneUsluge a = db.AktivneUslugeDbSet.Find(id);
            if (a == null)
            {
                return HttpNotFound();
            }

            model.AktivnaUslugaId = id;
            model.Usluga = a;
            model.KorisnikId = model.Usluga.KorisnikId;
            model.ListaRazloga = db.RazloziDbSet.ToList();

            //model.ListaNovihUsluga = db.PaketDbSet.ToList();
            //model.ListaNovihUsluga.Insert(0, new Models.Paket() { Id = 0, Naziv = "Zelim iskljuciti uslugu..." });

            return View(model);
        }
        [HttpPost]
        public ActionResult Iskljuci(HomeIskljuciVM model)
        {

            if (!ModelState.IsValid)
            {
                model.ListaRazloga = db.RazloziDbSet.ToList();
                model.Usluga = db.AktivneUslugeDbSet.Find(model.AktivnaUslugaId);
                return View(model);
            }


            // napravi novi zahtjev 
            // pokazi alert    + " <br> " + model.ListaRazloga.Where(c => c.Id == model.RazlogId).ToString()
            ZahtjevPromjena z = new ZahtjevPromjena();
            z.KorisnikId = db.AktivneUslugeDbSet.Where(c => c.Id == model.AktivnaUslugaId).First().KorisnikId;
            z.AktivneUslugeId = model.AktivnaUslugaId;
            z.Datum = DateTime.Now;
            z.ZahtjevstatusId = 1;
            z.Napomena = model.Napomena;
            z.Razlog = db.RazloziDbSet.Where(c => c.Id == model.RazlogId).First().Poruka;
            // paket je null znaci iskljucenje

            db.ZahtjevPromjenaDbSet.Add(z);
            db.SaveChanges();
            TempData["Message"] = "Poslali ste zahtjev za isključenje usluge. ";
            TempData["code"] = "info";




            return RedirectToAction("Index","Profil",new { @id = model.KorisnikId});

        }

      


        public ActionResult Login()
        {
            HomeLoginVM model = new HomeLoginVM();
           

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(HomeLoginVM model)
        {

          


            if (ModelState.IsValid == false)
            {
                //TempData["zahtjev"] = "zahtjev";
                model.poruka = "nema";
                return View(model);

            }


            bool nadjen = false;
            bool korisnik = false;

            Korisnik k = db.KorisnikDbSet.FirstOrDefault(x => x.Username == model.Username && x.Password == model.Password);
            Uposlenici u = db.UposlenikDbSet.FirstOrDefault(x => x.Username == model.Username && x.Password == model.Password && x.Aktivan == true);

            if (k == null)
            {

                // ako nema korisni  ka provjeri uposlenike

               
                if (u != null)
                {
                    nadjen = true;
                    korisnik = false;
                }
               

                
            }
            else
            {
                nadjen = true;
                korisnik = true;
            }

            if (nadjen == false)
            {
                model.poruka = "nema";
                return View(model);
            }
            else
            {   
                // ima korisnik
                if (korisnik == true)
                {
                    //TempData["zahtjev"] = "zahtjev";
                    //TempData["Message"] = "Uspješno ste se prijavili!";
                    //TempData["code"] = "info";

                    Session["logiran_korisnik"] = k.Id;
                    Session["imeprezime"] = k.Ime + " " + k.Prezime;

                    return RedirectToAction("Index");

                }
                else
                {
                    //TempData["Message"] = "Uspješno ste se prijavili!";
                    //TempData["code"] = "info";

                    Session["logiran_uposlenik"] = u.Id;
                    Session["role"] = u.Uloga.Sifra;
                    Session["imeprezime"] = u.Ime + " " + u.Prezime;

                    return RedirectToAction("Index", "Start", new { area = "admin" });


                }
            }

        }






        public ActionResult Odjava()
        {

            Session["logiran_korisnik"] = null;
            Session["imeprezime"] = null;
            Session["logiran_uposlenik"] = null;
            Session["role"] = null;

            //TempData["Message"] = "Uspješno ste se odjavili!";
            //TempData["code"] = "info";


            return RedirectToAction("Index", "Home", new { area = "" });
        }



        public ActionResult Kontakt()
        {

            TempData["zahtjev"] = "zahtjev";


            return RedirectToAction("Index");
        }




        public ActionResult Zahtjev()
        {
            HomeZahtjevVM model = new HomeZahtjevVM();

            model.ListaPaketica = new List<ListaPaketaView> { };

            foreach (var item in db.PaketDbSet.ToList())
            {
                ListaPaketaView a = new ListaPaketaView
                {
                    Paket = item,
                    Izabran = false
                };
                model.ListaPaketica.Add(a);
            }


            model.ListaTipKontakta = db.TipKontaktaDbSet.ToList();
            model.ListaOpcina = db.OpcinaDbSet.ToList();

            //model.IzabraniPaketi = new List<int>();
            return View(model);
        }
        public ActionResult ZahtjevUsluga(int id)
        {
            HomeZahtjevVM model = new HomeZahtjevVM();

            model.ListaPaketica = new List<ListaPaketaView> { };

            foreach (var item in db.PaketDbSet.ToList())
            {
                ListaPaketaView a = new ListaPaketaView
                {
                    Paket = item,
                    Izabran =  (item.Id == id) ? true : false
                };


                model.ListaPaketica.Add(a);
            }


            model.ListaTipKontakta = db.TipKontaktaDbSet.ToList();
            model.ListaOpcina = db.OpcinaDbSet.ToList();

            //model.IzabraniPaketi = new List<int>();
            return View("Zahtjev", model);
        }
        [HttpPost]
        public ActionResult Zahtjev(HomeZahtjevVM model)
        {

            // ako nije nista izabravno u paketima dodaj error
            //@Html.ValidationMessageFor(model => Model.IzabraniPaketi, "", new { @class = "text-danger" })

            if (!ModelState.IsValid)
            {

                model.ListaPaketica = new List<ListaPaketaView> { };

                foreach (var item in db.PaketDbSet.ToList())
                {
                    ListaPaketaView a = new ListaPaketaView
                    {
                        Paket = item,
                        Izabran = false
                    };
                    model.ListaPaketica.Add(a);
                }


                if (model.IzabraniPaketi != null)
                {

                    foreach (var u in model.IzabraniPaketi)
                    {
                        foreach (var i in model.ListaPaketica)
                        {
                            if (u == i.Paket.Id)
                            {
                                i.Izabran = true;
                            }
                        }
                    }
                }




                model.ListaTipKontakta = db.TipKontaktaDbSet.ToList();
                model.ListaOpcina = db.OpcinaDbSet.ToList();

                return View(model);
            }

            //string a = "";
            //foreach (var item in model.PaketiciId)
            //{
            //     a = a + item.ToString() +" - " ;
            //}

            //IList<int> ee = model.IzabraniPaketi;



            //var odabrano = string.Join(",", model.IzabraniPaketi);
            //return Content("<script language='javascript' type='text/javascript'>alert('" + odabrano + " ');</script>");


            Zahtjev z = new Zahtjev()
            {
                Broj = model.Broj,
                Email = model.Email,
                Ime = model.Ime,
                NapomenaKorisnika = model.NapomenaKorisnika,
                Telefon = model.Telefon,
                TipKontaktaId = model.TipKontaktaId,
                Naselje = model.Naselje,
                NazivUlice = model.NazivUlice,
                OpcinaId = model.OpcinaId,
                Prezime = model.Prezime,
                DatumPodnosenja = DateTime.Now,
                ZahtjevstatusId = 1,
                ZatvorenZahtjev = false,
                JMBG = model.JMBG,
                DatumZatvaranja = DateTime.Parse("2000-01-01"),
                Komentar = ""
            };

            db.ZahtjevDbSet.Add(z);
            db.SaveChanges();

            // napravljen zahtjev
            // dodamo izabrane usluge u novu tabelu na osnovu  id novog zahtjeva
            int tempBrojac = 0;

            int NoviZahtjevId = z.Id;
            foreach (var paketic in model.IzabraniPaketi)
            {
                ZahtjevUsluge zu = new ZahtjevUsluge()
                {
                    ZahtjevId = NoviZahtjevId,
                    PaketId = paketic
                };

                db.ZahtjevUslugeDbSet.Add(zu);
                db.SaveChanges();
                tempBrojac++;
            }

            int brojac2 = tempBrojac;

            TempData["zahtjev"] = "zahtjev";
            //TempData["Message"] = "Uspješno ste poslali zahtjev. <br><br>Naši uposlenici će pregledati zahtjev, te vas kontaktirati. <br><br> Hvala na strpljenju!";



            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Printaj(int id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Racuni r = db.RacuniDbSet.Find(id);
            if (r == null)
            {
                return HttpNotFound();
            }


            RacunPrintajVM model = db.RacuniDbSet
              .Where(x => x.Id == id)
              .Select(f => new RacunPrintajVM
              {
                  Id = f.Id,
                  Placen = f.Placen ? "DA" : "NE",
                  DatumIzdavanja = f.DatumIzdavanja,
                  KorisnikId = f.KorisnikId,
                  PeriodDo = f.ObracunskiPeriodDO,
                  PeriodOd = f.ObracunskiPeriodOD,
                  RokPlacanja = f.RokPlacanja,
                  Sifra = f.Sifra,
                  Ukupno = f.UkupnoBezPDV.ToString(),
                  PDF = (f.UkupnoSaPDV - f.UkupnoBezPDV).ToString(),
                  UkupnoPDV = f.UkupnoSaPDV.ToString(),

              }).Single();



            model.Korisnik = db.KorisnikDbSet.Find(model.KorisnikId);
            model.ListaStavki = db.RacuniStavkeDbSet.Where(g => g.RacunId == model.Id).ToList();



            PdfPTable pdftabela = new PdfPTable(2);
            PdfPTable tabela3 = new PdfPTable(3);
            PdfPTable tabelaUsluge = new PdfPTable(7);
            PdfPTable tabelaTotal = new PdfPTable(5);


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


                tabela3.WidthPercentage = 100;
                tabela3.HorizontalAlignment = Element.ALIGN_LEFT;

                tabelaUsluge.WidthPercentage = 100;
                tabelaUsluge.HorizontalAlignment = Element.ALIGN_LEFT;

                tabelaTotal.WidthPercentage = 100;
                tabelaTotal.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                                
                BaseFont nf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, false);

                var Sivaboja = new BaseColor(25, 25, 25);

                Font fontsadrzaj = new Font(nf, 11);
                Font font8 = new Font(nf, 8);
                Font font8bold = new Font(bf, 8);
                Font font11 = new Font(nf, 11);
                Font font9 = new Font(nf, 9);
                Font font10 = new Font(nf, 10);
                Font font11bold = new Font(bf, 11);
                Font font16Bold = new Font(bf, 16);
                Font font16 = new Font(nf, 16);
                Font font20 = new Font(nf, 18);

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
                tabela3.SetWidths(new float[] { 50, 40, 50 });
                tabelaUsluge.SetWidths(new float[] { 35, 35, 35, 35, 35, 35, 35 });
                tabelaTotal.SetWidths(new float[] { 50, 50, 50, 50, 50 });



                #region tijelo

                pdftabela.SpacingBefore = 50f;


                celija = new PdfPCell(new Phrase("RAČUN - " + model.Sifra,  font20));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                pdftabela.AddCell(celija);


                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                pdftabela.AddCell(celija);

                //celija = new PdfPCell(new Phrase("Sifra " + model.Sifra, font11));
                //celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                //celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                //celija.BackgroundColor = BaseColor.WHITE;
                //celija.Border = Rectangle.NO_BORDER;
                //pdftabela.AddCell(celija);


                tabela3.SpacingBefore = 50f;


                #endregion


                document.Add(pdftabela);

               


                celija = new PdfPCell(new Phrase("Klijent: ", font11bold));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 25f;
                tabela3.AddCell(celija);

                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                celija = new PdfPCell(new Phrase("Detalji o računu: ", font11bold));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.MinimumHeight = 25f;
                celija.PaddingLeft = 20;
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                tabela3.CompleteRow();



                celija = new PdfPCell(new Phrase(model.Korisnik.Ime + " " + model.Korisnik.Prezime, font11));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 16f;
                tabela3.AddCell(celija);

                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                celija = new PdfPCell(new Phrase("Datum izdavanja: "  + model.DatumIzdavanja.ToShortDateString(), font11));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.MinimumHeight = 16f;
                celija.PaddingLeft = 20;
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                tabela3.CompleteRow();



                celija = new PdfPCell(new Phrase(model.Korisnik.Adresa + " ", font11));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 16f;
                tabela3.AddCell(celija);


                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                celija = new PdfPCell(new Phrase("Period od: " + model.PeriodOd.ToShortDateString(), font11));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.PaddingLeft = 20;
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                tabela3.CompleteRow();






                celija = new PdfPCell(new Phrase(model.Korisnik.Naselje + ", " + model.Korisnik.Opcina.NazivOpcine, font11));
                celija.HorizontalAlignment = Element.ALIGN_LEFT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 16f;
                tabela3.AddCell(celija);


                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                celija = new PdfPCell(new Phrase("Period od: " + model.PeriodDo.ToShortDateString(), font11));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.PaddingLeft = 20;
                celija.Border = Rectangle.NO_BORDER;
                tabela3.AddCell(celija);

                tabela3.CompleteRow();

                document.Add(tabela3);





                tabelaUsluge.SpacingBefore = 50f;



                celija = new PdfPCell(new Phrase("USLUGA ", font10));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("NAZIV ", font10));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("DATUM OD ", font10));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("DATUM DO ", font10));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("CIJENA ", font10));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("PDV ", font10));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("UKUPNO ", font10));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                
                /////////////////////////////////////////////////////////////////
                tabelaUsluge.CompleteRow();
                /////////////////////////////////////////////////////////////////


                celija = new PdfPCell(new Phrase("PROŠIRENI ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("TV ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("01.10.2018 ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("31.10.2018 ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("15,35 ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("3,63 ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("18,99 ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                ////////////////////////////////////////////////////////////////////////
                tabelaUsluge.CompleteRow();
                ///////////////////////////////////////////////////////////////////////////

                celija = new PdfPCell(new Phrase("PROŠIRENI ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("TV ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("01.10.2018 ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("31.10.2018 ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("15,35 ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("3,63 ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                celija = new PdfPCell(new Phrase("18,99 ", font11));
                celija.HorizontalAlignment = Element.ALIGN_CENTER;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                celija.MinimumHeight = 20f;
                tabelaUsluge.AddCell(celija);

                tabelaUsluge.CompleteRow();
                document.Add(tabelaUsluge);

                ////////////////////////////////////////////////////////////////////////


                tabelaTotal.SpacingBefore = 100f;

                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);

                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);


                celija = new PdfPCell(new Phrase("Ukupno bez PDV", font11));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);

                celija = new PdfPCell(new Phrase("PDV 17%", font11));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);

                celija = new PdfPCell(new Phrase("Ukupno sa PDV", font11));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);

                tabelaTotal.CompleteRow();

                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);

                celija = new PdfPCell();
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);


                celija = new PdfPCell(new Phrase("55.27 KM", font16));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);

                celija = new PdfPCell(new Phrase("6.92 KM", font16));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);

                celija = new PdfPCell(new Phrase("59.49 KM", font16Bold));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);

                tabelaTotal.CompleteRow();

                document.Add(tabelaTotal);



                document.Close();

                byte[] bytes = ms.ToArray();
                ms.Close();






                return File(bytes, "application/pdf");
            }
        }


        //public ActionResult Vijesti(int ? page)
        //{

        //    HomeVijestiVM model = new HomeVijestiVM();

        //    var u = db.VijestiDbSet.OrderByDescending(x => x.DatumObjave).ToList();


        //    foreach (var item in u)
        //    {
        //        item.Opis = Regex.Replace(Regex.Replace(item.Opis, @"<[^>]+>|&nbsp;", "").Trim(), @"\s{2,}", " ");
        //        if (item.Opis.Length > 70)
        //        {
        //            item.Opis = item.Opis.Substring(0, 70);
        //            item.Opis += "...";
        //        }

        //        item.Opis = item.Opis.Replace("&scaron;", "š");
        //    }


        //    model.ListaRezultata = u.ToPagedList(page ?? 1, 20);

        //    return View(model);
        //}

        //public ActionResult VijestDetalji(int id)
        //{

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var x = db.VijestiDbSet.Find(id);

        //    if (x == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        //    }


        //    HomeVijestDetaljiVM model = new HomeVijestDetaljiVM();
        //    model.Id = x.Id;
        //    model.Kategorija = x.VijestiKategorija.Naziv;
        //    model.Naslov = x.Naslov;
        //    model.Opis = x.Opis;
        //    model.ImageUrl = x.ImageUrl;
        //    model.DatumObjave = x.DatumObjave.ToString("dd.MM.yyyy");

        //    return View(model);
        //}






    }
}