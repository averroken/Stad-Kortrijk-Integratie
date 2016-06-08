namespace ASP_WEB.Migrations
{
    using DAL.Context;
    using Helpers;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    internal sealed class Configuration : DbMigrationsConfiguration<ASP_WEB.DAL.Context.IntegratieContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ASP_WEB.DAL.Context.IntegratieContext context)
        {
            //if (!Debugger.IsAttached) Debugger.Launch();
            AddThemes(context);
            AddOffices(context);
            AddSubthemes(context);
            AddOfficeSubthemes(context);
        }

        private void AddThemes(IntegratieContext context)
        {
            using (StreamReader sr = new StreamReader(@"C:\NMCT 2015-2016\Project\Stad Kortrijk - Integratie\Stad-Kortrijk-Integratie\ASP-WEB\ASP-WEB\CSVfiles\ThemaCSV1.txt"))
            {
                string line;
                sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] linesplit = line.Split(';');
                    context.Theme.AddOrUpdate<Theme>(t => t.Name, new Theme() { Name = linesplit[1], FotoURL = linesplit[2] });
                }
                context.SaveChanges();
            }
        }
        private void AddOffices(IntegratieContext context)
        {
            using (StreamReader sr = new StreamReader(@"C:\NMCT 2015-2016\Project\Stad Kortrijk - Integratie\Stad-Kortrijk-Integratie\ASP-WEB\ASP-WEB\CSVfiles\DienstCSV1.txt"))
            {
                string line;
                sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] linesplit = line.Split(';');
                    int zip = 0;
                    Int32.TryParse(linesplit[6], out zip);
                    context.Office.AddOrUpdate<Office>(o => o.Name, new Office()
                    {
                        Name = linesplit[1],
                        URL = linesplit[2],
                        EmailAddress = linesplit[3],
                        Street = linesplit[4],
                        HouseNumber = linesplit[5],
                        ZipCode = zip,
                        City = linesplit[7],
                        PhoneNumber = linesplit[8],
                        Monday = linesplit[9],
                        Tuesday = linesplit[10],
                        Wednesday = linesplit[11],
                        Thursday = linesplit[12],
                        Friday = linesplit[13],
                        Saturday = linesplit[14],
                        Sunday = linesplit[15],
                    });
                }
                context.SaveChanges();
            }
        }
        private void AddSubthemes(IntegratieContext context)
        {
            Encoding targetEncoding = Encoding.GetEncoding(1252);
            using (StreamReader sr = new StreamReader(@"C:\NMCT 2015-2016\Project\Stad Kortrijk - Integratie\Stad-Kortrijk-Integratie\ASP-WEB\ASP-WEB\CSVfiles\SubthemaCSV1.txt", targetEncoding))
            {
                string line;
                sr.ReadLine();

                ICollection<int> col = new List<int>();
                string[] splitofficeid;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] linesplit = line.Split(';');
                    if (!String.IsNullOrWhiteSpace(linesplit[4]))
                    {
                        splitofficeid = linesplit[4].Split('-');
                    }
                    else
                    {
                        splitofficeid = new string[0];
                    }

                    foreach (string id in splitofficeid)
                    {
                        col.Add(Convert.ToInt32(id));
                    }
                    context.Subtheme.AddOrUpdate<Subtheme>(o => o.Name, new Subtheme()
                    {
                        ThemeID = Convert.ToInt32(linesplit[1]),
                        Name = WebUtility.HtmlEncode(linesplit[2]),
                        Description = WebUtility.HtmlEncode(linesplit[3]),
                        OfficeID = col,
                        FotoURL = linesplit[5]
                    });
                }
                context.SaveChanges();
            }
        }
        private void AddOfficeSubthemes(IntegratieContext context)
        {
            using (StreamReader sr = new StreamReader(@"C:\NMCT 2015-2016\Project\Stad Kortrijk - Integratie\Stad-Kortrijk-Integratie\ASP-WEB\ASP-WEB\CSVfiles\DienstSubthemaCSV1.txt"))
            {
                string line;
                string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["IntegratieContext"].ConnectionString;
                sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] linesplit = line.Split(';');
                    using (SqlConnection connection = new SqlConnection(CONNECTIONSTRING))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand())
                        {
                            command.Connection = connection;
                            string sql = "INSERT INTO OfficeSubthemes VALUES (@Office_OfficeID, @Subtheme_SubthemeID);select @@IDENTITY";
                            command.CommandText = sql;
                            command.Parameters.AddWithValue("@Office_OfficeID", Convert.ToInt32(linesplit[1]));
                            command.Parameters.AddWithValue("@Subtheme_SubthemeID", Convert.ToInt32(linesplit[2]));
                            object result = command.ExecuteScalar();
                            //return int.Parse(result.ToString());
                        }
                    }
                }
            }
        }
    }
}