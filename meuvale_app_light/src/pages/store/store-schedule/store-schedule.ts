import { Component } from '@angular/core';
import { IonicPage, NavParams, NavController } from 'ionic-angular';
import { UtilsProvider } from '../../../providers/utils/utils';
import { HttpProvider } from '../../../providers/http/http';
import { StoreProvider } from '../../../providers/store/store';

@IonicPage()
@Component({
  selector: 'page-store-schedule',
  templateUrl: 'store-schedule.html'
})
export class StoreSchedulePage {

  diaSelecionado: any;
  loja: any;

  diasSemana =
    [{
      Id: 0,
      Nome: 'Domingo',
      Abertura: '',
      Fechamento: '',
      Loja: { Id: 0 }
    },
    {
      Id: 1,
      Nome: 'Segunda',
      Abertura: '',
      Fechamento: '',
      Loja: { Id: 0 }
    },
    {
      Id: 2,
      Nome: 'Terca',
      Abertura: '',
      Fechamento: '',
      Loja: { Id: 0 }
    },
    {
      Id: 3,
      Nome: 'Quarta',
      Abertura: '',
      Fechamento: '',
      Loja: { Id: 0 }
    },
    {
      Id: 4,
      Nome: 'Quinta',
      Abertura: '',
      Fechamento: '',
      Loja: { Id: 0 }
    },
    {
      Id: 5,
      Nome: 'Sexta',
      Abertura: '',
      Fechamento: '',
      Loja: { Id: 0 }
    },
    {
      Id: 6,
      Nome: 'Sabado',
      Abertura: '',
      Fechamento: '',
      Loja: { Id: 0 }
    }];

  Segunda: boolean = false;
  Terca: boolean = false;
  Quarta: boolean = false;
  Quinta: boolean = false;
  Sexta: boolean = false;
  Sabado: boolean = false;
  Domingo: boolean = false;

  constructor(
    private utils: UtilsProvider,
    private navParams: NavParams,
    private storeProvider: StoreProvider,
    private navCtrl: NavController
  ) {

    this.loja = this.navParams.get("loja");
    this.diaSelecionado = { Abertura: '08:00', Fechamento: '18:00' };

    if (!HttpProvider.userAuth) {
      this.utils.showAlert('Atenção', 'Cadastre-se ou logue-se para ver as suas lojas.');
      this.navCtrl.setRoot("LoginPage");
      return;
    }
  }

  ionViewDidLoad() {
    if (this.loja)
      this.prepareForm(this.loja.HorarioFuncionamento);
  }


  dismiss() {
    this.navCtrl.setRoot("StoreRegisterPage", { loja: this.loja })
  }

  btnActivate(name) {
    if (name == 'seg') {
      this.Segunda = !this.Segunda;
      this.utils.showToast('Segunda-feira', 30);
    }
    if (name == 'ter') {
      this.Terca = !this.Terca;
      this.utils.showToast('Terça-feira', 30);
    }
    if (name == 'qua') {
      this.Quarta = !this.Quarta;
      this.utils.showToast('Quarta-feira', 30);
    }
    if (name == 'qui') {
      this.Quinta = !this.Quinta;
      this.utils.showToast('Quinta-feira', 30);
    }
    if (name == 'sex') {
      this.Sexta = !this.Sexta;
      this.utils.showToast('Sexta-feira', 30);
    }
    if (name == 'sab') {
      this.Sabado = !this.Sabado;
      this.utils.showToast('Sábado', 30);
    }
    if (name == 'dom') {
      this.Domingo = !this.Domingo;
      this.utils.showToast('Domingo', 30);
    }
  }

  validaHorario() {
    let horario = this.diaSelecionado.Abertura.split(':', 4);
    let horaabertura = horario[0];
    let minutoabertura = horario[1];

    horario = this.diaSelecionado.Fechamento.split(':', 4);
    let horafechamento = horario[0];
    let minutofechamento = horario[1];

    if ((horaabertura > 23 || minutoabertura > 59)
      || (horaabertura >= horafechamento && minutoabertura >= minutofechamento)
      || (horaabertura > horafechamento)) {
      this.utils.showAlert('Erro', 'Horario de abertura inválido');
      this.diaSelecionado.Abertura = '08:00';
      return false;
    }

    if ((horafechamento > 23 || minutofechamento > 59)
      || (horafechamento <= horaabertura && minutofechamento <= minutoabertura)
      || (horafechamento < horaabertura)) {
      this.utils.showAlert('Erro', 'Horario de fechamento inválido');
      this.diaSelecionado.Fechamento = '18:00';
      return false;
    }
  }

