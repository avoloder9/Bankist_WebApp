namespace API.Helper
{
    public class CurrencyConverter
    {


        private readonly ConversionRate[] exchangeRates;

        public CurrencyConverter()
        {
            exchangeRates = new ConversionRate[]
            {
            new ConversionRate("BAM", 1.0f),
            new ConversionRate("EUR", 1.95f),
            new ConversionRate("USD", 1.82f)
            };
        }

        public float ConvertAmount(float amount, string sourceCurrency, string targetCurrency)
        {
            ConversionRate sourceRate = FindRateByCurrency(sourceCurrency);
            ConversionRate targetRate = FindRateByCurrency(targetCurrency);

            if (sourceRate != null && targetRate != null)
            {
                float convertedAmount = amount * (sourceRate.Rate / targetRate.Rate);
                return convertedAmount;
            }
            else
            {
                return amount;
            }
        }

        private ConversionRate FindRateByCurrency(string currency)
        {

            foreach (var rate in exchangeRates)
            {
                if (rate.Currency == currency)
                {
                    return rate;
                }
            }
            return null;
        }

        private class ConversionRate
        {
            public string Currency { get; }
            public float Rate { get; }

            public ConversionRate(string currency, float rate)
            {
                Currency = currency;
                Rate = rate;
            }

        }
    }
}
