export class ProductLoja {
    Id: number;
    Nome: string;
    Descricao: string;
    Codigo: string;
    CodigoBarras: string;
    Valor: number = 0;
    ValorDesconto: number = 0;
    Imagens: string[];
    Quantidade: number = 0;
    CategoriaProdutoId: number = 0;
    Data: any;
    Produto: any;
    InicioVigencia: string;
    FimVigencia: string;
    Status: boolean;
    Preco: any;

    constructor(data: any) {
        this.Id = data.Produto.Id;
        this.Nome = data.Produto.Nome;
        this.Descricao = data.Produto.Descricao;
        this.Codigo = data.Produto.Codigo;
        this.CodigoBarras = data.Produto.CodigoBarras;
        this.Valor = data.Valor;
        this.ValorDesconto = data.ValorDesconto;
        this.Quantidade = data.Quantidade;
        this.Data = data;
        this.CategoriaProdutoId = data.Produto.CategoriaProduto.Id;
        this.Imagens = data.Produto.Imagens;
        this.Produto = data.Produto;
        this.InicioVigencia = data.InicioVigencia;
        this.FimVigencia = data.FimVigencia;
        this.Status = data.Status;
        this.Preco = data.Preco;
    }

    public static listFromData(data: any): ProductLoja[] {
        let products: ProductLoja[] = [];

        data.forEach( d => {
            products.push(new ProductLoja(d));
        });

        return products;
    }
}
