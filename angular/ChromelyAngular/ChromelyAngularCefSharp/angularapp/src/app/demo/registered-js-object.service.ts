import { Injectable } from '@angular/core';

declare var boundObjectGetJson: any;
declare var boundObjectPostJson: any;

@Injectable()
export class RegisteredJsObjectService {

  boundJsObjectGetRequest(url: string, parameters: any, callback: Function): void {
      boundObjectGetJson(url, parameters, response => {
          var jsonData = JSON.parse(response.ResponseText);
          if (jsonData.ReadyState == 4 && jsonData.Status == 200) {
            callback(jsonData.Data);
          } else {
            console.log("An error occurs during message routing. With ur:" + url + ". Response received:" + response);
          }
      });
  }

  boundJsObjectPostRequest(url: string, parameters: any, postData: any, callback: Function): void {
      boundObjectPostJson(url, parameters, postData, response => {
         var jsonData = JSON.parse(response.ResponseText);
         if (jsonData.ReadyState == 4 && jsonData.Status == 200) {
            callback(jsonData.Data);
          } else {
            console.log("An error occurs during message routing. With ur:" + url + ". Response received:" + response);
          }
      });
  }
}
