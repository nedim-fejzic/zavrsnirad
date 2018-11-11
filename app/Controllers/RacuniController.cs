using app.Areas.admin.ViewModels;
using app.Models;
using app.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace app.Controllers
{
    [AuthKorisnik]
    public class RacuniController : Controller
    {

        private MojKontekst db = new MojKontekst();

        // GET: Racuni
        public ActionResult Index()
        {

            int korisnikid = (int)Session["logiran_korisnik"];
            Korisnik k = db.KorisnikDbSet.Find(korisnikid);

            if (k == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }



            RacuniIndexVM model = new RacuniIndexVM();
            model.ListaRacuna = db.RacuniDbSet.Where(c => c.KorisnikId == korisnikid).ToList();


            return View(model);
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


            RacuniDetaljiVM model = db.RacuniDbSet
              .Where(x => x.Id == id)
              .Select(f => new RacuniDetaljiVM
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






                celija = new PdfPCell(new Phrase(model.Korisnik.Opcina.NazivOpcine, font11));
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
    }
}