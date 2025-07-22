namespace Shared.Utils.Errors;

public static class GrpcCalledError
{
    public static class BasketClientError
    {
        public const string CartNotificationScheduleError = "Error while sending cart notification schedule";
        public const string GetListCatalogProductsError = "Error while getting list of products";
    }

    public static class NotificationClientError
    {
        public const string GetCustomerInfoError = "Error while getting customer info";
    }
}