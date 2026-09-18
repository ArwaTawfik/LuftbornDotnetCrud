import { DatePipe } from '@angular/common';
import { Component, computed, input, output } from '@angular/core';
import { RouterLink } from '@angular/router';
import { APPLICATION_STATUSES, ApplicationStatus, JobApplication } from '../../models/job-application.model';

@Component({
  imports: [DatePipe, RouterLink],
  selector: 'app-job-card',
  styleUrl: './job-card.scss',
  templateUrl: './job-card.html',
})
export class JobCard {
  readonly statuses = APPLICATION_STATUSES;

  application = input.required<JobApplication>();
  delete = output<number>();
  statusChange = output<ApplicationStatus>();

  statusClass = computed(() => `pill pill--${this.application().status.toLowerCase()}`);

  onDelete(): void {
    this.delete.emit(this.application().id);
  }

  onStatusChange(event: Event): void {
    const status = (event.target as HTMLSelectElement).value as ApplicationStatus;
    this.statusChange.emit(status);
  }
}
