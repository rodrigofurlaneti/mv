using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Repositorio.Base;

namespace Repositorio
{
    public class LojaRepositorio : NHibRepository<Loja>, ILojaRepositorio
    {
        public LojaRepositorio(NHibContext context)
            : base(context)
        {
        }

        public IList<Loja> BuscaLojasPor(int estado, int cidade, string bairro, string dadosPesquisa = "")
        {
            var sql = new StringBuilder();

            sql.Append("SELECT distinct L.Id, L.Telefone, L.DataInsercao, " +
                        "   L.Descricao, L.Cnpj, L.RazaoSocial, L.InscricaoEstadual, L.Status, " +
                        "   0, L.NotaAvaliacao, L.LogoUpload, L.Classificacao,0,0, " + //Adicionado os 0 para nao alterar as posicoes de endereco
                        "   E.Id, E.Cep, E.Tipo, E.Logradouro, E.Numero, E.Complemento, E.Bairro, " +
                        "   E.Descricao, E.Latitude, E.Longitude, E.DataInsercao, E.Cidade_Id, CID.Descricao as Cidade, CID.Estado_Id, EST.Sigla " +
                        " FROM LOJA L " +
                        " LEFT JOIN ENDERECO E ON L.Endereco = E.Id" +
                        " LEFT JOIN CIDADE CID ON CID.Id = E.Cidade_Id " +
                        " LEFT JOIN ESTADO EST ON EST.Id = CID.Estado_Id " +
                        " WHERE L.Status = 1 and L.LojaAprovada = 1");

            if (estado != 0)
                sql.Append($" and EST.Id = '{estado}' ");

            if (cidade != 0)
                sql.Append($" and CID.Id = '{cidade}' ");

            if (!string.IsNullOrEmpty(bairro))
                sql.Append($" and E.Bairro like '%{bairro}%' ");

            if (!string.IsNullOrEmpty(dadosPesquisa))
            {
                if (dadosPesquisa.Contains(";"))
                {
                    sql.Append($" and L.CodigoInfox in('{dadosPesquisa.Replace(";", "','")}')");
                }
                else
                {
                    sql.Append($" and (L.Descricao like '%{dadosPesquisa}%' or L.RazaoSocial like '%{dadosPesquisa}%' or L.Cnpj like '%{dadosPesquisa}%' or cast(L.CodigoInfox as varchar) like '{dadosPesquisa}' )");
                }
            }

            var sqlQuery = Session.CreateSQLQuery(sql.ToString()).SetCacheable(true);
            return ConverterResultadoPesquisaEmObjeto(sqlQuery.List())?.ToList() ?? new List<Loja>();
        }

        public IList<Loja> BuscarLojas(string classificacao = "")
        {
            var sql = new StringBuilder();

            sql.Append("SELECT L.Id, L.Telefone, L.DataInsercao, " +
                        "   L.Descricao, L.Cnpj, L.RazaoSocial, L.InscricaoEstadual, L.Status, " +
                        "   0, L.NotaAvaliacao, L.LogoUpload, L.Classificacao,0,0, " + //Adicionado os 0 para nao alterar as posicoes de endereco
                        "   E.Id, E.Cep, E.Tipo, E.Logradouro, E.Numero, E.Complemento, E.Bairro, " +
                        "   E.Descricao, E.Latitude, E.Longitude, E.DataInsercao, E.Cidade_Id, CID.Descricao as Cidade, CID.Estado_Id, EST.Sigla " +
                        " FROM LOJA L " +
                        " LEFT JOIN ENDERECO E ON L.Endereco = E.Id" +
                        " LEFT JOIN CIDADE CID ON CID.Id = E.Cidade_Id " +
                        " LEFT JOIN ESTADO EST ON EST.Id = CID.Estado_Id " +
                        " WHERE L.Status = 1 and L.LojaAprovada = 1");

            if (!string.IsNullOrEmpty(classificacao) && classificacao != TipoClassificacao.Nenhum.ToString())
                sql.Append(" and (L.Classificacao = '" + classificacao + "' or L.Classificacao = '" + TipoClassificacao.Ambos.ToString() + "')");

            var query = Session.CreateSQLQuery(sql.ToString()).SetCacheable(true);

            return ConverterResultadoPesquisaEmObjeto(query.List())?.ToList() ?? new List<Loja>();
        }

        public IList<Loja> BuscarLojasPorNome(string nome = "")
        {
            var sql = new StringBuilder();

            sql.Append("SELECT L.Id, L.Telefone, L.DataInsercao, " +
                        "   L.Descricao, L.Cnpj, L.RazaoSocial, L.InscricaoEstadual, L.Status, " +
                        "   0, L.NotaAvaliacao, L.LogoUpload, L.Classificacao,0,0, " + //Adicionado os 0 para nao alterar as posicoes de endereco
                        "   E.Id, E.Cep, E.Tipo, E.Logradouro, E.Numero, E.Complemento, E.Bairro, " +
                        "   E.Descricao, E.Latitude, E.Longitude, E.DataInsercao, E.Cidade_Id, CID.Descricao as Cidade, CID.Estado_Id, EST.Sigla " +
                        " FROM LOJA L " +
                        " LEFT JOIN ENDERECO E ON L.Endereco = E.Id" +
                        " LEFT JOIN CIDADE CID ON CID.Id = E.Cidade_Id " +
                        " LEFT JOIN ESTADO EST ON EST.Id = CID.Estado_Id " +
                        " WHERE L.Status = 1 and L.LojaAprovada = 1");

            if (!string.IsNullOrEmpty(nome))
                sql.Append(" and (L.Descricao like '%" + nome + "%' or L.RazaoSocial like '%" + nome + "%')");

            var query = Session.CreateSQLQuery(sql.ToString()).SetCacheable(true);

            return ConverterResultadoPesquisaEmObjeto(query.List())?.ToList() ?? new List<Loja>();
        }

