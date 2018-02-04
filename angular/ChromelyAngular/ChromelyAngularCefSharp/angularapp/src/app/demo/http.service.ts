import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';

@Injectable()
export class HttpService {

    private _dataUrl = './data/movies.json';

    constructor(private _http: HttpClient,) { }

    getData(): Observable<any> {
        return this._http.get<any>(this._dataUrl)
            .catch(this.handleError);
    }

    getInfo(url: string): Observable<any> {
        return this._http.get<any>(url)
            .catch(this.handleError);
    }

    /*
     * Http Get Request 
     */
    getMovies(url: string): Observable<any> {
        return this._http.get<any>(url)
            .catch(this.handleError);
    }

    postMovies(url: string, postData: any): Observable<any> {
        return this._http.post<any>(url, postData)
            .catch(this.handleError);
    }

    private handleError(err: HttpErrorResponse) {
        let errorMessage = '';
        if (err.error instanceof Error) {
            // A client-side or network error occurred. Handle it accordingly.
            errorMessage = `An error occurred: ${err.error.message}`;
        } else {
            // The backend returned an unsuccessful response code.
            // The response body may contain clues as to what went wrong,
            errorMessage = `Chromely returned code: ${err.status}, error message is: ${err.message}`;
        }
        console.error(errorMessage);
        return Observable.throw(errorMessage);
    }
}
