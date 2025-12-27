using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace PhoneBook
{
    public class PhoneBookController
    {
        public static async Task startApp()
        {
            DatabaseManager.InjectDataToDatabase().GetAwaiter().GetResult(); // Database setup create if not exists and injecting data
            GetUserInput getUserInput = new();

            await getUserInput.MainMenu();
        }

        public async Task Get() // Get the data from database and send it to the tablevisualization
        {
            List<PhoneBookTable> tableData = new List<PhoneBookTable>();

            using (var context = new PhoneBookContext())
            {
                tableData = await context.PhoneBooks.ToListAsync();
            }

            TableVisualization.DisplayTable(tableData);

        }
        public async Task Post() // Posting the data to the database
        {
            GetUserInput getUserInput = new();
            var name = getUserInput.GetNameInput();  // Task variable
            var email = getUserInput.GetEmailInput();
            var phoneNumber = getUserInput.GetPhoneNumberInput();

            using (var context = new PhoneBookContext())
            {
                var newDbRegistry = new PhoneBookTable
                {
                    Name = name.Result,              // Task.Result
                    Email = email.Result,
                    PhoneNumber = phoneNumber.Result
                };

                context.PhoneBooks.Add(newDbRegistry);
                await context.SaveChangesAsync();

                Console.WriteLine("The data successfully stored!");
            }
        }
        public void Delete(int id) // Deleting data from database
        {
            using (var context = new PhoneBookContext())
            {
                var record = context.PhoneBooks.Find(id);

                if (record != null)
                {
                    context.PhoneBooks.Remove(record);
                    context.SaveChanges();
                }
            }
        }
        public PhoneBookTable? GetById(int id) // Get the row of data with id from database
        {
            using (var context = new PhoneBookContext())
            {
                var record = context.PhoneBooks.Find(id);
                return record;
            }
        }

        public async Task Update(PhoneBookTable phoneBookTable) // Update the data in database
        {
            using (var context = new PhoneBookContext())
            {
                var updateDbRegistry = new PhoneBookTable
                {
                    Id = phoneBookTable.Id,
                    Name = phoneBookTable.Name,
                    Email = phoneBookTable.Email,
                    PhoneNumber = phoneBookTable.PhoneNumber
                };

                context.PhoneBooks.Update(updateDbRegistry);
                await context.SaveChangesAsync();
            }
        }
    }
}