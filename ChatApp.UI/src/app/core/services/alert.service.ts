import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  // private alerts: Alert[] = [];
  // private alertSubject = new BehaviorSubject<Alert[]>([]);
  // alert$ = this.alertSubject.asObservable();
  // constructor() {}
  // createAlert(alert: Alert): void {
  //   this.alerts.unshift(alert);
  //   if (this.alerts.length > 5) {
  //     this.alerts.pop();
  //   }
  //   this.alertSubject.next([...this.alerts]);
  //   setTimeout(() => {
  //     this.dismissAlert(alert);
  //   }, 5000);
  // }
  // dismissAlert(alert: Alert): void {
  //   this.alerts = this.alerts.filter((a) => a !== alert);
  //   this.alertSubject.next([...this.alerts]);
  // }
}
