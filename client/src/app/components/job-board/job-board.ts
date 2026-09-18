import {Component, computed, OnInit, signal} from '@angular/core';
import {JobApplicationService} from '../../services/job-application.service';
import {
  APPLICATION_STATUSES,
  ApplicationStatus,
  JobApplication,
  UpdateJobApplicationRequest,
} from '../../models/job-application.model';
import {RouterLink} from '@angular/router';
import {JobCard} from '../job-card/job-card';

@Component({
  imports: [RouterLink, JobCard],
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

    this.jobApplicationService.update(application.id, request).subscribe(() => {
      this.jobApplications.update(list =>
        list.map(item => (item.id === application.id ? {...item, status} : item)),
      );
    });
  }

  onDelete(id: number): void {
    this.jobApplicationService.delete(id).subscribe(() => {
      this.jobApplications.update(list => list.filter(a => a.id !== id));
    });
  }

}
