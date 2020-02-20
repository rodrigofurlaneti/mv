using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using Aplicacao.br.com.payzen.secure;
using Core.Exceptions;
using Entidade;

namespace Aplicacao.apipayzen
{
    public static class ApiPayzenBase
    {
        #region Properties
        private static readonly string UrlApiWsdl = ConfigurationManager.AppSettings["API_URL_WSDL_PAYZEN"];
        private static readonly string UrlHeader = ConfigurationManager.AppSettings["API_URL_HEADER_PAYZEN"];
        private static readonly string ApiShopid = ConfigurationManager.AppSettings["API_SHOPID_PAYZEN"];
        private static readonly string ApiMode = ConfigurationManager.AppSettings["API_MODE_PAYZEN"];
        private static readonly string ApiCertificado = ConfigurationManager.AppSettings["API_CERTIFICADO_PAYZEN"];
        public static PayzenHeader SoapHeader { get; set; }
        #endregion

        /// <summary>
        /// Criação de Pagamento do Pedido
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns>KeyValuePair (TransactionUuid, Message)</returns>
        public static KeyValuePair<string, string> CreatePayment(Pedido pedido)
        {
            #region SoapHeader
            SoapHeader = new PayzenHeader
            {
                ShopId = ApiShopid,
                RequestId = Guid.NewGuid().ToString(),
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Mode = ApiMode
            };

            var concatRequestIdTimeStamp = SoapHeader.RequestId + SoapHeader.Timestamp;
            SoapHeader.AuthToken = SecurityHmacSha256.HmacSha256(concatRequestIdTimeStamp, ApiCertificado);
            #endregion

            #region ObjectsRequest
            var common = new commonRequest
            {
                paymentSource = PaymentSource.EC.ToString(),
                submissionDate = pedido.DataInsercao,
                submissionDateSpecified = true
            };

            var payment = new paymentRequest
            {
                //transactionId = "0",
                amount = Convert.ToInt64(pedido.ListaCompra.Total), //não concordo com a conversão
                amountSpecified = true,
                currency = 986, //BRL - Real,
                currencySpecified = true
            };
            if (!string.IsNullOrEmpty(pedido?.ListaHistorico?.LastOrDefault(x => !string.IsNullOrEmpty(x.CodigoRetornoTransacao))?.CodigoRetornoTransacao))
                payment.retryUuid = pedido?.ListaHistorico?.LastOrDefault(x => !string.IsNullOrEmpty(x.CodigoRetornoTransacao))?.CodigoRetornoTransacao;

            var order = new orderRequest
            {
                orderId = $"{ApiMode}-{pedido.Id}"
            };

            var card = new cardRequest
            {
                expiryYear = Convert.ToInt32($"20{pedido.Cartao.Validade.Split('/').LastOrDefault()}"), //2020,
                expiryYearSpecified = true,
                expiryMonth = Convert.ToInt32(pedido.Cartao.Validade.Split('/').FirstOrDefault()), //4,
                expiryMonthSpecified = true,
                cardSecurityCode = pedido.Cartao.Cvv, //"123",
                number = pedido.Cartao.NumeroSemMascara, //"5970100300000018",
                scheme = "MASTERCARD"//Core.Resources.Resources.SearchTypeCardByNumber(pedido.Cartao.NumeroSemMascara)
            };
            #endregion

            #region Create - SoapRequest
            var body = Serialization.Serialize(common);
            body += Serialization.Serialize(payment);
            body += Serialization.Serialize(order);
            body += Serialization.Serialize(card);

            var soapEnvelopeXml = CreateSoapEnvelope(UrlHeader, SoapHeader, PayzenMethods.createPayment.ToString(), body);
            var webRequest = CreateWebRequest(UrlApiWsdl);

            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            #endregion

            string result;
            using (var response = webRequest.GetResponse())
            {
                using (var rd = new StreamReader(response.GetResponseStream()))
                {
                    result = rd.ReadToEnd();
                }
            }

            //Extraindo o objeto [CreatePaymentResponseCreatePaymentResult]
            if (!result.Contains("createPaymentResult"))
                throw new BusinessRuleException($"Não encontrado [createPaymentResult]: {result}");

            var xml = new XmlDocument();
            xml.LoadXml(result);
            var xmlMethod = xml.DocumentElement.SelectNodes("//createPaymentResult").Item(0).OuterXml.Replace("createPaymentResult", "createPaymentResponseCreatePaymentResult");
            var methodResult = Serialization.LoadFromXmlString<createPaymentResponseCreatePaymentResult>(xmlMethod);

            TransactionStatus statusPagamento;
            Enum.TryParse(methodResult.commonResponse.transactionStatusLabel, out statusPagamento);

            return new KeyValuePair<string, string>(methodResult.paymentResponse.transactionUuid,
                $"{(methodResult.commonResponse.responseCode == 0 && statusPagamento == TransactionStatus.AUTHORISED ? "sucesso" : "erro")} - responseCodeDetails [{methodResult.commonResponse.responseCodeDetail}] transactionStatusLabel [{methodResult.commonResponse.transactionStatusLabel}]");
        }

        #region AuxMethods
        private static HttpWebRequest CreateWebRequest(string url)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "application/soap+xml;charset=UTF-8";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
        private static XmlDocument CreateSoapEnvelope(string url, PayzenHeader header, string method, string methodBody)
        {
            var soapEnvelopeXml = new XmlDocument();
            soapEnvelopeXml.LoadXml($"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:v5=\"{url}\">" +
                   $"<soap:Header xmlns:soapHeader=\"{url}Header\">" +
                   $"<soapHeader:shopId>{header.ShopId}</soapHeader:shopId>" +
                   $"<soapHeader:requestId>{header.RequestId}</soapHeader:requestId>" +
                   $"<soapHeader:timestamp>{header.Timestamp}</soapHeader:timestamp>" +
                   $"<soapHeader:mode>{header.Mode}</soapHeader:mode>" +
                   $"<soapHeader:authToken>{header.AuthToken}</soapHeader:authToken>" +
                   "</soap:Header>" +
                   "<soap:Body>" +
                   $"<v5:{method}>" +
                   methodBody +
                   $"</v5:{method}>" +
                   "</soap:Body>" +
                   "</soap:Envelope>");

            return soapEnvelopeXml;
        }
        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, WebRequest webRequest)
        {
            using (var stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
        #endregion
    }
}
