using System;

namespace Aplicacao.ViewModels
{
    public class EspacoLojaArquivosViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ArquivoViewModel Arquivo { get; set; }
    }
}