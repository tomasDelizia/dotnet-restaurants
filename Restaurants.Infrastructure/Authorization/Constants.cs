namespace Restaurants.Infrastructure.Authorization;

public static class PolicyNames
{
    public const string HasNationality = "HasNationality";
    public const string IsArgentinian = "IsArgentinian";
    public const string IsAdult = "IsAdult";
    public const string HasRestaurants = "HasRestaurants";
}

public static class AppClaimTypes
{
    public const string Nationality = "Nationality";
    public const string DateOfBirth = "DateOfBirth";
}