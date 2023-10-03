using OpenWeatherMapSharp.Models;
using OpenWeatherMapSharp.Models.Enums;
using Spectre.Console;

internal partial class Program
{
    // Creates the header for the program
    static void CreateHeader()
    {
        // Create a grid for the header text
        Grid grid = new();
        grid.AddColumn();
        grid.AddRow(new FigletText("OpenWeatherMap").Centered().Color(Color.Red));
        grid.AddRow(Align.Center(new Panel("[red]Sample by Thomas Sebastian Jensen ([link]https://www.tsjdev-apps.de[/])[/]")));

        // Write the grid to the console
        AnsiConsole.Write(grid);
        AnsiConsole.WriteLine();
    }

    // Creates a panel for the location data of the weatherRoot parameter
    // weatherRoot - the weather data to create the panel from
    // geocode - the location information
    static Panel GetLocationPanel(WeatherRoot weatherRoot, GeocodeInfo geocode)
    {
        // Create a list of markup items for the rows of the panel
        List<Markup> markupList = new()
    {
        new Markup($"[red]City: [/][white]{weatherRoot.Name}[/]"),
        new Markup($"[red]Latitude: [/][white]{weatherRoot.Coordinates.Latitude:0.0000}[/]"),
        new Markup($"[red]Longitude: [/][white]{weatherRoot.Coordinates.Longitude:0.0000}[/]"),
        new Markup($"[red]Country: [/][white]{geocode.Country}[/]"),
        new Markup($"[red]State: [/][white]{geocode.State}[/]"),
        new Markup($"[red]Id: [/][white]{weatherRoot.CityId}[/]")
    };

        // Create the rows for the panel and add the markup items to it
        Rows rows = new(markupList);

        // Create the location panel and set its width and header
        Panel panel = new(rows)
        {
            Header = new PanelHeader("Location"),
            Width = 30,
            Border = BoxBorder.Ascii,
            BorderStyle = new Style(Color.Red)
        };

        // Return the location panel
        return panel;
    }

    // Creates a panel for the weather data of the weatherRoot parameter
    // weatherRoot - the weather data to create the panel from
    // unit - the unit of temperature to display the data in
    static Panel GetWeatherPanel(WeatherRoot weatherRoot, Unit unit)
    {
        // Get the main weather data from the weatherRoot
        Main mainWeather = weatherRoot.MainWeather;
        string temperatureString = unit switch
        {
            Unit.Standard => "K",
            Unit.Metric => "C",
            Unit.Imperial => "F",
            _ => ""
        };

        // Create a list of markup items for the rows of the panel
        List<Markup> markupList = new()
    {
        new Markup($"[red]Temperature: [/][white]{mainWeather.Temperature}° {temperatureString}[/]"),
        new Markup($"[red]Temperature (Feels Like): [/][white]{mainWeather.FeelsLikeTemperature}° {temperatureString}[/]"),
        new Markup($"[red]Minimal Temperature: [/][white]{mainWeather.MinTemperature}° {temperatureString}[/]"),
        new Markup($"[red]Maximal Temperature: [/][white]{mainWeather.MaxTemperature}° {temperatureString}[/]"),
        new Markup("[white]-----[/]"),
        new Markup($"[red]Pressure Sea Level hPa: [/][white]{mainWeather.SeaLevelPressure} hPa[/]"),
        new Markup($"[red]Pressure Ground Level hPa: [/][white]{mainWeather.GroundLevelPressure} hPa[/]"),
        new Markup($"[red]Humidity: [/][white]{mainWeather.Humidity} %[/]"),
        new Markup("[white]-----[/]"),
        new Markup($"[red]Sunrise: [/][white]{weatherRoot.System.Sunrise:g}[/]"),
        new Markup($"[red]Sunset: [/][white]{weatherRoot.System.Sunset:g}[/]"),
        new Markup("[white]-----[/]"),
    };
        foreach (var weatherInfo in weatherRoot.WeatherInfos)
        {
            markupList.Add(new Markup($"[red]Current Conditions: [/][white]{weatherInfo.Description}[/]"));
        }

        // Create the rows for the panel and add the markup items to it
        Rows rows = new(markupList);

        // Create the current weather panel and set its width and header
        Panel panel = new(rows)
        {
            Header = new PanelHeader("Current Weather"),
            Width = 120,
            Border = BoxBorder.Ascii,
            BorderStyle = new Style(Color.Red)
        };

        // Return the current weather panel
        return panel;
    }
}