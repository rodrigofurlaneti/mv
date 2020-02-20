using Aplicacao.ViewModels;
using AutoMapper;
using Entidade;

namespace Aplicacao.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        // Não realizar este override na versão 4.x e superiores
        public override string ProfileName => "DomainToViewModelMappings";

        protected void Configure()
        {
            CreateMap<Pessoa, PessoaViewModel>().MaxDepth(1);
            CreateMap<Endereco, EnderecoViewModel>().MaxDepth(1);
            CreateMap<Documento, DocumentoViewModel>().MaxDepth(1);
            CreateMap<Contato, ContatoViewModel>().MaxDepth(1);
            CreateMap<Usuario, UsuarioViewModel>().MaxDepth(1);

            CreateMap<Perfil, PerfilViewModel>().ForMember(x => x.Usuarios, opt => opt.MapFrom(o => o.Usuarios)).MaxDepth(1);
            CreateMap<Permissao, PermissaoViewModel>().ForMember(x => x.Perfis, opt => opt.MapFrom(o => o.Perfis)).MaxDepth(1);
            CreateMap<PerfilMenu, PerfilMenuViewModel>().MaxDepth(1);

            CreateMap<Menu, MenuViewModel>().ForMember(x => x.MenuPai, opt => opt.MapFrom(o => o.MenuPai)).MaxDepth(1);

            CreateMap<Cidade, CidadeViewModel>();
            CreateMap<Estado, EstadoViewModel>();
            CreateMap<Pais, PaisViewModel>();
        }
    }
}
