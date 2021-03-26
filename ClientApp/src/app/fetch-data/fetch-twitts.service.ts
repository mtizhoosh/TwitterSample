import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable, of, throwError } from 'rxjs';
import { catchError, tap, map, shareReplay } from 'rxjs/operators';
import { Moment } from 'moment';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root'
})
export class FetchTwittsService {

  private twittUrl = '';
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.twittUrl = baseUrl + 'api/TwitterFeed';
    this.sampleTwittes$ = this.http.get<TwittStatistics>(this.twittUrl).pipe(
      tap(t => console.log('From service:twitt feeds \n' + JSON.stringify(t))),
      catchError(this.handleError)
    );
  }

  // "http://localhost:64903/TwitterFeed"
  sampleTwittes$: Observable<TwittStatistics>;

  getTwitts(url: string): Observable<TwittStatistics> {
    return this.http.get<TwittStatistics>(url)
      .pipe(
        tap(data => console.log(JSON.stringify(data))),
        catchError(this.handleError)
      );
  }

  private handleError(err: any) {
    // in a real world app, we may send the server to some remote logging infrastructure
    // instead of just logging it to the console
    let errorMessage: string;
    if (err.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      errorMessage = `Backend returned code ${err.status}: ${err.body.error}`;
    }
    console.error(err);
    return throwError(errorMessage);
  }

}



export interface ITwitInfo {
  text: string;
  created_at: string;
  author_id: string;
  id: string;
}

export interface ITwittStatistics {
  twitts: ITwitInfo[];
  total: number;
  averagePerMinute: number;
}
 
export class TwittStatistics implements ITwittStatistics {
  twitts: ITwitInfo[];
  total: number;
  averagePerMinute: number;
  _now: Moment;

  constructor(t: ITwittStatistics) {
    this.twitts = t.twitts;
    this.total = t.total;
    this.averagePerMinute = t.averagePerMinute;

    const DATE_TIME_FORMAT = 'YYYY-MM-DDTHH:mm';
    this._now = moment(new Date(), DATE_TIME_FORMAT);
  }

}
