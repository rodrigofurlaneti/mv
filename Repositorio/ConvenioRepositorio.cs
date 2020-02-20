using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Repositorio.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositorio
{
    public class ConvenioRepositorio : NHibRepository<Convenio>, IConvenioRepositorio
    {
        public ConvenioRepositorio(NHibContext context)
            : base(context)
        {
        }

        public IList<Convenio> BuscaPor(int estado, int cidade, string bairro, string dadosPesquisa = "")
        {
            var sql = new StringBuilder();

            sql.Append("SELECT distinct L.Id, L.DataInsercao, " +
                        "   L.Descricao, L.Cnpj, L.RazaoSocial, L.Status, " +
                        "   E.Id, E.Cep, E.Tipo, E.Logradouro, E.Numero, E.Complemento, E.Bairro, " +
                        "   E.Descricao, E.Latitude, E.Longitude, E.DataInsercao, E.Cidade_Id, CID.Descricao as Cidade, CID.Estado_Id, EST.Sigla " +
                        " FROM CONVENIO L " +
                        " LEFT JOIN ENDERECO E ON L.Endereco = E.Id" +
                        " LEFT JOIN CIDADE CID ON CID.Id = E.Cidade_Id " +
                        " LEFT JOIN ESTADO EST ON EST.Id = CID.Estado_Id " +
                        " WHERE L.Status = 1 ");

            if (estado != 0)
                sql.Append($" and EST.Id = '{estado}' ");

            if (cidade != 0)
                sql.Append($" and CID.Id = '{cidade}' ");

            if (!string.IsNullOrEmpty(bairro))
                sql.Append($" and E.Bairro like '%{bairro}%' ");

            if (!string.IsNullOrEmpty(dadosPesquisa))
            {
                sql.Append($" and (L.Descricao like '%{dadosPesquisa}%' or L.RazaoSocial like '%{dadosPesquisa}%' or L.Cnpj like '%{dadosPesquisa}%' )");
            }

            var sqlQuery = Session.CreateSQLQuery(sql.ToString()).SetCacheable(true);
            return ConverterResultadoPesquisaEmObjeto(sqlQuery.List())?.ToList() ?? new List<Convenio>();
        }

        public IList<Convenio> ConverterResultadoPesquisaEmObjeto(IList results)
        {
            var lista = new List<Convenio>();
            foreach (object[] p in results)
            {
                var item = new Convenio
                {
                    Id = p.Length > 0 && p[0] != null ? Convert.ToInt32(p[0] ?? 0) : 0,
                    DataInsercao = p.Length > 1 && p[1] != null ? DateTime.Parse(p[1]?.ToString()) : DateTime.Now.Date,
                    Descricao = p.Length > 2 && p[2] != null ? p[2]?.ToString() ?? string.Empty : string.Empty,
                    Cnpj = p.Length > 3 && p[3] != null ? p[3]?.ToString() ?? string.Empty : string.Empty,
                    RazaoSocial = p.Length > 4 && p[4] != null ? p[4]?.ToString() ?? string.Empty : string.Empty,
                    Status = p.Length > 5 && p[5] != null ? Convert.ToBoolean(p[5] ?? 0) : true,
                    
                    Endereco = new Endereco
                    {
                        Id = p.Length > 6 && p[6] != null ? Convert.ToInt32(p[6] ?? 0) : 0,
                        Cep = p.Length > 7 && p[7] != null ? p[7]?.ToString() ?? string.Empty : string.Empty,
                        Tipo = p.Length > 8 && p[8] != null ? Convert.ToInt32(p[8] ?? 0) : (int)TipoEndereco.Comercial,
                        Logradouro = p.Length > 9 && p[9] != null ? p[9]?.ToString() : string.Empty,
                        Numero = p.Length > 10 && p[10] != null ? p[11]?.ToString() : string.Empty,
                        Complemento = p.Length > 11 && p[11] != null ? p[11]?.ToString() : string.Empty,
                        Bairro = p.Length > 12 && p[12] != null ? p[12]?.ToString() : string.Empty,
                        Descricao = p.Length > 13 && p[13] != null ? p[13]?.ToString() : string.Empty,
                        Latitude = p.Length > 14 && p[14] != null ? p[14]?.ToString() : string.Empty,
                        Longitude = p.Length > 15 && p[15] != null ? p[15]?.ToString() : string.Empty,
                        DataInsercao = p.Length > 16 && p[16] != null ? DateTime.Parse(p[16]?.ToString()) : DateTime.Now.Date,
                        Cidade = new Cidade
                        {
                            Id = p.Length > 17 && p[17] != null ? Convert.ToInt32(p[17] ?? 0) : 0,
                            Descricao = p.Length > 18 && p[18] != null ? p[18]?.ToString() : string.Empty,
                            Estado = new Estado
                            {
                                Id = p.Length > 19 && p[19] != null ? Convert.ToInt32(p[19] ?? 0) : 0,
                                Sigla = p.Length > 20 && p[20] != null ? p[20]?.ToString() : string.Empty
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
