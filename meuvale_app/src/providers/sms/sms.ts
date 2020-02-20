import { Injectable } from '@angular/core';
import { Response, Http } from "@angular/http";
import { Observable } from 'rxjs';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class SmsProvider {

    private smsUrl: string = 'https://sms.comtele.com.br/api/v2/send';
    private key: string = 'c1a80a9e-2f71-487d-90de-ea845256581c';//'&key=AIzaSyDStwPWvpxHIHraTgrVMB-4otJuIBc3JVo';

    constructor(private http: Http) { };

    sendSMS(sender: string, content: string, receiver: number) {
        // const TextMessageService = require('comtele-sdk').TextMessageService;

        // const apiKey = 'c1a80a9e-2f71-487d-90de-ea845256581c';
        // var textMessageService = new TextMessageService(apiKey);
        // textMessageService.send(sender, content, [receiver], data => console.log(data));

        // var request = require("request");

        // var options = {
        //     method: 'POST',
        //     url: 'https://sms.comtele.com.br/api/v2/send',
        //     headers: { 'content-type': 'application/json', 'auth-key': 'c1a80a9e-2f71-487d-90de-ea845256581c' },
        //     qs: {Sender: sender, Receivers: '11989062645', Content: content}
        // };

        // request(options, function (error, response, body) {
        //     if (error) throw new Error(error);

        //     console.log(body);
        // });
    }

}