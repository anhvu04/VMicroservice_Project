namespace Notification.Application.Common.EmailTemplateUtils;

public class EmailTemplateConstant
{
    private const string EmailTemplateFolderPath = "Common/EmailTemplates";
    private const string CartNotificationTemplate = "cart_notification_template.html";

    private static string BasePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

    private static string GetFullTemplatePath(string templateName)
    {
        return Path.Combine(BasePath, EmailTemplateFolderPath, templateName);
    }

    public static string GetCartNotificationTemplatePath()
    {
        return GetFullTemplatePath(CartNotificationTemplate);
    }
}