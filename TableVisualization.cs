using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spectre.Console;

namespace PhoneBook
{
    public class TableVisualization
    {
        public static void DisplayTable(List<PhoneBookTable> tableData)
        {
            // Create Table 
            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.Title("[yellow]Phone Book[/]");

            // Defining the columns
            table.AddColumn("[bold blue]ID[/]");
            table.AddColumn("[bold green]Name[/]");
            table.AddColumn("[bold magenta]Email[/]");
            table.AddColumn("[bold yellow]Phone Number[/]");

            // Add data to the rows
            foreach (var t in tableData)
            {
                table.AddRow(
                    t.Id.ToString(),
                    t.Name ?? "",
                    t.Email ?? "",
                    t.PhoneNumber ?? ""
                );
            }
            AnsiConsole.Write(table);
        }
    }
}