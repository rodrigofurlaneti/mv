export const MENU_LIST_OPEN: NavItem[] = [

    { title: 'Login', icon: "log-in", page: "LoginPage" },
    { title: 'Tutorial', icon: "book", page: "TutorialPage" },
    { title: 'Contato', icon: "chatboxes", page: "ContactPage" }

];

//1 - Consumidor
export const MENU_LIST_CONSUMIDOR: NavItem[] = [
    { title: 'Meu Vale', icon: "home", page: "MainHomePage" },
    { title: 'Meus Dados', icon: "contact", page: "ClientRegisterSimplePage" },
    { title: 'Cartões', icon: "card", page: "CartaoListPage" },
    { title: 'Minhas Compras', icon: "filing", page: "OrderListPage" },
    { title: 'Indique um Estabelecimento', icon: "appstore", page: "StoreIndicatePage" },
    { title: 'Contato', icon: "chatboxes", page: "ContactPage" },
    { title: 'Tutorial', icon: "book", page: "TutorialPage" },
    { title: 'Sobre', icon: "information-circle", page: "AboutPage" },
    { title: 'Sair', icon: 'log-out', page: null }
];

//2 - Lojista
export const MENU_LIST_LOJISTA: NavItem[] = [
    { title: 'Principal', icon: "home", page: "MainStorePage" },
    { title: 'Lêr Voucher', icon: "barcode", page: "OrderWithdrawPage" },
    { title: 'Contato', icon: "chatboxes", page: "ContactPage" },
    { title: 'Sobre', icon: "information-circle", page: "AboutPage" },
    { title: 'Sair', icon: 'log-out', page: null }
];

//3 - Parceiro
export const MENU_LIST_PARCEIRO: NavItem[] = [
    { title: 'Parceiros', icon: "home", page: "MainPartnerPage" },
    { title: 'Sobre', icon: "information-circle", page: "AboutPage" },
    { title: 'Sair', icon: 'log-out', page: null }
];

//99 - Administrador --MENU_LIST_ADMIN
export const MENU_LIST_ADMIN: NavItem[] = [
    { title: 'Cockpit', icon: "home", page: "MainAdminPage" },
    { title: 'Visão Cliente', icon: "home", page: "MainHomePage" },
    { title: 'Visão Lojista', icon: "home", page: "MainStorePage" },
    { title: 'Categoria', icon: "reorder", page: "CategoryRegisterPage" },
    { title: 'Departamento', icon: "reorder", page: "DepartamentRegisterPage" },
    { title: 'Indique um Estabelecimento', icon: "appstore", page: "StoreIndicatePage" },
    { title: 'Contato', icon: "chatboxes", page: "ContactPage" },
    { title: 'Meus Dados', icon: "contact", page: "ClientRegisterSimplePage" },
    { title: 'Cartões', icon: "card", page: "CartaoListPage" },
    { title: 'Minhas Compras', icon: "filing", page: "OrderListPage" },
    { title: 'Tutorial', icon: "book", page: "TutorialPage" },
    { title: 'Sobre', icon: "information-circle", page: "AboutPage" },
    { title: 'Sair', icon: 'log-out', page: null }
];

//FIRTS LOGIN
export const MENU_LIST_FIRST_LOGIN: NavItem[] = [
    { title: 'Contato', icon: "chatboxes", page: "ContactPage" },
    { title: 'Sobre', icon: "information-circle", page: "AboutPage" },
    { title: 'Sair', icon: 'log-out', page: null }
];

//4 - Agente LOGIN
export const MENU_LIST_AGENTE: NavItem[] = [
    { title: 'Principal', icon: "appstore", page: "MainAgentePage" },
    { title: 'Cad. Estabelecimento', icon: "appstore", page: "StoreRegisterFull" },
    { title: 'Contato', icon: "chatboxes", page: "ContactPage" },
    { title: 'Sobre', icon: "information-circle", page: "AboutPage" },
    { title: 'Sair', icon: 'log-out', page: null }
];

interface NavItem {
    title: string;
    icon: string;
    page: string;
}