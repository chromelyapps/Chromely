import { Injectable } from '@angular/core';
import { IResponse } from './response';

declare var messsageRouterQuery: any;

@Injectable()
export class MessagerouterService {

    constructor() { }

    cefQueryGetRequest(url: string, parameters: any, callback: Function): void {
        var request = {
            "method": "GET",
            "url": url,
            "parameters": parameters,
            "postData": null,
        };

        messsageRouterQuery(
            request,
            response => {
                var jsonData = JSON.parse(response);
                if (jsonData.ReadyState == 4 && jsonData.Status == 200) {
                    callback(jsonData.Data);
                } else {
                    console.log("An error occurs during message routing. With ur:" + url + ". Response received:" + response);
                }
            },
            this.cefQueryOnError);;
    }

    cefQueryPostRequest(url: string, parameters: any, postData: any, callback: Function): void {
        var request = {
            "method": "POST",
            "url": url,
            "parameters": parameters,
            "postData": postData,
        };

        messsageRouterQuery(
            request,
            response => {
                var jsonData = JSON.parse(response);
                if (jsonData.ReadyState == 4 && jsonData.Status == 200) {
                    callback(jsonData.Data);
                } else {
                    console.log("An error occurs during message routing. With ur:" + url + ". Response received:" + response);
                }
            },
            this.cefQueryOnError);
    }

    cefQueryResponse(url: string, data: any, callback: Function): void {
        var jsonData = JSON.parse(data);

        if (jsonData.ReadyState == 4 && jsonData.Status == 200) {
            callback(jsonData.Data);
        } else {
            console.log("An error occurs during message routing. With ur:" + url + ". Response received:" + data);
        }
    }

    cefQueryOnError(err: any, msg: string): void {
        console.log(err, msg);
    }
}
