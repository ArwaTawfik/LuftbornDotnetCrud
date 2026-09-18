import { DatePipe } from '@angular/common';
import { Component, computed, input, output } from '@angular/core';
import { JobApplication } from '../../models/job-application.model';

@Component({
  imports: [DatePipe],
  selector: 'app-job-card',
  styleUrl: './job-card.scss',
  templateUrl: './job-card.html',
})
export class JobCard {
  application = input.required<JobApplication>();
  delete = output<number>();

  statusClass = computed(() => `pill pill--${this.application().status.toLowerCase()}`);

  onDelete(): void {
    this.delete.emit(this.application().id);
  }
}
