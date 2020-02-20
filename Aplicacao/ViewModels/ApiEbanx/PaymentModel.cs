using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class PaymentModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string document { get; set; }
        public string address { get; set; }
        public string street_number { get; set; }
        public string street_complement { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipcode { get; set; }
        public string country { get; set; }
        public string phone_number { get; set; }
        public string customer_ip { get; set; }
        public string payment_type_code { get; set; }
        public string merchant_payment_code { get; set; }
        public string currency_code { get; set; }
        public string user_value_1 { get; set; }
        public string user_value_2 { get; set; }
        public string user_value_3 { get; set; }
        public string user_value_4 { get; set; }
        public string user_value_5 { get; set; }
        public int instalments { get; set; }
        public decimal amount_total { get; set; }
        public string due_date { get; set; }
        public bool create_token { get; set; }
        public string token { get; set; }
        public string note { get; set; }
        public string eft_code { get; set; }
        public string notification_url { get; set; }
        public string redirect_url { get; set; }
        public string person_type { get; set; }
        public CreditCardModel creditcard { get; set; }
        public SubAccountModel sub_account { get; set; }
        public ResponsibleModel responsible { get; set; }
        public IEnumerable<PaymentItemModel> items { get; set; }
    }
}
