using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class FormularioContato 
    {
	    public string Usuario { get; set; }
	    public string Email { get; set; }
	    public string Telefone { get; set; }
	    public string Assunto { get; set; }
	    public string Descricao { get; set; }
    }
}