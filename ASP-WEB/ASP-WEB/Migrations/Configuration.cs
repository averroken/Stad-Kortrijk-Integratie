namespace ASP_WEB.Migrations
{
    using DAL.Context;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IntegratieContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IntegratieContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //ApplicationDbContext appContext = ApplicationDbContext.Create();
            //appContext.Roles.AddOrUpdate(new IdentityRole { Name = "admin" });
            AddData(context);
        }

        private void AddData(IntegratieContext context)
        {
            if (context.Theme.Count() == 0)
            {
                //Themas
                using (StreamReader sr = new StreamReader(@"C:\NMCT\Stad-Kortrijk-Integratie\ASP-WEB\ASP-WEB\CSVfiles\ThemaCSV1.txt"))
                {
                    string line;
                    sr.ReadLine();
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] linesplit = line.Split(';');
                        context.Theme.AddOrUpdate<Theme>(o => o.Name, new Theme() { Name = linesplit[1], FotoURL = linesplit[2] });
                    }
                    context.SaveChanges();
                }             
            }

            if (context.Office.Count() == 0)
            {
                //Themas
                using (StreamReader sr = new StreamReader(@"C:\NMCT\Stad-Kortrijk-Integratie\ASP-WEB\ASP-WEB\CSVfiles\DienstCSV1.txt"))
                {
                    string line;
                    sr.ReadLine();
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] linesplit = line.Split(';');
                        context.Office.AddOrUpdate<Office>(o => o.Name, new Office() {
                            Name = linesplit[1],
                            URL = linesplit[2],
                            EmailAddress = linesplit[3],
                            Street = linesplit[4],
                            HouseNumber = linesplit[5],
                            ZipCode = linesplit[6],
                            City = linesplit[7],
                            PhoneNumber = linesplit[8],
                            OpeningHours = linesplit[9]
                        });
                    }
                    context.SaveChanges();
                }
            }

            /*if (context.Theme.Count() == 0)
            {
                //Themas
                using (StreamReader sr = new StreamReader(@"C:\NMCT\Stad-Kortrijk-Integratie\ASP-WEB\ASP-WEB\CSVfiles\DienstSubthemaCSV1.txt"))
                {
                    string line;
                    sr.ReadLine();
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] linesplit = line.Split(';');
                        
                    }
                    context.SaveChanges();
                }
            }

            if (context.Subtheme.Count() == 0)
            {
                //Themas
                using (StreamReader sr = new StreamReader(@"C:\NMCT\Stad-Kortrijk-Integratie\ASP-WEB\ASP-WEB\CSVfiles\SubthemaCSV1.txt"))
                {
                    string line;
                    sr.ReadLine();
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] linesplit = line.Split(';');
                        context.Subtheme.AddOrUpdate<Subtheme>(o => o.SubthemeID, new Subtheme() {
                            ThemeID = Convert.ToInt32(linesplit[1]),
                            Name = linesplit[2],
                            Description = linesplit[3],
                            SubthemeID = Convert.ToInt32(linesplit[4]),
                            FotoURL = linesplit[5]
                        });
                    }
                    context.SaveChanges();
                }
            }*/
        }
    }
}
