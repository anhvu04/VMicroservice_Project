using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Notification.Application.Usecases.CartNotification.Common;
using Shared.InfrastructureGrpcModels.CustomerSegmentInfo;
using Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;

namespace Notification.Application.Common.EmailTemplateUtils;

public class EmailTemplateUtils
{
    /// <summary>
    /// Gets email template
    /// </summary>
    private static async Task<string> GetEmailTemplateAsync(string templatePath,
        CancellationToken cancellationToken = default)
    {
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Template not found at: {templatePath}");
        }

        return await File.ReadAllTextAsync(templatePath, cancellationToken);
    }

    /// <summary>
    /// Processes cart notification template with customer and product data
    /// </summary>
    public static async Task<string> ProcessCartNotificationTemplateAsync(
        GetCustomerSegmentInfoGrpcBaseResponse customerInfo,
        List<CatalogProductItem> products, string checkoutUrl,
        CancellationToken cancellationToken = default)
    {
        var template =
            await GetEmailTemplateAsync(EmailTemplateConstant.GetCartNotificationTemplatePath(), cancellationToken);

        // Generate product list HTML
        var productListHtml = GenerateProductListHtml(products);

        // Replace placeholders in the template
        template = template.Replace("{{CustomerName}}", customerInfo.FirstName + " " + customerInfo.LastName)
            .Replace("{{CartTotal}}",
                products.Sum(x => x.SalePrice != 0 ? x.SalePrice * x.Quantity : x.OriginalPrice * x.Quantity)
                    .ToString())
            .Replace("{{CartItems}}", productListHtml);
        return template;
    }

    private static string GenerateProductListHtml(List<CatalogProductItem> products)
    {
        var productListHtml = new StringBuilder();

        foreach (var product in products)
        {
            var effectivePrice = product.SalePrice != 0 ? product.SalePrice : product.OriginalPrice;
            var subtotal = effectivePrice * product.Quantity;

            var priceHtml = product.SalePrice != 0
                ? $@"<span class=""original-price"">{product.OriginalPrice}</span>
                     <span class=""sale-price"">{product.SalePrice}</span>"
                : $@"<span class=""regular-price"">{product.OriginalPrice}</span>";

            productListHtml.AppendLine($@"
                <div class=""cart-item"">
                    <img class=""item-image"" src=""{product.Thumbnail}"" alt=""Product Image"">
                    <div class=""item-details"">
                        <div class=""item-name"">{product.Name}</div>
                        <div class=""item-price"">
                            {priceHtml}
                        </div>
                        <div class=""item-quantity"">Quantity: {product.Quantity}</div>
                    </div>
                    <div class=""item-subtotal"">
                         {subtotal}
                    </div>
                </div>");
        }

        return productListHtml.ToString();
    }
}