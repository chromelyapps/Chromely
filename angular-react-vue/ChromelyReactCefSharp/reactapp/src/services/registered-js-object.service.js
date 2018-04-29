
module.exports = {
     boundObjectInfoTemp: function() {
      console.log('External - info');
      return { objective: 'External - Chromely Main Objectives', platform: 'External - Platforms', version: 'External - Version' }
  },

    /* 
  * Get request for RegisterAsyncJsObject
  * https://github.com/mattkol/Chromely/wiki/Expose-.NET-class-to-JavaScript
  * url - Chromely route path
  * request - a Json object
  * response - callback response method
  */
  boundObjectGet: function(url, parameters, response) {
    boundControllerAsync.getJson(url, parameters, response);
  },

  /* 
  * Get request for RegisterAsyncJsObject
  * https://github.com/mattkol/Chromely/wiki/Expose-.NET-class-to-JavaScript
  * url - Chromely route path
  * request - a Json object
  * response - callback response method
  */
  boundObjectPost: function(url, parameters, postData, response) {
    boundControllerAsync.postJson(url, parameters, postData, response);
  }

}


