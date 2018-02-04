/* 
 * CEF Generic Message Routing
 * https://github.com/mattkol/Chromely/wiki/Generic-Message-Routing
 * request - a Json object
 * response - callback response method
 * onError - callback on erorr or exception.
 */
function messsageRouterQuery(request, response, onError) {
    window.cefQuery({
        request: JSON.stringify(request),
        onSuccess: function (data) {
            response(data);
        }, onFailure: function (err, msg) {
            console.log(err, msg);
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