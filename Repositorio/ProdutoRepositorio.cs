using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositorio
{
    public class ProdutoRepositorio : NHibRepository<Produto>, IProdutoRepositorio
    {
        public ProdutoRepositorio(NHibContext context)
            : base(context)
        {
        }
        
        public IList<DepartamentoProduto> BuscarDepartamentoPorLoja(int loja)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT DISTINCT d FROM Produto p");
            sql.Append("  JOIN p.Lojas l ");
            sql.Append("  JOIN p.SubGrupo sg ");
            sql.Append("  JOIN sg.Grupo g ");
            sql.Append("  JOIN g.Secao s ");
            sql.Append("  JOIN s.Departamento d ");

            if (loja > 0)
                sql.Append($" WHERE l.Id = {loja}");

            var query = Session.CreateQuery(sql.ToString());

            return query.List<DepartamentoProduto>()?.ToList() ?? new List<DepartamentoProduto>();
        }

        public IList<SecaoProduto> BuscarSecaoPorDepartamentoELoja(int loja, int departamento)
        {
            var sql = new StringBuilder();

            sql.Append("SELECT DISTINCT s FROM Produto p");
            sql.Append("  JOIN p.Lojas l ");
            sql.Append("  JOIN p.SubGrupo sg ");
            sql.Append("  JOIN sg.Grupo g ");
            sql.Append("  JOIN g.Secao s ");
            sql.Append("  JOIN s.Departamento d ");
            sql.Append(" WHERE 1 = 1 ");

            if (loja > 0)
                sql.Append($" AND l.Id = {loja}");
            if (departamento > 0)
                sql.Append($" AND d.Id = {departamento}");

            var query = Session.CreateQuery(sql.ToString());

            return query.List<SecaoProduto>()?.ToList() ?? new List<SecaoProduto>();
        }

        public List<Produto> BucarUltimoIdProduto()
        {
            var sql = new StringBuilder();

            sql.Append("SELECT TOP 1 p.Id FROM Produto p order by p.Id desc");

            var query = Session.CreateQuery(sql.ToString());

            return query.List<Produto>().ToList();
        }
    }
}
