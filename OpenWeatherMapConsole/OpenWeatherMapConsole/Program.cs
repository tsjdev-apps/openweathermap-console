using OpenWeatherMapSharp;
using OpenWeatherMapSharp.Models;
using OpenWeatherMapSharp.Models.Enums;
using Spectre.Console;

const string openWeatherMapApiKey = "OWMAPIKEY";

// Starts the program
await StartAsync();

// Get the weather for a given city
// cityName - the name of the city to retrieve the weather for
// languageCode - the language to retrieve the weather information in
// unit - the unit of temperature to use for the retrieved data
async Task<(WeatherRoot? Weather, GeocodeInfo? Geocode)> GetWeatherAsync(string cityName, LanguageCode languageCode, Unit unit)
{
    WeatherRoot? weatherRoot = null;
    GeocodeInfo? geocodeInfo = null;

    await AnsiConsole.Status()
        .StartAsync("Getting the current weather...", async ctx =>
        {
            // Create an instance of the OpenWeatherMapService
            OpenWeatherMapService openWeatherMapService = new(openWeatherMapApiKey);

            // Get the location information from the API
            OpenWeatherMapServiceResponse<List<GeocodeInfo>> geocodeResponse = await openWeatherMapService.GetLocationByNameAsync(cityName);

            if (!geocodeResponse.IsSuccess || (geocodeResponse.Response is not null && geocodeResponse.Response.Count == 0))
            {
                weatherRoot = null;
                return;
            }

            // Get the first geocodeinfo object
            GeocodeInfo? info = geocodeResponse?.Response?.FirstOrDefault();

            if (info is null)
            {
                weatherRoot = null;
                return;
            }

            geocodeInfo = info;

            // Get the weather data for the city, with the specified language and units
            OpenWeatherMapServiceResponse<WeatherRoot> response = await openWeatherMapService.GetWeatherAsync(info.Latitude, info.Longitude, languageCode, unit);

            // If the request was successful, return the response, otherwise return null
            weatherRoot = response.IsSuccess ? response.Response : null;
        });

    return (weatherRoot, geocodeInfo);
}

// Displays the current weather in the console
// weatherRoot - the weather data to display
// geocode - the location information
// unit - the unit of temperature to display the data in
static void ShowCurrentWeather(WeatherRoot weatherRoot, GeocodeInfo geocode, Unit unit)
{
    // Get the panels for the location and weather data
    Panel locationPanel = GetLocationPanel(weatherRoot, geocode);
    Panel weatherPanel = GetWeatherPanel(weatherRoot, unit);

    // Create a grid to display the location and weather panels
    Grid grid = new();
    grid.AddColumn();
    grid.AddColumn();
    grid.AddRow(locationPanel, weatherPanel);

    // Write the grid to the console
    AnsiConsole.Write(grid);
}

// Starts the program and retrieves weather data
async Task StartAsync()
{
    // Clear the console and create the program header
    AnsiConsole.Clear();
    CreateHeader();

    // Ask the user for a city name, language code, and unit
    string cityName = GetCityName();
    LanguageCode languageCode = GetLanguageCode();
    Unit unit = GetUnit();

    // Get the weather data for the given city and set it to weatherRoot
    (WeatherRoot? Weather, GeocodeInfo? Geocode) = await GetWeatherAsync(cityName, languageCode, unit);

    // If the weather data couldn't be retrieved, inform the user and exit.
    // Otherwise, display the weather data and ask the user if they want to retrieve more data for another city.
    if (Weather is null || Geocode is null)
    {
        AnsiConsole.Write("[white]Unfortunately I can't recognize the city. Please try again.[/]");
        return;
    }
    else
    {
        ShowCurrentWeather(Weather, Geocode, unit);
    }

    await AskForRetryAsync();
}

// Asks the user if they want to retrieve more data for another city
async Task AskForRetryAsync()
{
    AnsiConsole.WriteLine();

    if (AnsiConsole.Confirm("[white]Do you want to see the [/][red]weather[/] [white]for another city?[/]"))
    {
        await StartAsync();
    }
}