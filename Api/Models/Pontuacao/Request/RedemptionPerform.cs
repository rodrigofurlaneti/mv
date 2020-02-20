using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Api.Models.Pontuacao.Request
{
    public partial class RedemptionPerform
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("orderId")]
        public long OrderId { get; set; }

        [JsonProperty("parentOrderId")]
        public long ParentOrderId { get; set; }

        [JsonProperty("totalPoints")]
        public long TotalPoints { get; set; }

        [JsonProperty("totalOrder")]
        public long TotalOrder { get; set; }

        [JsonProperty("payments")]
        public Payment[] Payments { get; set; }

        [JsonProperty("shippingValue")]
        public decimal ShippingValue { get; set; }

        [JsonProperty("conversionRate")]
        public long ConversionRate { get; set; }

        [JsonProperty("orderItems")]
        public OrderItem[] OrderItems { get; set; }

        [JsonProperty("parameters")]
        public Parameter[] Parameters { get; set; }
    }

    public partial class OrderItem
    {
        [JsonProperty("codeItem")]
        public long CodeItem { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("forecastDate")]
        public DateTimeOffset ForecastDate { get; set; }

        [JsonProperty("supplier")]
        public string Supplier { get; set; }

        [JsonProperty("supplierId")]
        public string SupplierId { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("unitPointsValue")]
        public decimal UnitPointsValue { get; set; }

        [JsonProperty("unitCurrencyValue")]
        public decimal UnitCurrencyValue { get; set; }

        [JsonProperty("unitCostPointsValue")]
        public decimal UnitCostPointsValue { get; set; }

        [JsonProperty("unitCostCurrencyValue")]
        public decimal UnitCostCurrencyValue { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }
    }
    
    public partial class Payment
    {
        [JsonProperty("paymentType")]
        public long PaymentType { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }
    }
}
