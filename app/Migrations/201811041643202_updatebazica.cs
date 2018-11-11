namespace app.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebazica : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Racunis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KorisnikId = c.Int(nullable: false),
                        Sifra = c.String(),
                        ObracunskiPeriodOD = c.DateTime(nullable: false),
                        ObracunskiPeriodDO = c.DateTime(nullable: false),
                        DatumIzdavanja = c.DateTime(nullable: false),
                        RokPlacanja = c.DateTime(nullable: false),
                        UkupnoSaPDV = c.Double(nullable: false),
                        UkupnoBezPDV = c.Double(nullable: false),
                        Placen = c.Boolean(nullable: false),
                        DatumPlacanja = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Korisniks", t => t.KorisnikId, cascadeDelete: true)
                .Index(t => t.KorisnikId);
            
            CreateTable(
                "dbo.RacuniStavkes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RacunId = c.Int(nullable: false),
                        IznosBezPDV = c.Double(nullable: false),
                        IznosSaPDV = c.Double(nullable: false),
                        DatumPocetka = c.DateTime(nullable: false),
                        DatumKraja = c.DateTime(nullable: false),
                        AktivneUslugeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AktivneUsluges", t => t.AktivneUslugeId)
                .ForeignKey("dbo.Racunis", t => t.RacunId, cascadeDelete: true)
                .Index(t => t.RacunId)
                .Index(t => t.AktivneUslugeId);
            
            CreateTable(
                "dbo.Razlogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Poruka = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Smetnjes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrojSmetnje = c.String(nullable: false),
                        AktivnaUslugaId = c.Int(),
                        DatumUocavanja = c.DateTime(),
                        Opis = c.String(),
                        SmetnjeStatusId = c.Int(nullable: false),
                        KorisnikId = c.Int(),
                        DatumOtvaranja = c.DateTime(nullable: false),
                        DatumZatvaranja = c.DateTime(),
                        UposlenikId = c.Int(),
                        Uposlenici_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AktivneUsluges", t => t.AktivnaUslugaId)
                .ForeignKey("dbo.Korisniks", t => t.KorisnikId)
                .ForeignKey("dbo.SmetnjeStatus", t => t.SmetnjeStatusId, cascadeDelete: true)
                .ForeignKey("dbo.Uposlenicis", t => t.Uposlenici_Id)
                .Index(t => t.AktivnaUslugaId)
                .Index(t => t.SmetnjeStatusId)
                .Index(t => t.KorisnikId)
                .Index(t => t.Uposlenici_Id);
            
            CreateTable(
                "dbo.SmetnjeStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uposlenicis",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Ime = c.String(),
                        Prezime = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        DatumDodavanja = c.DateTime(nullable: false),
                        Aktivan = c.Boolean(nullable: false),
                        UlogaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ulogas", t => t.UlogaId, cascadeDelete: true)
                .Index(t => t.UlogaId);
            
            CreateTable(
                "dbo.Ulogas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NazivUloge = c.String(),
                        Sifra = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SmetnjeOdgovoris",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SmetnjaId = c.Int(nullable: false),
                        Datum = c.DateTime(nullable: false),
                        Poruka = c.String(),
                        KorisnikId = c.Int(),
                        UposlenikId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Korisniks", t => t.KorisnikId)
                .ForeignKey("dbo.Uposlenicis", t => t.UposlenikId)
                .Index(t => t.KorisnikId)
                .Index(t => t.UposlenikId);
            
            CreateTable(
                "dbo.UputstvaKategorijes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ZahtjevPromjenas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KorisnikId = c.Int(),
                        AktivneUslugeId = c.Int(nullable: false),
                        Datum = c.DateTime(nullable: false),
                        ZatvorenZahtjev = c.Boolean(nullable: false),
                        DatumZatvaranja = c.DateTime(),
                        ZahtjevstatusId = c.Int(nullable: false),
                        PaketId = c.Int(),
                        Napomena = c.String(),
                        Razlog = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AktivneUsluges", t => t.AktivneUslugeId, cascadeDelete: true)
                .ForeignKey("dbo.Korisniks", t => t.KorisnikId)
                .ForeignKey("dbo.Pakets", t => t.PaketId)
                .ForeignKey("dbo.ZahtjevStatus", t => t.ZahtjevstatusId, cascadeDelete: true)
                .Index(t => t.KorisnikId)
                .Index(t => t.AktivneUslugeId)
                .Index(t => t.ZahtjevstatusId)
                .Index(t => t.PaketId);
            
            AddColumn("dbo.Korisniks", "Username", c => c.String());
            AddColumn("dbo.Korisniks", "Password", c => c.String());
            AddColumn("dbo.Uputstvas", "DatumDodavanja", c => c.DateTime(nullable: false));
            AddColumn("dbo.Uputstvas", "Putanja", c => c.String());
            AddColumn("dbo.Uputstvas", "UputstvoKategorijaID", c => c.Int(nullable: false));
            AddColumn("dbo.Uputstvas", "UputstvaKategorije_Id", c => c.Int());
            CreateIndex("dbo.Uputstvas", "UputstvaKategorije_Id");
            AddForeignKey("dbo.Uputstvas", "UputstvaKategorije_Id", "dbo.UputstvaKategorijes", "Id");
            DropColumn("dbo.Uputstvas", "Datum");
            DropColumn("dbo.Uputstvas", "Dokument");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Uputstvas", "Dokument", c => c.Binary());
            AddColumn("dbo.Uputstvas", "Datum", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.ZahtjevPromjenas", "ZahtjevstatusId", "dbo.ZahtjevStatus");
            DropForeignKey("dbo.ZahtjevPromjenas", "PaketId", "dbo.Pakets");
            DropForeignKey("dbo.ZahtjevPromjenas", "KorisnikId", "dbo.Korisniks");
            DropForeignKey("dbo.ZahtjevPromjenas", "AktivneUslugeId", "dbo.AktivneUsluges");
            DropForeignKey("dbo.Uputstvas", "UputstvaKategorije_Id", "dbo.UputstvaKategorijes");
            DropForeignKey("dbo.SmetnjeOdgovoris", "UposlenikId", "dbo.Uposlenicis");
            DropForeignKey("dbo.SmetnjeOdgovoris", "KorisnikId", "dbo.Korisniks");
            DropForeignKey("dbo.Smetnjes", "Uposlenici_Id", "dbo.Uposlenicis");
            DropForeignKey("dbo.Uposlenicis", "UlogaId", "dbo.Ulogas");
            DropForeignKey("dbo.Smetnjes", "SmetnjeStatusId", "dbo.SmetnjeStatus");
            DropForeignKey("dbo.Smetnjes", "KorisnikId", "dbo.Korisniks");
            DropForeignKey("dbo.Smetnjes", "AktivnaUslugaId", "dbo.AktivneUsluges");
            DropForeignKey("dbo.RacuniStavkes", "RacunId", "dbo.Racunis");
            DropForeignKey("dbo.RacuniStavkes", "AktivneUslugeId", "dbo.AktivneUsluges");
            DropForeignKey("dbo.Racunis", "KorisnikId", "dbo.Korisniks");
            DropIndex("dbo.ZahtjevPromjenas", new[] { "PaketId" });
            DropIndex("dbo.ZahtjevPromjenas", new[] { "ZahtjevstatusId" });
            DropIndex("dbo.ZahtjevPromjenas", new[] { "AktivneUslugeId" });
            DropIndex("dbo.ZahtjevPromjenas", new[] { "KorisnikId" });
            DropIndex("dbo.Uputstvas", new[] { "UputstvaKategorije_Id" });
            DropIndex("dbo.SmetnjeOdgovoris", new[] { "UposlenikId" });
            DropIndex("dbo.SmetnjeOdgovoris", new[] { "KorisnikId" });
            DropIndex("dbo.Uposlenicis", new[] { "UlogaId" });
            DropIndex("dbo.Smetnjes", new[] { "Uposlenici_Id" });
            DropIndex("dbo.Smetnjes", new[] { "KorisnikId" });
            DropIndex("dbo.Smetnjes", new[] { "SmetnjeStatusId" });
            DropIndex("dbo.Smetnjes", new[] { "AktivnaUslugaId" });
            DropIndex("dbo.RacuniStavkes", new[] { "AktivneUslugeId" });
            DropIndex("dbo.RacuniStavkes", new[] { "RacunId" });
            DropIndex("dbo.Racunis", new[] { "KorisnikId" });
            DropColumn("dbo.Uputstvas", "UputstvaKategorije_Id");
            DropColumn("dbo.Uputstvas", "UputstvoKategorijaID");
            DropColumn("dbo.Uputstvas", "Putanja");
            DropColumn("dbo.Uputstvas", "DatumDodavanja");
            DropColumn("dbo.Korisniks", "Password");
            DropColumn("dbo.Korisniks", "Username");
            DropTable("dbo.ZahtjevPromjenas");
            DropTable("dbo.UputstvaKategorijes");
            DropTable("dbo.SmetnjeOdgovoris");
            DropTable("dbo.Ulogas");
            DropTable("dbo.Uposlenicis");
            DropTable("dbo.SmetnjeStatus");
            DropTable("dbo.Smetnjes");
            DropTable("dbo.Razlogs");
            DropTable("dbo.RacuniStavkes");
            DropTable("dbo.Racunis");
        }
    }
}