        public IList<Loja> BuscarLojasPorTipoPorNome(string nomeTipo = "")
        {
            var sql = new StringBuilder();

            sql.Append("SELECT L.Id, L.Telefone, L.DataInsercao, " +
                        "   L.Descricao, L.Cnpj, L.RazaoSocial, L.InscricaoEstadual, L.Status, " +
                        "   0, L.NotaAvaliacao, L.LogoUpload, L.Classificacao,0,0, " + //Adicionado os 0 para nao alterar as posicoes de endereco
                        "   E.Id, E.Cep, E.Tipo, E.Logradouro, E.Numero, E.Complemento, E.Bairro, " +
                        "   E.Descricao, E.Latitude, E.Longitude, E.DataInsercao, E.Cidade_Id, CID.Descricao as Cidade, CID.Estado_Id, EST.Sigla " +
                        " FROM LOJA L " +
                        " LEFT JOIN ENDERECO E ON L.Endereco = E.Id" +
                        " LEFT JOIN CIDADE CID ON CID.Id = E.Cidade_Id " +
                        " LEFT JOIN ESTADO EST ON EST.Id = CID.Estado_Id " +
                        " LEFT JOIN LojaTipoLoja TPL ON TPL.Loja = L.Id " +
                        " LEFT JOIN TipoLoja TP ON TP.Id = TPL.TipoLoja " +
                        " WHERE L.Status = 1 and L.LojaAprovada = 1");

            if (!string.IsNullOrEmpty(nomeTipo))
                sql.Append(" and (TP.Descricao like '%" + nomeTipo + "%')");

            var query = Session.CreateSQLQuery(sql.ToString()).SetCacheable(true);

            return ConverterResultadoPesquisaEmObjeto(query.List())?.ToList() ?? new List<Loja>();
        }

        public IList<Loja> ConverterResultadoPesquisaEmObjeto(IList results)
        {
            var lista = new List<Loja>();
            foreach (object[] p in results)
            {
                var item = new Loja
                {
                    Id = p.Length > 0 && p[0] != null ? Convert.ToInt32(p[0] ?? 0) : 0,
                    Telefone = p.Length > 1 && p[1] != null ? p[1]?.ToString() ?? string.Empty : string.Empty,
                    DataInsercao = p.Length > 2 && p[2] != null ? DateTime.Parse(p[2]?.ToString()) : DateTime.Now.Date,
                    Descricao = p.Length > 3 && p[3] != null ? p[3]?.ToString() ?? string.Empty : string.Empty,
                    Cnpj = p.Length > 4 && p[4] != null ? p[4]?.ToString() ?? string.Empty : string.Empty,
                    RazaoSocial = p.Length > 5 && p[5] != null ? p[5]?.ToString() ?? string.Empty : string.Empty,
                    InscricaoEstadual = p.Length > 6 && p[6] != null ? p[6]?.ToString() ?? string.Empty : string.Empty,
                    Status = p.Length > 7 && p[7] != null ? Convert.ToBoolean(p[7] ?? 0) : true,
                    NotaAvaliacao = p.Length > 9 && p[9] != null ? Convert.ToInt32(p[9] ?? 0) : 0,
                    LogoUpload = p.Length > 10 && p[10] != null ? p[10].ToString() : null,
                    Classificacao = p.Length > 11 && p[11] != null ? p[11].ToString() : null,

                    Endereco = new Endereco
                    {
                        Id = p.Length > 14 && p[14] != null ? Convert.ToInt32(p[14] ?? 0) : 0,
                        Cep = p.Length > 15 && p[15] != null ? p[15]?.ToString() ?? string.Empty : string.Empty,
                        Tipo = p.Length > 16 && p[16] != null ? Convert.ToInt32(p[16] ?? 0) : (int)TipoEndereco.Comercial,
                        Logradouro = p.Length > 17 && p[17] != null ? p[17]?.ToString() : string.Empty,
                        Numero = p.Length > 18 && p[18] != null ? p[18]?.ToString() : string.Empty,
                        Complemento = p.Length > 19 && p[19] != null ? p[19]?.ToString() : string.Empty,
                        Bairro = p.Length > 20 && p[20] != null ? p[20]?.ToString() : string.Empty,
                        Descricao = p.Length > 21 && p[21] != null ? p[21]?.ToString() : string.Empty,
                        Latitude = p.Length > 22 && p[22] != null ? p[22]?.ToString() : string.Empty,
                        Longitude = p.Length > 23 && p[23] != null ? p[23]?.ToString() : string.Empty,
                        DataInsercao = p.Length > 24 && p[24] != null ? DateTime.Parse(p[24]?.ToString()) : DateTime.Now.Date,
                        Cidade = new Cidade
                        {
                            Id = p.Length > 25 && p[25] != null ? Convert.ToInt32(p[25] ?? 0) : 0,
                            Descricao = p.Length > 26 && p[26] != null ? p[26]?.ToString() : string.Empty,
                            Estado = new Estado
                            {
                                Id = p.Length > 27 && p[27] != null ? Convert.ToInt32(p[27] ?? 0) : 0,
                                Sigla = p.Length > 28 && p[28] != null ? p[28]?.ToString() : string.Empty
                            }
                        }
                    }
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}
