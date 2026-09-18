import {Component, computed, OnInit, signal} from '@angular/core';
import {JobApplicationService} from '../../services/job-application.service';
import {
  APPLICATION_STATUSES,
  ApplicationStatus,
  JobApplication,
  UpdateJobApplicationRequest,
} from '../../models/job-application.model';
import {RouterLink} from '@angular/router';
import {CdkDrag, CdkDragDrop, CdkDropList, CdkDropListGroup} from '@angular/cdk/drag-drop';
import {JobCard} from '../job-card/job-card';

@Component({
  imports: [RouterLink, JobCard, CdkDropListGroup, CdkDropList, CdkDrag],
  selector: 'app-job-board',
  styleUrl: './job-board.scss',
  templateUrl: './job-board.html',
})
export class JobBoard implements OnInit {

  jobApplications = signal<JobApplication[]>([]);

  columns = computed(() =>
    APPLICATION_STATUSES.map(status => ({
      status,
      slug: status.toLowerCase(),
      applications: this.jobApplications().filter(application => application.status === status),
    })),
  );

  constructor(private jobApplicationService: JobApplicationService) {
  }

  ngOnInit(): void {
    this.jobApplicationService.getAll().subscribe(applications => {
      this.jobApplications.set(applications);
    });
  }

  onStatusChange(application: JobApplication, status: ApplicationStatus): void {
    const request: UpdateJobApplicationRequest = {
      title: application.title,
      description: application.description,
      applicationStartDate: application.applicationStartDate,
      applicationEndDate: application.applicationEndDate,
      status,
    };

    const previousStatus = application.status;
    this.setStatus(application.id, status);

    this.jobApplicationService.update(application.id, request).subscribe({
      error: () => this.setStatus(application.id, previousStatus),
    });
  }

  onDrop(event: CdkDragDrop<ApplicationStatus, ApplicationStatus, JobApplication>): void {
    if (event.previousContainer === event.container) {
      return;
    }
    this.onStatusChange(event.item.data, event.container.data);
  }

  private setStatus(id: number, status: ApplicationStatus): void {
    this.jobApplications.update(list =>
      list.map(item => (item.id === id ? {...item, status} : item)),
    );
  }

  onDelete(id: number): void {
    this.jobApplicationService.delete(id).subscribe(() => {
      this.jobApplications.update(list => list.filter(a => a.id !== id));
    });
  }

}
