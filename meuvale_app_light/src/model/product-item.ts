export class ProductItem {
    Id: number;
    Nome: string;
    Descricao: string;
    Codigo: string;
    CodigoBarras: string;
    Preco: number = 0;
    PrecoComDesconto: number = 0;
    Imagens: string[];
    Quantidade: number = 0;
    Data: any;

    constructor(data: any) {
        this.Id = data.Id;
        this.Nome = data.Nome;
        this.Descricao = data.Descricao;
        this.Codigo = data.Codigo;
        this.CodigoBarras = data.CodigoBarras;
        this.Preco = data.Valor;
        this.PrecoComDesconto = data.ValorDesconto;
        this.Data = data;
    }

    public static listFromData(data: any): ProductItem[] {
        let products: ProductItem[] = [];

        data.forEach(d => {
            products.push(new ProductItem(d));
        });

        return products;
    }
}