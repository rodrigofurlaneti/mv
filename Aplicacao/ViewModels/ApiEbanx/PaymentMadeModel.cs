using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.apiebanx.Models
{
    public class PaymentMadeModel
    {
        public string hash { get; set; }
        public string pin { get; set; }
        public string country { get; set; }
        public string merchant_payment_code { get; set; }
        public string order_number { get; set; }
        public string status { get; set; }
        public DateTime? status_date { get; set; }
        public DateTime? open_date { get; set; }
        public DateTime? confirm_date { get; set; }
        public DateTime? transfer_date { get; set; }
        public string amount_br { get; set; }
        public string amount_ext { get; set; }
        public string amount_iof { get; set; }
        public string currency_rate { get; set; }
        public string currency_ext { get; set; }
        public string due_date { get; set; }
        public string instalments { get; set; }
        public string payment_type_code { get; set; }
        public string boleto_url { get; set; }
        public string boleto_barcode { get; set; }
        public string boleto_barcode_raw { get; set; }
        public string redirect_url { get; set; }
        public string pay_with_balance_url { get; set; }
        public string cip_url { get; set; }
        public string cip_code { get; set; }
        public bool pre_approved { get; set; }
        public bool capture_available { get; set; }
        public bool chargeback_credit { get; set; }
        public PaymentDetailsModel details { get; set; }
        public RefundModel[] refunds { get; set; }
        public PaymentChargebackModel[] chargeback { get; set; }
        public TransactionStatusModel transaction_status { get; set; }
    }
}
