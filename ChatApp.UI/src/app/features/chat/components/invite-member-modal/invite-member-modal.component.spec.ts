import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InviteMemberModalComponent } from './invite-member-modal.component';

describe('InviteMemberModalComponent', () => {
  let component: InviteMemberModalComponent;
  let fixture: ComponentFixture<InviteMemberModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InviteMemberModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InviteMemberModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
