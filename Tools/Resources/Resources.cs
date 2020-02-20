using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core.Resources
{
    public static class Resources
    {
        public static string SearchTypeCardByNumber(string cardNumber)
        {
            var cardRegex = new Dictionary<string, string>
            {
                {"ELECTRON", @"(4026|417500|4405|4508|4844|4913|4917)\d+"},
                {"MAESTRO", @"(5018|5020|5038|5612|5893|6304|6759|6761|6762|6763|0604|6390)\d+"},
                {"DANKORT", @"(5019)\d+"},
                {"INTERPAYMENT", @"(636)\d+"},
                {"UNIONPAY", @"(62|88)\d+"},
                {"VISA", @"4[0-9]{12}(?:[0-9]{3})?"},
                {"MASTERCARD", @"5[1-5][0-9]{14}"},
                {"AMEX", @"3[47][0-9]{13}"},
                {"DINERS", @"3(?:0[0-5]|[68][0-9])[0-9]{11}"},
                {"DISCOVER", @"6(?:011|5[0-9]{2})[0-9]{12}"},
                {"JCB", @"(?:2131|1800|35\d{3})\d{11}"}
            };

            var typeCard = string.Empty;
            foreach (var reg in cardRegex)
            {
                if (!Regex.IsMatch(cardNumber, reg.Value, RegexOptions.IgnoreCase)) continue;
                typeCard = reg.Key;
                break;
            }

            return typeCard;
        }
    }
}
