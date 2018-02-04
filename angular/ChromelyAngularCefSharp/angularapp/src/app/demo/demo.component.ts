import { Component, OnInit, Output, NgZone } from '@angular/core';
import { Location } from '@angular/common';
import { HttpService } from './http.service';
import { IInfo } from './info';
import { RegisteredJsObjectService } from './registered-js-object.service';

@Component({
  selector: 'app-demo',
  templateUrl: './demo.component.html',
  styleUrls: ['./demo.component.css']
})
export class DemoComponent implements OnInit {
    @Output() info: IInfo;

    @Output() moviesCefQueryLocalAssembly: any;
    @Output() moviesCefQueryExternalAssembly: any;
    @Output() postCefQueryResult: string;

    @Output() moviesHttpLocalAssembly: any;
    @Output() moviesHttpExternalAssembly: any;
    @Output() postHttpResult: string;

    postData: any;

    constructor(private _registeredJsObjectService: RegisteredJsObjectService,
                private _httpService: HttpService,
                private _zone: NgZone,
                private _location: Location) {

        this.info = <IInfo>{};

        // Initialize info
        this.info.objective = 'Chromely Main Objectives';
        this.info.platform = 'Platforms';
        this.info.version = 'Version';
    }

     /*
     * Navigate back to home page
     */
    goBack(): void {
      this._location.back();
    }

    /*
     * Regsitered JS Object Requests 
     */
    regsiteredJSObjectRequests(type: string, url: string): void {
        switch (type) {
            case "getlocal":
              this._registeredJsObjectService.boundJsObjectGetRequest(url, null, data => {
                    this._zone.run(
                        () => {
                            this.moviesCefQueryLocalAssembly = data;
                        })
                });
                break;
            case "getexternal":
              this._registeredJsObjectService.boundJsObjectGetRequest(url, null, data => {
                    this._zone.run(
                        () => {
                            this.moviesCefQueryExternalAssembly = data;
                        })
                });
                break;
            case "post":
              this._registeredJsObjectService.boundJsObjectPostRequest(url, null, this.postData, result => {
                    this._zone.run(
                        () => {
                            this.postCefQueryResult = result;
                        })
                });
                break;
            default:
                console.log('No valid cefQuery request found');
        }
    }

    /*
     * Http Requests 
     */
    httpRequest(type: string, url: string): void {
        switch (type) {
            case "getlocal":
                this._httpService.getMovies(url).subscribe(data => {
                    this.moviesHttpLocalAssembly = data;
                });
                break;
            case "getexternal":
                this._httpService.getMovies(url).subscribe(data => {
                    this.moviesHttpExternalAssembly = data;
                });
                break;
            case "post":
                this._httpService.postMovies(url, this.postData)
                    .subscribe(result => {
                        this.postHttpResult = result['Data'];
                    });
                break;
            default:
                console.log('No valid http request found');
        }
    }

    ngOnInit() {
        /*
         * 
         */
        this._httpService.getData().subscribe(data => {
            this.postData = data;
        });

        /*
         * 
         */ 
        this._httpService.getInfo('http://chromely.com/info').subscribe(data => {
            this.info.objective = data['divObjective'];
            this.info.platform = data['divPlatform'];
            this.info.version = data['divVersion'];
        });
  }
}
