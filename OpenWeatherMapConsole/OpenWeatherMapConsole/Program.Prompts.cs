using OpenWeatherMapSharp.Models.Enums;
using Spectre.Console;

internal partial class Program
{
    // Asks the user for a city name and returns it
    static string GetCityName()
    {
        // ask the user for a city name
        string cityName = AnsiConsole.Ask<string>("[white]Please type a [/][red]city[/][white].[/]");

        // Create the header
        AnsiConsole.Clear();
        CreateHeader();

        return cityName;
    }

    // Asks the user for a language code and returns it
    static LanguageCode GetLanguageCode()
        => AnsiConsole.Prompt(new SelectionPrompt<LanguageCode>()
            .Title("[white]Please select the [/][red]language[/][white].[/]")
            .PageSize(10)
            .MoreChoicesText("[white](Move up and down to reveal more languages)[/]")
            .AddChoices(Enum.GetValues(typeof(LanguageCode)).Cast<LanguageCode>().ToList()));

    // Asks the user for a unit and returns it
    static Unit GetUnit()
        => AnsiConsole.Prompt(new SelectionPrompt<Unit>()
            .Title("[white]Please select the [/][red]unit[/][white].[/]")
            .PageSize(10)
            .MoreChoicesText("[white](Move up and down to reveal more units)[/]")
            .AddChoices(Enum.GetValues(typeof(Unit)).Cast<Unit>().ToList()));
}
