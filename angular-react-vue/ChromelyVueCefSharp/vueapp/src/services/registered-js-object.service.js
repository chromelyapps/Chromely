 
  export function boundObjectGet(url, parameters, callback) {
    boundObjectGetJson(url, parameters, response => {
        var jsonData = JSON.parse(response.ResponseText);
        if (jsonData.ReadyState == 4 && jsonData.Status == 200) {
          callback(jsonData.Data);
        } else {
          console.log("An error occurs during message routing. With ur:" + url + ". Response received:" + response);
        }
    });
  }
  
  export function boundObjectPost(url, parameters, postData, callback) {
    boundObjectPostJson(url, parameters, postData, response => {
        var jsonData = JSON.parse(response.ResponseText);
        if (jsonData.ReadyState == 4 && jsonData.Status == 200) {
           callback(jsonData.Data);
         } else {
           console.log("An error occurs during message routing. With ur:" + url + ". Response received:" + response);
         }
     });
  }

/* 
 * Get request for RegisterAsyncJsObject
 * https://github.com/mattkol/Chromely/wiki/Expose-.NET-class-to-JavaScript
 * url - Chromely route path
 * request - a Json object
 * response - callback response method
 */
function boundObjectGetJson(url, parameters, response) {
    boundControllerAsync.getJson(url, parameters, response);
}

/* 
 * Get request for RegisterAsyncJsObject
 * https://github.com/mattkol/Chromely/wiki/Expose-.NET-class-to-JavaScript
 * url - Chromely route path
 * request - a Json object
 * response - callback response method
 */
function boundObjectPostJson(url, parameters, postData, response) {
    boundControllerAsync.postJson(url, parameters, postData, response);
}