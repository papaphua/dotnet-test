namespace TestTask.Services;

public sealed record PopularItemDto(
    int Year,
    string ItemName,
    int TimesBoughtInMostPopularDay);