import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FetchTwittsService, ITwitInfo, ITwittStatistics, TwittStatistics } from './fetch-twitts.service';
import { BehaviorSubject, EMPTY, interval, merge, Observable, of, Subject, Subscription, throwError, timer } from 'rxjs';
import { catchError, concat, concatMap, delay, map, mergeAll, skip, switchMap, take, tap, throttleTime } from 'rxjs/operators';
import { Moment } from 'moment';
import * as moment from 'moment';


@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit, OnDestroy {

  errorMessage = '';
  public twitData: TwittStatistics = { twitts:[], total:0, averagePerMinute:0, _now:moment()};


  twittRefresh$: Observable<TwittStatistics>;
  twittFeeds$: Observable<TwittStatistics>;
  subscribtion: Subscription;

  trigger$ = timer(0, 2000);

  public sampleTwitts$ = this.feedService.sampleTwittes$.pipe(
    map(t => this.twitData = t),
    catchError(err => {
      this.errorMessage = err;
      return EMPTY;
    })
  );

  load$ = new BehaviorSubject('');
  userRefresh = new Subject<number>();
  _now: Moment;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private feedService: FetchTwittsService) {

  }

  refreshStat() {
    this.userRefresh.next(1);
  }

  ngOnInit(): void {

    
    // one time http get by button click event
    this.twittRefresh$ = this.userRefresh.pipe(
      concatMap(_ => this.feedService.sampleTwittes$),
      tap(t => console.log(JSON.stringify(t))),
      map(f => this.twitData = f)
    );

    this.twittRefresh$.subscribe(d => this.twitData = d);

    
    this.twittFeeds$ = timer(0, 5000).pipe(
      concatMap(_ => this.feedService.sampleTwittes$),
      tap(t => console.log('Twitter feeds (from component) \n' + JSON.stringify(t))),
      map(t => new TwittStatistics(t))
    );
    
    this.subscribtion = this.twittFeeds$.subscribe(result => 
      this.twitData = result,
      err => this.errorMessage = 'Recieved an error from ',
      () => this.subscribtion.unsubscribe()
    );
      
  }

  ngOnDestroy(): void {
    this.subscribtion.unsubscribe();
  }


}

