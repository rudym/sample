import { TestBed, inject } from '@angular/core/testing';

import { CustomersApiService } from './customers-api.service';

describe('CustomersApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CustomersApiService]
    });
  });

  it('should be created', inject([CustomersApiService], (service: CustomersApiService) => {
    expect(service).toBeTruthy();
  }));
});
