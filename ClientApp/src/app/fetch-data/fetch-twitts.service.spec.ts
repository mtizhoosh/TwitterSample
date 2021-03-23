import { TestBed } from '@angular/core/testing';

import { FetchTwittsService } from './fetch-twitts.service';

describe('FetchTwittsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FetchTwittsService = TestBed.get(FetchTwittsService);
    expect(service).toBeTruthy();
  });
});
