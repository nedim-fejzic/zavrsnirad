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

namespace app.Areas.admin.Controllers
{
    [AuthUposlenik]
    public class RacunController : Controller
    {

        private MojKontekst db = new MojKontekst();

        public ActionResult Index(int? page, int? OdabranKorisnik, string Naziv, string tip)
        {

            int id = OdabranKorisnik ?? 0;

            RacunIndexVM model = new RacunIndexVM();

            model.ListaKorisnika = db.KorisnikDbSet.ToList();
            model.ListaKorisnika.Insert(0, new Models.Korisnik() { Id = 0, Ime = "Odaberite" , Prezime = " korisnika... " });


            var u = db.RacuniDbSet.OrderByDescending(c=>c.DatumIzdavanja).ToList();
          

            if (id != 0)
                u = u.Where(i => i.KorisnikId == id).ToList();

            if (!string.IsNullOrEmpty(Naziv))
                u = u.Where(i =>  i.Sifra.Contains(Naziv)).ToList();


            if (tip != null && tip != "")
            {
                if (tip == "P")
                {
                    u = u.Where(i => i.Placen == true).ToList();

                }
                else if (tip == "N")
                {
                    u = u.Where(i => i.Placen == false).ToList();
                }
            }

            model.ListaRezultata = u.ToPagedList(page ?? 1, 10);
            model.tip = tip;
            model.PlacenoRacuna = db.RacuniDbSet.Where(c => c.Placen == true).Count().ToString();
            model.NijePlacenoRacuna = db.RacuniDbSet.Where(c => c.Placen == false).Count().ToString();

            return View(model);


        }

