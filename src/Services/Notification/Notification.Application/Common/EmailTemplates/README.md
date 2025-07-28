# Cart Notification Email Template

## Overview
This directory contains the email template for cart abandonment notifications. The template is designed to be beautiful, responsive, and engaging to encourage users to complete their purchases.

## Files
- `cart_notification_template.html` - Main template with placeholders for dynamic content
- `cart_notification_sample.html` - Sample template with example data for preview
- `README.md` - This documentation file

## Template Features

### 🎨 Design Features
- **Colorful Gradient Backgrounds**: Modern gradient designs throughout the template
- **Responsive Design**: Optimized for both desktop and mobile devices
- **Professional Typography**: Clean, readable fonts with proper hierarchy
- **Card-based Layout**: Modern card design for cart items
- **Hover Effects**: Interactive elements with smooth transitions
- **Emoji Integration**: Friendly emojis to make the email more engaging

### 📱 Responsive Design
- Mobile-first approach
- Flexible layouts that adapt to different screen sizes
- Optimized button sizes for touch interfaces
- Readable text on all devices

### 🎯 Conversion Optimization
- Clear call-to-action button with gradient background
- Urgency messaging to encourage immediate action
- Professional footer with contact information
- Social media links for brand engagement

## Template Variables

The main template (`cart_notification_template.html`) uses the following placeholders that should be replaced with actual data:

### Customer Information
- `{{CustomerName}}` - Customer's name for personalization

### Cart Data
- `{{CartItems}}` - Dynamic section containing all cart items
- `{{TotalAmount}}` - Total cart value (formatted as currency)

### URLs
- `{{CheckoutUrl}}` - Link to complete the purchase
- `{{UnsubscribeUrl}}` - Link to unsubscribe from emails

### Cart Item Structure
Each cart item in the `{{CartItems}}` section should follow this structure:

```html
<div class="cart-item">
    <img src="{{ProductThumbnail}}" alt="{{ProductName}}" class="item-image">
    <div class="item-details">
        <div class="item-name">{{ProductName}}</div>
        <div class="item-price">
            <span class="original-price">${{OriginalPrice}}</span>
            <span class="sale-price">${{SalePrice}}</span>
        </div>
        <div class="item-quantity">Quantity: {{Quantity}}</div>
    </div>
    <div class="item-subtotal">
        ${{ItemSubtotal}}
    </div>
</div>
```

### Cart Item Variables
- `{{ProductThumbnail}}` - Product image URL
- `{{ProductName}}` - Product name
- `{{OriginalPrice}}` - Original price (will show with strikethrough if different from sale price)
- `{{SalePrice}}` - Current/sale price
- `{{Quantity}}` - Quantity of the item in cart
- `{{ItemSubtotal}}` - Subtotal for this item (SalePrice × Quantity)

## Implementation Guide

### 1. Data Mapping
Based on the existing codebase models, here's how to map the data:

```csharp
// From SendCartNotificationScheduleRequest
var customerName = await GetCustomerName(request.UserId);
var cartItems = await BuildCartItemsHtml(request.Items);
var totalAmount = await CalculateTotalAmount(request.Items);

// URLs
var checkoutUrl = $"{baseUrl}/checkout?userId={request.UserId}";
var unsubscribeUrl = $"{baseUrl}/unsubscribe?userId={request.UserId}";
```

### 2. Cart Items Generation
For each item in `request.Items`, fetch product details and generate HTML:

```csharp
foreach (var item in request.Items)
{
    var product = await productService.GetProductById(item.ProductId);
    var itemSubtotal = product.SalePrice * item.Quantity;
    
    // Generate cart item HTML using the structure above
}
```

### 3. Template Processing
Replace all placeholders with actual data before sending the email:

```csharp
var emailContent = template
    .Replace("{{CustomerName}}", customerName)
    .Replace("{{CartItems}}", cartItemsHtml)
    .Replace("{{TotalAmount}}", totalAmount.ToString("C"))
    .Replace("{{CheckoutUrl}}", checkoutUrl)
    .Replace("{{UnsubscribeUrl}}", unsubscribeUrl);
```

## Customization

### Colors
The template uses CSS custom properties for easy color customization. Main color schemes:

- **Header Gradient**: `#667eea` to `#764ba2`
- **Cart Items Gradient**: `#f093fb` to `#f5576c`
- **Total Section Gradient**: `#4facfe` to `#00f2fe`
- **CTA Button Gradient**: `#ff6b6b` to `#ee5a24`
- **Footer Gradient**: `#2c3e50` to `#34495e`

### Branding
Update the following elements to match your brand:

1. **Company Name**: Replace "Your Store Name" in the footer
2. **Contact Information**: Update email, phone, and hours in the footer
3. **Social Media Links**: Update the social media URLs
4. **Logo**: Add your company logo to the header section if needed

### Content
Customize the messaging:

1. **Header Text**: Modify the main heading and subtitle
2. **Greeting Message**: Personalize the welcome message
3. **Urgency Text**: Adjust the call-to-action urgency messaging
4. **Footer Content**: Update support information

## Testing

### Preview
Use `cart_notification_sample.html` to preview how the email will look with sample data.

### Email Client Testing
Test the template across different email clients:
- Gmail (Web, Mobile)
- Outlook (Desktop, Web)
- Apple Mail
- Yahoo Mail
- Mobile email apps

### Responsive Testing
Test on various screen sizes:
- Desktop (1200px+)
- Tablet (768px - 1199px)
- Mobile (320px - 767px)

## Best Practices

1. **Keep Images Optimized**: Ensure product images are optimized for email
2. **Fallback Text**: Always include alt text for images
3. **Test Thoroughly**: Test across multiple email clients and devices
4. **Monitor Performance**: Track open rates, click-through rates, and conversions
5. **A/B Testing**: Consider testing different subject lines and content variations

## Support

For questions or issues with the email template, contact the development team or refer to the project documentation.
