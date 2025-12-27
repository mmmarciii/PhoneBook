using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace PhoneBook
{
    public class GetUserInput
    {
        PhoneBookController phoneBookController = new();
        public async Task MainMenu() // Main menu logic
        {
            Console.Clear();
            bool closeApp = false;
            while (!closeApp)
            {
                // Spectre Console Menu Listing
                var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]\n\nMain Menu[/]")
                    .PageSize(10)
                    .MoreChoicesText("[gray](Move up and down)[/]")
                    .AddChoices(new[] {
                        "Close Application",
                        "View records",
                        "Add new",
                        "Delete record",
                        "Update records"
                    }));

                switch (menu)
                {
                    case "Close Application":
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "View records":
                        await phoneBookController.Get();
                        break;
                    case "Add new":
                        await phoneBookController.Post();
                        break;
                    case "Delete record":
                        await DeleteById();
                        break;
                    case "Update records":
                        await UpdateById();
                        break;
                    default:
                        AnsiConsole.MarkupLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }

        public async Task UpdateById() // Get the id from input to update the row
        {
            await phoneBookController.Get();
            var updateId = AnsiConsole.Prompt(new TextPrompt<string>("Please add an id of the registry you want to Update:"));

            if (!Int32.TryParse(updateId, out int id) || id == 0) return; // return to main menu if id not exist or 0

            var phoneBook = phoneBookController.GetById(id);
            if (phoneBook == null)
            {
                AnsiConsole.MarkupLine("[red]Record not found![/]");
                await Task.Delay(1500);
                return;
            }

            // Update Menu. You can choose datafield to update data one by one 
            bool updating = true;
            while (updating)
            {
                // Spectre Update Menu listing
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]What to Update?[/]")
                    .PageSize(10)
                    .MoreChoicesText("[gray](Move up and down)[/]")
                    .AddChoices(new[] {
                        "Name",
                        "Email",
                        "Phone number",
                        "Save",
                        "Exit"
                    }));


                if (choice == "Exit") return;
                if (choice == "Save")
                {
                    await phoneBookController.Update(phoneBook);
                    updating = false;
                    continue;
                }
                if (choice == "Name")
                {
                    var newName = await GetNameInput();
                    if (newName != null) phoneBook.Name = newName;
                }
                if (choice == "Email")
                {
                    var newEmail = await GetEmailInput();
                    if (newEmail != null) phoneBook.Email = newEmail;
                }
                if (choice == "Phone number")
                {
                    var newPhoneNumber = await GetPhoneNumberInput();
                    if (newPhoneNumber != null) phoneBook.PhoneNumber = newPhoneNumber;
                }

            }
        }
        public async Task DeleteById() // Get the id from input to delete it from tha database
        {
            await phoneBookController.Get();
            var commandInput = AnsiConsole.Prompt(new TextPrompt<string>("Please add an id of the registry you want to delete:"));

            if (!Int32.TryParse(commandInput, out int id) || id == 0) return;

            var phoneBook = phoneBookController.GetById(id); // Get the id specified row from the db 
            if (phoneBook == null || phoneBook.Id == 0) // If the row doesn't exist get a new id 
            {
                AnsiConsole.MarkupLine($"[red]Record with id {id} doesn't exist![/]");
                await Task.Delay(2000); // Wait a bit then go back to Main Menu
                return;
            }

            phoneBookController.Delete(id); // Delete with id
            AnsiConsole.MarkupLine("[lime]You have successfully deleted![/]");
            await Task.Delay(1500);
        }

        public async Task<string?> GetNameInput() // Get name from input
        {
            var name = AnsiConsole.Prompt(new TextPrompt<string>("[bold]Please insert a Name:[/]"));

            if (name == "0") await MainMenu();

            while (String.IsNullOrWhiteSpace(name)) // Validation 
            {
                name = AnsiConsole.Prompt(new TextPrompt<string>("[red][bold]Not valid Name. Please insert a correct one:[/][/]"));
            }

            return name;
        }
        public async Task<string?> GetEmailInput() // Get the email from input 
        {
            var email = AnsiConsole.Prompt(new TextPrompt<string>("[bold]Please insert a Email:[/]"));

            if (email == "0") await MainMenu();

            var emailValidator = new System.ComponentModel.DataAnnotations.EmailAddressAttribute(); // Email validating
            while (!emailValidator.IsValid(email))
            {
                email = AnsiConsole.Prompt(new TextPrompt<string>("[red][bold]Not valid Email. Please insert a correct one:[/][/]"));
            }

            return email;
        }

        public async Task<string?> GetPhoneNumberInput() // Get the phone number from input 
        {
            var phoneNumber = AnsiConsole.Prompt(new TextPrompt<string>("[bold]Please insert a Phonenumber:[/]"));

            if (phoneNumber == "0") await MainMenu();

            while (!PhoneNumberValidation(phoneNumber))
            {
                phoneNumber = AnsiConsole.Prompt(new TextPrompt<string>("[red][bold]Not valid Phonenumber. Please insert a correct one:[/][/]"));
            }

            return phoneNumber;
        }

        internal bool PhoneNumberValidation(string? phoneNumber) // Validating the phone number; libphonenumber-csharp nuget package 
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return false;

            var phoneUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

            try
            {
                var numberProto = phoneUtil.Parse(phoneNumber, "AT");

                return phoneUtil.IsValidNumber(numberProto);
            }
            catch (PhoneNumbers.NumberParseException)
            {
                return false;
            }
        }


    }
}