        [HttpGet]
        public ActionResult Kreiraj()
        {
            RacunKreirajVM model = new RacunKreirajVM();
            model.Mjesec = DateTime.Now.Month.ToString();
            model.Godina = DateTime.Now.Year.ToString();
            return View(model);
        }
        [HttpPost]
        public ActionResult Kreiraj(RacunKreirajVM model)
        {
            if (!string.IsNullOrEmpty(model.Mjesec))
            {
                if (!Regex.IsMatch(model.Mjesec, @"^\d+$"))
                {
                    ModelState.AddModelError(string.Empty, "Niste unijeli ispravan mjesec!");
                    return View(model);
                }
            }
            if (!string.IsNullOrEmpty(model.Godina))
            {
                if (!Regex.IsMatch(model.Mjesec, @"^\d+$"))
                {
                    ModelState.AddModelError(string.Empty, "Niste unijeli ispravnu godinu!");
                    return View(model);
                }
            }
            int mjesec = Int32.Parse(model.Mjesec);
            if (DateTime.Now.Month == mjesec)
            {
                ModelState.AddModelError(string.Empty, "Racuni se mogu generisati samo za prethodni mjesec!");
                return View(model);
            }
            if (db.RacuniDbSet.Where(c=>c.ObracunskiPeriodOD.Month== mjesec).Count()>0)
            {
                ModelState.AddModelError(string.Empty, "Već ste generisali račune za mjesec " + model.Mjesec + "!");
                return View(model);
            }

            int brojRacuna = 0;

            var korisnici = db.KorisnikDbSet.ToList();
            foreach (var x in korisnici)
            {
                var datum = new DateTime(Int32.Parse(model.Godina), Int32.Parse(model.Mjesec), 1);
                var datumdo = datum.AddMonths(1).AddDays(-1);

                var aktivne = db.AktivneUslugeDbSet.Where(c => c.KorisnikId == x.Id  && c.DatumAktivacije< datumdo ).ToList();
                if (aktivne !=null && aktivne.Count()>0)
                {
                    Racuni racun = new Racuni();
                    racun.KorisnikId = x.Id;
                    racun.ObracunskiPeriodOD = new DateTime(Int32.Parse(model.Godina), Int32.Parse(model.Mjesec), 1);
                    racun.ObracunskiPeriodDO =  racun.ObracunskiPeriodOD.AddMonths(1).AddDays(-1);
                    racun.RokPlacanja = DateTime.Now.AddDays(20);
                    racun.DatumIzdavanja = DateTime.Now;
                    racun.Placen = false;
                    db.RacuniDbSet.Add(racun);
                    db.SaveChanges();



                    double sumPDV = 0, sumBEZPDV = 0;

                    foreach (var stavka in aktivne)
                    {
                        RacuniStavke rs = new RacuniStavke();
                        rs.RacunId = racun.Id;
                        bool temp = false;

                        rs.AktivneUslugeId = stavka.Id;
                        rs.DatumPocetka = racun.ObracunskiPeriodOD;
                        rs.DatumKraja = racun.ObracunskiPeriodDO;
                        rs.IznosBezPDV = Math.Round(stavka.Paket.CijenaBezPdv, 2);
                        rs.IznosSaPDV = Math.Round(stavka.Paket.CijenaSaPdv, 2);

                        // ako je usluga aktivirana u toku mjeseca, pocetni datum promjeniti
                        if (stavka.DatumAktivacije > rs.DatumPocetka)
                        {
                            rs.DatumPocetka = stavka.DatumAktivacije;
                            temp = true;
                        }
                        if (stavka.DatumZatvaranja < rs.DatumKraja)
                        {
                            rs.DatumKraja = stavka.DatumZatvaranja??rs.DatumKraja;
                        }

                        if (temp)
                        {
                            rs.IznosBezPDV = Math.Round(stavka.Paket.CijenaBezPdv/30*(rs.DatumKraja-rs.DatumPocetka).TotalDays, 2);
                            rs.IznosSaPDV = Math.Round(rs.IznosBezPDV*1.17, 2);
                        }


                        db.RacuniStavkeDbSet.Add(rs);
                        db.SaveChanges();

                        sumPDV += rs.IznosSaPDV;
                        sumBEZPDV += rs.IznosBezPDV;

                        
                    }



                    racun.UkupnoBezPDV = Math.Round(sumBEZPDV, 2);
                    racun.UkupnoSaPDV = Math.Round(sumPDV, 2);
                    racun.Sifra = racun.Id.ToString() + model.Mjesec + "/" + model.Godina.Substring(model.Godina.Length - 2);
                    db.SaveChanges();




                    sumPDV = sumBEZPDV = 0;

                    brojRacuna++;



                }

            }








            TempData["Message"] = "Generisemo racune za mjesec " + model.Mjesec  + " i godinu " + model.Godina + "<br><br> Generisano je ukupno: " + brojRacuna + " racuna!";
            TempData["code"] = "info";



            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detalji(int id)
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


            RacunDetaljiVM model = db.RacuniDbSet
              .Where(x => x.Id == id)
              .Select(f => new RacunDetaljiVM
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

            return View(model);
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


                celija = new PdfPCell(new Phrase("RAČUN - " + model.Sifra, font20));
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

                celija = new PdfPCell(new Phrase("Datum izdavanja: " + model.DatumIzdavanja.ToShortDateString(), font11));
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






                celija = new PdfPCell(new Phrase( model.Korisnik.Opcina.NazivOpcine, font11));
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


                foreach (var item in model.ListaStavki)
                {
                    celija = new PdfPCell(new Phrase(item.AktivnaUsluga.Paket.Naziv, font11));
                    celija.HorizontalAlignment = Element.ALIGN_CENTER;
                    celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celija.BackgroundColor = BaseColor.WHITE;
                    celija.Border = Rectangle.NO_BORDER;
                    celija.MinimumHeight = 20f;
                    tabelaUsluge.AddCell(celija);

                    celija = new PdfPCell(new Phrase(item.AktivnaUsluga.Paket.TipUsluga.Naziv, font11));
                    celija.HorizontalAlignment = Element.ALIGN_CENTER;
                    celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celija.BackgroundColor = BaseColor.WHITE;
                    celija.Border = Rectangle.NO_BORDER;
                    celija.MinimumHeight = 20f;
                    tabelaUsluge.AddCell(celija);


                    celija = new PdfPCell(new Phrase(item.DatumPocetka.ToShortDateString(), font11));
                    celija.HorizontalAlignment = Element.ALIGN_CENTER;
                    celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celija.BackgroundColor = BaseColor.WHITE;
                    celija.Border = Rectangle.NO_BORDER;
                    celija.MinimumHeight = 20f;
                    tabelaUsluge.AddCell(celija);

                    celija = new PdfPCell(new Phrase(item.DatumKraja.ToShortDateString(), font11));
                    celija.HorizontalAlignment = Element.ALIGN_CENTER;
                    celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celija.BackgroundColor = BaseColor.WHITE;
                    celija.Border = Rectangle.NO_BORDER;
                    celija.MinimumHeight = 20f;
                    tabelaUsluge.AddCell(celija);

                    celija = new PdfPCell(new Phrase(item.IznosBezPDV.ToString(), font11));
                    celija.HorizontalAlignment = Element.ALIGN_CENTER;
                    celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celija.BackgroundColor = BaseColor.WHITE;
                    celija.Border = Rectangle.NO_BORDER;
                    celija.MinimumHeight = 20f;
                    tabelaUsluge.AddCell(celija);

                    celija = new PdfPCell(new Phrase((item.IznosSaPDV - item.IznosBezPDV).ToString(), font11));
                    celija.HorizontalAlignment = Element.ALIGN_CENTER;
                    celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celija.BackgroundColor = BaseColor.WHITE;
                    celija.Border = Rectangle.NO_BORDER;
                    celija.MinimumHeight = 20f;
                    tabelaUsluge.AddCell(celija);

                    celija = new PdfPCell(new Phrase(item.IznosSaPDV.ToString(), font11));
                    celija.HorizontalAlignment = Element.ALIGN_CENTER;
                    celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                    celija.BackgroundColor = BaseColor.WHITE;
                    celija.Border = Rectangle.NO_BORDER;
                    celija.MinimumHeight = 20f;
                    tabelaUsluge.AddCell(celija);

                    ////////////////////////////////////////////////////////////////////////
                    tabelaUsluge.CompleteRow();
                    ///////////////////////////////////////////////////////////////////////////
                }


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






                celija = new PdfPCell(new Phrase(model.Ukupno.ToString(), font16));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);

                celija = new PdfPCell(new Phrase(model.PDF, font16));
                celija.HorizontalAlignment = Element.ALIGN_RIGHT;
                celija.VerticalAlignment = Element.ALIGN_MIDDLE;
                celija.BackgroundColor = BaseColor.WHITE;
                celija.Border = Rectangle.NO_BORDER;
                tabelaTotal.AddCell(celija);

                celija = new PdfPCell(new Phrase(model.UkupnoPDV, font16Bold));
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

        [HttpGet]
        public ActionResult PromjeniStatus(int id)
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

            r.Placen = !r.Placen;
            db.SaveChanges();



            string status;
            if (r.Placen)
            {
                status = "plaćen";
            }
            else
            {
                status = "nije plaćen";

            }
            TempData["Message"] = "Račun " + r.Sifra + " je označen kao <b>" + status + "</b>" ;
            TempData["code"] = "info";


            return RedirectToAction("Detalji", new { id = id });


        }

    }
}