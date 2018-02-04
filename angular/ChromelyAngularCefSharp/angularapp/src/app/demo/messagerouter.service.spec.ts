import { TestBed, inject } from '@angular/core/testing';

import { MessagerouterService } from './messagerouter.service';

describe('MessagerouterService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MessagerouterService]
    });
  });

  it('should be created', inject([MessagerouterService], (service: MessagerouterService) => {
    expect(service).toBeTruthy();
  }));
});
