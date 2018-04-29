<template>
  <div>
    <div class="col-12">
        <div class="centerBlock">
            <div class="row">
               <div class="col">
                    <img src="./assets/img/chromely.png" class="img-rounded" alt="Chromely Logo" width="120" height="120" style='margin-top: 20px;' />
               </div>
               <div class="col">
                    <img src="./assets/img/logo.png" class="img-rounded" alt="Vue Logo" width="120" height="120" style='margin-top: 20px;' />
               </div>
            </div>
            <div class="row centerBlock">
                <span class="text-primary text-center"><h2>demo panel</h2></span>
            </div>
        </div>
    </div>

    <div class="col-12">
      <div>
        <router-link to="/"> <button id="buttonDemoRun" type="button" class="btn btn-link" style='margin: 5px;'>Back</button></router-link>
      </div>
      <div class="centerBlock">
        <button v-b-modal.boundJsObjectModal type="button" class="btn btn-light" style='margin: 5px;'>RegisterAsyncJsObject Demo</button>
        <a href="https://github.com/mattkol/Chromely" class="btn btn-default" role="button" style='margin: 5px;'>more info</a>
      </div>
    </div>

    <br>

    <div id="infoPanel" class="col-12 centerBlock">

      <div>
        <div class="card-header card bg-primary text-white">Chromely Main objective</div>
        <div class="card-body"> {{info.objective}}  </div>
      </div>
      <br>

      <div>
        <div class="card-header card bg-primary text-white">Platforms</div>
        <div class="card-body"> {{info.platform}} </div>
      </div>
      <br>

      <div>
        <div class="card-header card bg-primary text-white">Current CefSharp/Chromium Version</div>
        <div class="card-body"> {{info.version}} </div>
      </div>
      <br>

    </div>

    <!-- Bounds Object Modal -->
    <b-modal id="boundJsObjectModal" size="lg" title=".NET/JavaScript Integration (RegisterAsyncJsObject) Demo">
        <b-tabs pills>
            <b-tab title="Get1" active>
              <div class="row">
                <div class="col-12">
                    Route Path:&ensp;/democontroller/movies &ensp; (Restful Service in Local Assembly)&ensp;<button type="button" class="btn btn-primary btn-sm" v-on:click="boundObjectGet1Click()">Run</button>
                </div>
                <br><br>
                <div class="col-12">
                    <div class='table-responsive'>
                        <table id="boundObjectResult1" class='table'>
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Title</th>
                                    <th>Year</th>
                                    <th>Votes</th>
                                    <th>Rating</th>
                                    <th>Date</th>
                                    <th>RestfulAssembly</th>
                                </tr>
                            </thead>
                            <tbody tr v-for="item in boundObjectGet1Result">
                                <tr>
                                    <th>{{ item.Id }}</th>
                                    <th>{{ item.Title }}</th>
                                    <th>{{ item.Year }}</th>
                                    <th>{{ item.Votes }}</th>
                                    <th>{{ item.Rating }}</th>
                                    <th>{{ item.Date }}</th>
                                    <th>{{ item.RestfulAssembly }}</th>
                                </tr>
                            </tbody>
                       </table>
                    </div>
                </div>
              </div>
            </b-tab>
            <b-tab title="Get2">
              <div class="row">
                <div class="col-12">
                    Route Path:&ensp;/externalcontroller/movies &ensp; (Restful Service in External Assembly)&ensp;<button type="button" class="btn btn-primary btn-sm" v-on:click="boundObjectGet2Click()">Run</button>
                </div>
                <br><br>
                <div class="col-12">
                    <div class='table-responsive'>
                        <table id="boundObjectResult1" class='table'>
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Title</th>
                                    <th>Year</th>
                                    <th>Votes</th>
                                    <th>Rating</th>
                                    <th>Date</th>
                                    <th>RestfulAssembly</th>
                                </tr>
                            </thead>
                           <tbody tr v-for="item in boundObjectGet2Result">
                                <tr>
                                    <th>{{ item.Id }}</th>
                                    <th>{{ item.Title }}</th>
                                    <th>{{ item.Year }}</th>
                                    <th>{{ item.Votes }}</th>
                                    <th>{{ item.Rating }}</th>
                                    <th>{{ item.Date }}</th>
                                    <th>{{ item.RestfulAssembly }}</th>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
              </div>
            </b-tab>
            <b-tab title="Post">
               <div class="row">
                  <div class="col-12">
                        Route Path:&ensp;/democontroller/savemovies&ensp;(Restful Service in Local Assembly)&ensp;<button type="button" class="btn btn-primary btn-sm" v-on:click="boundObjectPostClick()">Run</button>
                    </div>
                    <br><br>
                    <div class="col-12">
                        <div>{{ boundObjectPostResult }}</div>
                    </div>
                </div>
            </b-tab>
        </b-tabs>

    </b-modal>
    <!-- End Bounds Object Modal -->

  </div>
