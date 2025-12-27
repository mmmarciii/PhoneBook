using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook
{
    public class DatabaseManager
    {
        // Create the database if it is not created already
        // Upload the database with some starting data
        public static async Task InjectDataToDatabase()
        {
            using (var context = new PhoneBookContext())
            {
                bool isDbcreate = await context.Database.EnsureCreatedAsync();

                if (isDbcreate)
                {
                    Console.WriteLine("Injecting data to database...");

                    var newDbContatcts = new List<PhoneBookTable>
                    {
                        new PhoneBookTable { Name = "John Smith", Email = "john.smith88@gmail.com", PhoneNumber = "+12025550143" },
                        new PhoneBookTable { Name = "Emily Johnson", Email = "emily.j@outlook.com", PhoneNumber = "+442079460128" },
                        new PhoneBookTable { Name = "Michael Brown", Email = "brown.mike@icloud.com", PhoneNumber = "+13125550987" },
                        new PhoneBookTable { Name = "Sarah Williams", Email = "sarah.w.92@yahoo.com", PhoneNumber = "+14155552468" },
                        new PhoneBookTable { Name = "James Taylor", Email = "jtaylor.work@protonmail.com", PhoneNumber = "+441614960357" },
                        new PhoneBookTable { Name = "Olivia Martinez", Email = "olivia.mtz@gmail.com", PhoneNumber = "+15125550199" },
                        new PhoneBookTable { Name = "William Davis", Email = "will.davis@freemail.com", PhoneNumber = "+16175550102" },
                        new PhoneBookTable { Name = "Sophia Wilson", Email = "sophia.wilson@test.com", PhoneNumber = "+441134960812" },
                        new PhoneBookTable { Name = "David Miller", Email = "d.miller85@hotmail.com", PhoneNumber = "+17025550155" },
                        new PhoneBookTable { Name = "Isabella Garcia", Email = "isabella.g@gmail.com", PhoneNumber = "+18135550176" }
                    };

                    await context.PhoneBooks.AddRangeAsync(newDbContatcts);
                    int savedRows = await context.SaveChangesAsync();

                    Console.WriteLine($"Saved rows: {savedRows}.");
                }
            }
        }
    }
}