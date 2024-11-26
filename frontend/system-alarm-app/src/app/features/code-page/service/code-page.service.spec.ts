import { TestBed } from '@angular/core/testing';

import { CodePageService } from './code-page.service';

describe('CodePageService', () => {
  let service: CodePageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CodePageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