</template>

<script>
  var boundObjectService = require('./services/registered-js-object.service');
  export default {
      data () {
        return {
            info: {},
            boundObjectGet1Result: [],
            boundObjectGet2Result: [],
            boundObjectPostResult: 'Post request not ran or no result recieved.'
        }
     },
    methods: {

        /* Start -  Event Handling */

        boundObjectGet1Click: function () {
           boundObjectService.boundObjectGet('/democontroller/movies', null, this.boundObjectGet1Callback)
        },

        boundObjectGet2Click: function () {
           boundObjectService.boundObjectGet('/externalcontroller/movies', null, this.boundObjectGet2Callback)
        },

        boundObjectPostClick: function () {
              var moviesJson = [
                { Id: 1, Title: "The Shawshank Redemption", Year: 1994, Votes: 678790, Rating: 9.2 },
                { Id: 2, Title: "The Godfather", Year: 1972, votes: 511495, Rating: 9.2 },
                { Id: 3, Title: "The Godfather: Part II", Year: 1974, Votes: 319352, Rating: 9.0 },
                { Id: 4, Title: "The Good, the Bad and the Ugly", Year: 1966, Votes: 213030, Rating: 8.9 },
                { Id: 5, Title: "My Fair Lady", Year: 1964, Votes: 533848, Rating: 8.9 },
                { Id: 6, Title: "12 Angry Men", Year: 1957, Votes: 164558, Rating: 8.9 }
            ];

            boundObjectService.boundObjectPost('/democontroller/savemovies', null, moviesJson, this.boundObjectPostCallback)
        },

        /* End - Event Handling */
        
        boundObjectInfoCallback: function (res) {

            var tempInfo = { objective: '', platform: '', version: '' }
            tempInfo.objective = res.divObjective
            tempInfo.platform = res.divPlatform
            tempInfo.version = res.divVersion

            this.info = tempInfo
        },

        boundObjectGet1Callback: function (res) {
            var dataArray = this.parseArrayResult(res)
            this.boundObjectGet1Result = dataArray
        },

        boundObjectGet2Callback: function (res) {
            var dataArray = this.parseArrayResult(res)
            this.boundObjectGet2Result = dataArray
        },

        boundObjectPostCallback: function (res) {
           this.boundObjectPostResult = res
        },

        boundObjectInfo: function() {
           boundObjectService.boundObjectGet('/info', null, this.boundObjectInfoCallback)
        },

        parseArrayResult: function(data) {
            var dataArray = [];

            for (var i = 0; i < data.length; i++) {
                var tempItem = {
                    Id: data[i].Id,
                    Title: data[i].Title,
                    Votes: data[i].Votes,
                    Year: data[i].Year,
                    Votes: data[i].Votes,
                    Rating: data[i].Rating,
                    Date: data[i].Date,
                    RestfulAssembly: data[i].RestfulAssembly
                };

                dataArray.push(tempItem);
            }

            return dataArray;
        }
    },
    beforeMount(){
        this.boundObjectInfo()
    }
  }
</script>