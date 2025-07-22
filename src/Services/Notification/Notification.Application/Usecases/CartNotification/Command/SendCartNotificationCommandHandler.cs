using Notification.Application.Abstractions;
using Notification.Application.Abstractions.EmailService;
using Notification.Application.Common;
using Notification.Application.Common.EmailTemplateUtils;
using Notification.Application.Usecases.CartNotification.Common;
using Shared.InfrastructureGrpcModels.GetListCatalogProductsByIdModel;
using Shared.MediatR;
using Shared.Utils;

namespace Notification.Application.Usecases.CartNotification.Command;

public class SendCartNotificationCommandHandler : ICommandHandler<SendCartNotificationCommand>
{
    private readonly ISmtpEmailService _emailService;
    private readonly ICustomerSegmentService _customerSegmentService;
    private readonly ICatalogProductService _catalogProductService;

    public SendCartNotificationCommandHandler(ISmtpEmailService emailService,
        ICustomerSegmentService customerSegmentService, ICatalogProductService catalogProductService)
    {
        _emailService = emailService;
        _customerSegmentService = customerSegmentService;
        _catalogProductService = catalogProductService;
    }

    public async Task<Result> Handle(SendCartNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // call grpc (identity) to get user information
            var customerInfo = await _customerSegmentService.GetCustomerSegmentInfoAsync(request.UserId);

            // call grpc to enrich cart information
            var productIds = request.Items.Select(x => x.ProductId).ToList();
            var products = await _catalogProductService.GetListCatalogProductsByIdAsync(
                new GetListCatalogProductsByIdGrpcBaseRequest
                {
                    Ids = productIds
                });

            var enrichProduct = EnrichCatalogProducts(request.Items, products);

            // replace with template engine
            var emailBody = await EmailTemplateUtils.ProcessCartNotificationTemplateAsync(customerInfo, enrichProduct,
                "",
                cancellationToken);

            var emailRequest = new SmtpEmailRequest
            {
                ToEmail = new ToEmail { To = customerInfo.Email },
                Subject = "VMicroservice - Cart Notification",
                Body = emailBody
            };
            await _emailService.SendMailAsync(emailRequest, cancellationToken);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    private List<CatalogProductItem> EnrichCatalogProducts(List<SendCartItemNotificationCommand> items,
        List<GetListCatalogProductsByIdGrpcBaseResponse> products)
    {
        var itemDict = items.ToDictionary(x => x.ProductId);
        return products.Where(x => itemDict.ContainsKey(x.Id))
            .Select(x => new CatalogProductItem
            {
                Id = x.Id,
                Name = x.Name,
                Quantity = itemDict[x.Id].Quantity,
                OriginalPrice = x.OriginalPrice,
                SalePrice = x.SalePrice,
                Thumbnail = x.Thumbnail
            }).ToList();
    }
}