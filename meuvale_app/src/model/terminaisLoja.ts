export class TerminaisLoja {
    Id: number;
    Loja: { Id: number };
    Usuario: { Id: string };
    Terminal: Terminal;
}

export class Terminal {
        Id: number;
        Maquininha: string;
        Modelo: string;
        NumeroSerial: string;
        NomeGerenciadora: string;
        SoftwareHouse: string;
        TaxaDebito: number;
        TaxaCredito: number;
}