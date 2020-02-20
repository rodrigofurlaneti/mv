using ApiInfox.Models;
using Core.Exceptions;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace ApiInfox.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/saldoextrato")]
    public class SaldoExtratoController : BaseApiController
    {
        [HttpPost]
        [Route("saldo/{idCliente}/{cardNumber}/{cardPass}")]
        public virtual CartaoModelView Saldo(string idCliente, string cardNumber, string cardPass)
        {
            var cliente = new ClienteModelView
            {
                Id = idCliente,
                Cartao = new CartaoModelView
                {
                    Numero = cardNumber,
                    Senha = cardPass
                }
            };

            var inicializacao = new InicializacaoModelView
            {
                CodCliente = cliente.Id,
                NSU = ConfigurationManager.AppSettings["NSU"].ToString()
            };

            InicializacaoApiInfox(ref inicializacao);

            Saldo(ref inicializacao, ref cliente);

            return cliente.Cartao;
        }

        [HttpPost]
        [Route("extrato/{idCliente}/{cardNumber}/{cardPass}/{codEstabelecimento}")]
        public virtual ExtratoModelView Extrato(string idCliente, string cardNumber, string cardPass, string codEstabelecimento)
        {
            var cliente = new ClienteModelView
            {
                Id = idCliente,
                Cartao = new CartaoModelView
                {
                    Numero = cardNumber,
                    Senha = cardPass
                }
            };

            var inicializacao = new InicializacaoModelView
            {
                CodCliente = cliente.Id,
                NSU = ConfigurationManager.AppSettings["NSU"].ToString()
            };

            InicializacaoApiInfox(ref inicializacao);

            return Extrato(ref inicializacao, cliente, codEstabelecimento);
        }

        private void Saldo(ref InicializacaoModelView inicializacao, ref ClienteModelView cliente)
        {
            try
            {
                var CodResp = string.Empty;
                var NSU_Rede = string.Empty;
                var StrResp = string.Empty;
                var SaldoDisponivel = string.Empty;
                var LimiteCredito = string.Empty;
                var DiaVencimento = string.Empty;

                var workKeyData = Helpers.CryptoApiInfox.DecryptApi(inicializacao.WorkKey, MasterKey);
                var chaveAcessoChamadaConcatenada = Helpers.CryptoApiInfox.EncryptApi($"{cliente.Id}{NSURequest}{Helpers.CryptoApiInfox.DecryptApi(inicializacao.ChaveAcessoTx, workKeyData)}", workKeyData);

                var cartaoNumeroEncrypted = Helpers.CryptoApiInfox.EncryptApi(cliente.Cartao.Numero, workKeyData);
                var cartaoSenhaEncrypted = Helpers.CryptoApiInfox.EncryptApi(cliente.Cartao.Senha, workKeyData);

                var dados = _service.ConsultaSaldo(chaveAcessoChamadaConcatenada,
                    null,
                    ConfigurationManager.AppSettings["TIPO_CAPTURA"].ToString(),
                    null,
                    cartaoNumeroEncrypted,
                    NSURequest,
                    null,
                    cartaoSenhaEncrypted,
                    ref CodResp,
                    ref NSU_Rede,
                    ref SaldoDisponivel,
                    ref StrResp);

                cliente.Cartao.Numero = $"{cliente.Cartao.Numero.Substring(0, 4)} XXXX XXXX {cliente.Cartao.Numero.Substring(cliente.Cartao.Numero.Length -4 , 4)}";
                cliente.Cartao.Senha = null;

                if (dados > 0)
                {
                    cliente.Cartao.Return = dados;
                    cliente.Cartao.CodResp = dados.ToString();
                    cliente.Cartao.StrResp = $"Retorno da Chamada de Saldo: {StrResp}";
                    cliente.Cartao.SaldoDisponivel = string.Empty;
                    cliente.Cartao.LimiteCredito = string.Empty;
                    cliente.Cartao.DiaVencimento = string.Empty;
                    return;
                }

                var frases = StrResp.Replace("@@", "@").Split('@').ToList();
                LimiteCredito = frases?.FirstOrDefault(x => x.Contains("LIMITE"))?.Split(':')?.Last()?.Trim() ?? string.Empty;
                DiaVencimento = frases?.FirstOrDefault(x => x.Contains("VENCIMENTO"))?.Split(':')?.Last()?.Trim() ?? string.Empty;

                cliente.Cartao.Return = dados;
                cliente.Cartao.CodResp = CodResp;
                cliente.Cartao.NSU_Rede = NSU_Rede;
                cliente.Cartao.StrResp = StrResp;
                cliente.Cartao.SaldoDisponivel = SaldoDisponivel.Length <= 1 ? SaldoDisponivel : $"{SaldoDisponivel.Substring(0, SaldoDisponivel.Length - 2).Replace(",", "")},{SaldoDisponivel.Substring(SaldoDisponivel.Length - 2)}";
                cliente.Cartao.LimiteCredito = LimiteCredito;
                cliente.Cartao.DiaVencimento = DiaVencimento;
            }
            catch (BusinessRuleException br)
            {
                throw br;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private ExtratoModelView Extrato(ref InicializacaoModelView inicializacao, ClienteModelView cliente, string codEstabelecimento)
        {
            try
            {
                var CodResp = string.Empty;
                var NSU_Rede = string.Empty;
                var StrResp = string.Empty;
                var SaldoDisponivel = string.Empty;
                var LimiteCredito = string.Empty;
                var DiaVencimento = string.Empty;
                var DataHoraExtrato = string.Empty;

                var workKeyData = Helpers.CryptoApiInfox.DecryptApi(inicializacao.WorkKey, MasterKey);
                var chaveAcessoChamadaConcatenada = Helpers.CryptoApiInfox.EncryptApi($"{cliente.Id}{NSURequest}{Helpers.CryptoApiInfox.DecryptApi(inicializacao.ChaveAcessoTx, workKeyData)}", workKeyData);

                var cartaoNumeroEncrypted = Helpers.CryptoApiInfox.EncryptApi(cliente.Cartao.Numero, workKeyData);
                var cartaoSenhaEncrypted = Helpers.CryptoApiInfox.EncryptApi(cliente.Cartao.Senha, workKeyData);

                var dados = _service.ConsultaExtrato(chaveAcessoChamadaConcatenada,
                    codEstabelecimento,
                    ConfigurationManager.AppSettings["TIPO_CAPTURA"].ToString(),
                    null,
                    cartaoNumeroEncrypted,
                    NSURequest,
                    null,
                    cartaoSenhaEncrypted,
                    ref CodResp,
                    ref NSU_Rede,
                    ref SaldoDisponivel,
                    ref StrResp);

                cliente.Cartao.Numero = $"{cliente.Cartao.Numero.Substring(0, 4)} XXXX XXXX {cliente.Cartao.Numero.Substring(cliente.Cartao.Numero.Length - 4, 4)}";
                cliente.Cartao.Senha = null;

                if (dados > 0)
                    return new ExtratoModelView
                    {
                        Return = dados,
                        CodResp = dados.ToString(),
                        StrResp = $"Retorno da Chamada de Extrato: {StrResp}",
                        Cartao = cliente.Cartao
                    };

                var frases = StrResp.Replace("@@", "@").Split('@').ToList();
                LimiteCredito = frases?.FirstOrDefault(x => x.Contains("LIMITE"))?.Split(':')?.Last()?.Trim() ?? string.Empty;
                DiaVencimento = frases?.FirstOrDefault(x => x.Contains("VENCIMENTO"))?.Split(':')?.Last()?.Trim() ?? string.Empty;
                SaldoDisponivel = frases?.FirstOrDefault(x => x.Contains("SALDO DISPONIVEL"))?.Split(':')?.Last()?.Trim() ?? string.Empty;

                var resultado = TipoCartaoEnum.Cartao;
                var listaTipoCartao = Enum.GetValues(typeof(TipoCartaoEnum)).Cast<TipoCartaoEnum>().Select(e => new KeyValuePair<TipoCartaoEnum, string>(e, e.ToDescription())).ToList();
                if (frases?.Count > 2)
                    resultado = listaTipoCartao?.FirstOrDefault(x => x.Key.ToString() == frases[1].Trim().ToUpper() || x.Value.ToUpper() == frases[1].Trim().ToUpper()).Key ?? TipoCartaoEnum.Cartao;
                cliente.Cartao.TipoCartao = new Entidade.TipoCartao
                {
                    Id = (int)resultado,
                    DataInsercao = DateTime.Now,
                    Descricao = resultado.ToDescription()
                };
                cliente.Cartao.SaldoDisponivel = SaldoDisponivel.Length <= 1 ? SaldoDisponivel : $"{SaldoDisponivel.Substring(0, SaldoDisponivel.Length - 2).Replace(",", "")},{SaldoDisponivel.Substring(SaldoDisponivel.Length - 2)}";
                DataHoraExtrato = frases?.FirstOrDefault(x => x.Contains("DATA/HORA")) != null && frases?.FirstOrDefault(x => x.Contains("DATA/HORA")).Length > 10 ? frases?.FirstOrDefault(x => x.Contains("DATA/HORA"))?.Substring(10)?.Trim()?.Replace("/", "-") ?? string.Empty : string.Empty;

                var extrato = new ExtratoModelView
                {
                    Return = dados,
                    CodResp = CodResp,
                    NSU_Rede = NSU_Rede,
                    StrResp = StrResp,
                    Cabecalho = frases?.FirstOrDefault()?.Trim(),
                    DataHoraExtrato = DataHoraExtrato != null ? Convert.ToDateTime(DataHoraExtrato) : DateTime.Now,
                    SaldoDisponivel = SaldoDisponivel.Length <= 1 ? SaldoDisponivel : $"{SaldoDisponivel.Substring(0, SaldoDisponivel.Length - 2).Replace(",", "")},{SaldoDisponivel.Substring(SaldoDisponivel.Length - 2)}",
                    LimiteCredito = LimiteCredito,
                    DiaVencimento = DiaVencimento,
                    Cartao = cliente.Cartao,
                    Itens = new List<ExtratoItemModelView>()
                };

                var mesAno = new List<string>();
                foreach (var linha in frases)
                {
                    if (linha.Contains("MES/ANO"))
                    {
                        var splitMesAno = linha.Substring(linha.LastIndexOf(':') + 1).Split('/');
                        mesAno.Add($"DAY-{splitMesAno.First().Trim()}-20{splitMesAno.Last().Trim()}");
                        continue;
                    }

                    var dia = 1;
                    if (linha.Length > 2 && int.TryParse(linha.Substring(0, 3), out dia))
                    {
                        extrato.Itens.Add(new ExtratoItemModelView
                        {
                            Data = DateTime.Parse($"{mesAno.Last().Replace("DAY", dia.ToString())}"),
                            Descricao = linha.Substring(linha.IndexOf(' '), linha.LastIndexOf(' ') - 1).Trim(),
                            Valor = Convert.ToDecimal(linha.Substring(linha.LastIndexOf(' ') + 1).Trim())
                        });
                    }
                }
                extrato.TotalValorItens = extrato.Itens?.Sum(x => x.Valor).ToString("N2") ?? "0,00";

                return extrato;
            }
            catch (BusinessRuleException br)
            {
                throw br;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}