  add() {

    if (!this.validaHorario)
      return;

    if (this.Segunda) {
      let dia;
      if (this.diasSemana.find(x => x.Nome == 'Segunda')) {
        dia = this.diasSemana.find(x => x.Nome == 'Segunda');
      }
      dia.Abertura = this.diaSelecionado.Abertura;
      dia.Fechamento = this.diaSelecionado.Fechamento;
      dia.Nome = 'Segunda';
      this.Segunda = false;
    }
    if (this.Terca) {
      let dia;
      if (this.diasSemana.find(x => x.Nome == 'Terca')) {
        dia = this.diasSemana.find(x => x.Nome == 'Terca');
      }
      dia.Abertura = this.diaSelecionado.Abertura;
      dia.Fechamento = this.diaSelecionado.Fechamento;
      dia.Nome = 'Terca';
      this.Terca = false;
    }
    if (this.Quarta) {
      let dia;
      if (this.diasSemana.find(x => x.Nome == 'Quarta')) {
        dia = this.diasSemana.find(x => x.Nome == 'Quarta');
      }
      dia.Abertura = this.diaSelecionado.Abertura;
      dia.Fechamento = this.diaSelecionado.Fechamento;
      dia.Nome = 'Quarta';
      this.Quarta = false;
    }
    if (this.Quinta) {
      let dia;
      if (this.diasSemana.find(x => x.Nome == 'Quinta')) {
        dia = this.diasSemana.find(x => x.Nome == 'Quinta');
      }
      dia.Abertura = this.diaSelecionado.Abertura;
      dia.Fechamento = this.diaSelecionado.Fechamento;
      dia.Nome = 'Quinta';
      this.Quinta = false;
    }
    if (this.Sexta) {
      let dia;
      if (this.diasSemana.find(x => x.Nome == 'Sexta')) {
        dia = this.diasSemana.find(x => x.Nome == 'Sexta');
      }
      dia.Abertura = this.diaSelecionado.Abertura;
      dia.Fechamento = this.diaSelecionado.Fechamento;
      dia.Nome = 'Sexta';
      this.Sexta = false;
    }
    if (this.Sabado) {
      let dia;
      if (this.diasSemana.find(x => x.Nome == 'Sabado')) {
        dia = this.diasSemana.find(x => x.Nome == 'Sabado');
      }
      dia.Abertura = this.diaSelecionado.Abertura;
      dia.Fechamento = this.diaSelecionado.Fechamento;
      dia.Nome = 'Sabado';
      this.Sabado = false;
    }
    if (this.Domingo) {
      let dia;
      if (this.diasSemana.find(x => x.Nome == 'Domingo')) {
        dia = this.diasSemana.find(x => x.Nome == 'Domingo');
      }
      dia.Abertura = this.diaSelecionado.Abertura;
      dia.Fechamento = this.diaSelecionado.Fechamento;
      dia.Nome = 'Domingo';
      this.Domingo = false;
    }
  }

  edit(dia) {
    this.diaSelecionado = dia;
    this.Segunda = dia.Nome == 'Segunda';
    this.Terca = dia.Nome == 'Terca';
    this.Quarta = dia.Nome == 'Quarta';
    this.Quinta = dia.Nome == 'Quinta';
    this.Sexta = dia.Nome == 'Sexta';
    this.Sabado = dia.Nome == 'Sabado';
    this.Domingo = dia.Nome == 'Domingo';
  }

  exclude(dia) {
    let diaexcluido = this.diasSemana.find(x => x.Nome == dia.Nome);

    diaexcluido.Abertura = '';
    diaexcluido.Fechamento = '';
  }

  openStoreRegister() {
    this.navCtrl.setRoot("StoreRegisterPage", { loja: this.loja })
  }

  openDelivery() {
    this.navCtrl.push("StoreDeliveryPage", { loja: this.loja });
  }

  save() {
    this.utils.showLoader("Salvando dados do Estabelecimento");
    this.storeProvider.atualizarLoja(this.formToUserData())
      .subscribe(data => {
        this.utils.hideLoader();
        this.utils.showAlert("Sucesso", "Horário do Estabelecimento atualizado com sucesso!");
      }, error => {
        this.utils.hideLoader();
        this.utils.showToast(error.error);
      });
  }

  private formToUserData(): any {
    this.loja.HorarioFuncionamento = [
      {
        DiaDaSemana: 0,
        HoraInicio: this.diasSemana.find(x => x.Id == 0).Abertura,
        HoraFim: this.diasSemana.find(x => x.Id == 0).Fechamento,
        Loja: { Id: this.loja.Id }
      },
      {
        DiaDaSemana: 1,
        HoraInicio: this.diasSemana.find(x => x.Id == 1).Abertura,
        HoraFim: this.diasSemana.find(x => x.Id == 1).Fechamento,
        Loja: { Id: this.loja.Id }
      },
      {
        DiaDaSemana: 2,
        HoraInicio: this.diasSemana.find(x => x.Id == 2).Abertura,
        HoraFim: this.diasSemana.find(x => x.Id == 2).Fechamento,
        Loja: { Id: this.loja.Id }
      },
      {
        DiaDaSemana: 3,
        HoraInicio: this.diasSemana.find(x => x.Id == 3).Abertura,
        HoraFim: this.diasSemana.find(x => x.Id == 3).Fechamento,
        Loja: { Id: this.loja.Id }
      },
      {
        DiaDaSemana: 4,
        HoraInicio: this.diasSemana.find(x => x.Id == 4).Abertura,
        HoraFim: this.diasSemana.find(x => x.Id == 4).Fechamento,
        Loja: { Id: this.loja.Id }
      },
      {
        DiaDaSemana: 5,
        HoraInicio: this.diasSemana.find(x => x.Id == 5).Abertura,
        HoraFim: this.diasSemana.find(x => x.Id == 5).Fechamento,
        Loja: { Id: this.loja.Id }
      },
      {
        DiaDaSemana: 6,
        HoraInicio: this.diasSemana.find(x => x.Id == 6).Abertura,
        HoraFim: this.diasSemana.find(x => x.Id == 6).Fechamento,
        Loja: { Id: this.loja.Id }
      }
    ];

    return this.loja;
  }

  prepareForm(data: any): void {
    this.diasSemana.forEach(element => {
      let dia = element;
      let diaOrigem = data.find(x => x.DiaDaSemana == dia.Id);
      if (diaOrigem) {
        dia.Abertura = diaOrigem.HoraInicio;
        dia.Fechamento = diaOrigem.HoraFim;
      }
    });
  }

}
