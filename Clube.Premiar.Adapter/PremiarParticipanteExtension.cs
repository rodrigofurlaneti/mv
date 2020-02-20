using Clube.Premiar.Models;
using Core.Extensions;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Clube.Premiar.Adapter
{
    public static class PremiarParticipanteExtension
    {
        public static Participant FromUsuario(this Participant participante, Usuario usuario)
        {
            Func<string, string> getHashFor = new Func<string, string>(input => {
                string result;
                using (MD5 hash = MD5.Create())
                {
                    result = String.Join
                    (
                        "",
                        from ba in hash.ComputeHash
                        (
                            Encoding.UTF8.GetBytes(input)
                        )
                        select ba.ToString("x2")
                    );
                }
                return result;
            });

            Func<Usuario, string> getUsernameFor = new Func<Usuario, string>(usu =>
            {
                //var nameParts = usu.Pessoa.Nome.Split(' ');
                //return nameParts.First()[0] + nameParts.LastOrDefault();

                //return usu.Pessoa.Email.Split('@').First();

                //return usu.Pessoa.Cpf.ExtractNumbers();

                return usu.Pessoa.Cartoes.FirstOrDefault()?.NumeroSemMascara;
            });

            Func<Usuario, string> getPasswordFor = new Func<Usuario, string>(usu =>
            {
                //var hash = getHashFor(usu.Pessoa.Cpf.ExtractNumbers());
                //return hash.Substring(0, Math.Min(hash.Length, 30));

                return usu.Pessoa.Cpf.ExtractNumbers().Substring(11 - 5, 5);
            });

            Func<Usuario, Models.Address> getUserAddress = new Func<Usuario, Models.Address>((usu)=> {
                var endereco = usu.Pessoa.EnderecosEntrega.FirstOrDefault();
                return new Models.Address
                {
                    city = endereco?.Cidade?.Descricao ?? "",
                    complement = endereco?.Complemento ?? "",
                    district = endereco?.Bairro ?? "",
                    number = endereco?.Numero ?? "",
                    state = endereco?.Cidade?.Estado?.Sigla ?? "SP",
                    street = endereco?.Logradouro ?? "",
                    zipCode = endereco?.Cep ?? "01001001"
                };
            });

            Func<Entidade.Uteis.TipoContato, string> getPhoneType = new Func<Entidade.Uteis.TipoContato, string>((tp) => {
                switch (tp)
                {
                    case Entidade.Uteis.TipoContato.Email:
                        return "EMAIL";
                    case Entidade.Uteis.TipoContato.Residencial:
                        return "HOME";
                    case Entidade.Uteis.TipoContato.Celular:
                        return "MOBILE";
                    case Entidade.Uteis.TipoContato.Recado:
                        return "OTHERS";
                    case Entidade.Uteis.TipoContato.Comercial:
                        return "WORK";
                    case Entidade.Uteis.TipoContato.Fax:
                        return "FAX";
                    case Entidade.Uteis.TipoContato.OutroEmail:
                        return "EMAIL";
                    default:
                        return "OTHERS";
                }
            });

            Func<Contato, Models.Phone> getUserPhone = new Func<Contato, Phone>((ctt) =>
            {
                return new Phone
                {
                    areaCode = ctt.DDD ?? "",
                    number = ctt.Numero.Substring(5).Replace("-", "") ?? "",
                    type = getPhoneType(ctt.Tipo)
                };
            });

            var newParticipante = new Participant
            {
                name = usuario.Pessoa.Nome,
                birthDate = usuario.Pessoa.DataNascimento > DateTime.MinValue ? usuario.Pessoa.DataNascimento : (DateTime?)null,
                documentNumber = usuario.Pessoa.Cpf,
                genderType = (usuario.Pessoa.Sexo?.Equals("1") ?? true) ? "MALE" : "FEMALE",
                personType = "INDIVIDUAL",
                status = "ACTIVE",
                maritalStatus = "SINGLE",
                emails = new List<Email> {
                    new Email { email = usuario.Pessoa.Email, type = "PERSONAL" }
                },
                address = getUserAddress(usuario),
                username = getUsernameFor(usuario),
                password = getPasswordFor(usuario)
            };

            foreach (var contato in usuario.Pessoa.Contatos)
            {
                if (!new[] {
                    Entidade.Uteis.TipoContato.Email,
                    Entidade.Uteis.TipoContato.OutroEmail
                }.Contains(contato.Contato.Tipo))

                    if (!string.IsNullOrEmpty(contato.Contato.Numero))
                        newParticipante.phones.Add(getUserPhone(contato.Contato));
            }

            return newParticipante;
        }
    }
}
