import { TestBed, inject } from '@angular/core/testing';

import { RegisteredJsObjectService } from './registered-js-object.service';

describe('RegisteredJsObjectService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RegisteredJsObjectService]
    });
  });

  it('should be created', inject([RegisteredJsObjectService], (service: RegisteredJsObjectService) => {
    expect(service).toBeTruthy();
  }));
});
