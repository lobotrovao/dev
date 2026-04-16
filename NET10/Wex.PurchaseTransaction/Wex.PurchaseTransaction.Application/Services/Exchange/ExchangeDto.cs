namespace Wex.PurchaseTransaction.Application.Services.Exchange
{
    using System.Text.Json.Serialization;

    public record ExchangeDto
    {
        [JsonPropertyName("data")]
        public Data[]? Data { get; init; }

        [JsonPropertyName("links")]
        public Links? Links { get; init; }
    }

    public record Links
    {
        [JsonPropertyName("self")]
        public string? Self { get; init; }

        [JsonPropertyName("first")]
        public string? First { get; init; }

        [JsonPropertyName("prev")]
        public object? Prev { get; init; }

        [JsonPropertyName("next")]
        public string? Next { get; init; }

        [JsonPropertyName("last")]
        public string? Last { get; init; }
    }

    public record Data
    {
        [JsonPropertyName("record_date")]
        public DateTime? RecordDate { get; init; }

        [JsonPropertyName("country")]
        public string? Country { get; init; }

        [JsonPropertyName("currency")]
        public string? Currency { get; init; }

        [JsonPropertyName("country_currency_desc")]
        public string? CountryCurrencyDesc { get; init; }

        [JsonPropertyName("exchange_rate")]
        public decimal ExchangeRate { get; init; }

        [JsonPropertyName("effective_date")]
        public DateTime? EffectiveDate { get; init; }

        [JsonPropertyName("src_line_nbr")]
        public int? SrcLineNbr { get; init; }

        [JsonPropertyName("record_fiscal_year")]
        public int? RecordFiscalYear { get; init; }

        [JsonPropertyName("record_fiscal_quarter")]
        public int? RecordFiscalQuarter { get; init; }

        [JsonPropertyName("record_calendar_year")]
        public int? RecordCalendarYear { get; init; }

        [JsonPropertyName("record_calendar_quarter")]
        public int? RecordCalendarQuarter { get; init; }

        [JsonPropertyName("record_calendar_month")]
        public int? RecordCalendarMonth { get; init; }

        [JsonPropertyName("record_calendar_day")]
        public int? RecordCalendarDay { get; init; }
    }
}
