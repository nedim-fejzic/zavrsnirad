using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace app.Models
{
    public class MojKontekst:DbContext
    {
        public DbSet<FAQ> FAQDbSet { get; set; }
        public DbSet<FAQKategorija> FAQKategorijaDbSet { get; set; }
        public DbSet<Vijesti> VijestiDbSet { get; set; }
        public DbSet<VijestiKategorija> VijestiKategorijaDbSet { get; set; }
        public DbSet<Jezik> JezikDbSet { get; set; }
        public DbSet<Kanal> KanalDbSet { get; set; }
        public DbSet<KanaliPaketi> KanaliPaketiDbSet { get; set; }
        public DbSet<Kvalitet> KvalitetDbSet { get; set; }
        public DbSet<Paket> PaketDbSet { get; set; }
        public DbSet<TipKontakta> TipKontaktaDbSet { get; set; }
        public DbSet<TipUsluga> TipUslugaDbSet { get; set; }
        public DbSet<Uputstva> UputstvaDbSet { get; set; }
        public DbSet<Zahtjev> ZahtjevDbSet { get; set; }
        public DbSet<Zanr> ZanrDbSet { get; set; }
        public DbSet<Opcine> OpcinaDbSet { get; set; }
        public DbSet<Korisnik> KorisnikDbSet { get; set; }
        public DbSet<AktivneUsluge> AktivneUslugeDbSet { get; set; }
        public DbSet<AktivneUslugeStatus> AktivneUslugeStatusDbSet { get; set; }
        public DbSet<ZahtjevUsluge> ZahtjevUslugeDbSet { get; set; }
        public DbSet<ZahtjevStatus> ZahtjevStatusDbSet { get; set; }
        public DbSet<Uredjaj> UredjajDbSet { get; set; }
        public DbSet<UredjajZaduzenje> UredjajZaduzenjeDbSet { get; set; }
        public DbSet<UputstvaKategorije> UputstvaKategorijeDbSet { get; set; }
        public DbSet<Smetnje> SmetnjeDbSet { get; set; }
        public DbSet<SmetnjeStatus> SmetnjeStatusDbSet { get; set; }
        public DbSet<Uposlenici> UposlenikDbSet { get; set; }
        public DbSet<SmetnjeOdgovori> SmetnjeOdgovori { get; set; }
        public DbSet<Razlog> RazloziDbSet { get; set; }
        public DbSet<ZahtjevPromjena> ZahtjevPromjenaDbSet { get; set; }
        public DbSet<Racuni> RacuniDbSet { get; set; }
        public DbSet<RacuniStavke> RacuniStavkeDbSet { get; set; }
        public DbSet<Uloga> UlogaDbSet { get; set; }



        public MojKontekst() : base("name=MojConnectionString")
        {
        }


    }
}