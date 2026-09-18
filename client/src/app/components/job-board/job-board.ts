import {Component, OnInit, signal} from '@angular/core';
import {JobApplicationService} from '../../services/job-application.service';
import {JobApplication} from '../../models/job-application.model';
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

  constructor(private jobApplicationService: JobApplicationService) {
  }

  ngOnInit(): void {
    this.jobApplicationService.getAll().subscribe(applications => {
      this.jobApplications.set(applications);
    });
  }

  onDelete(id: number): void {
    this.jobApplicationService.delete(id).subscribe(() => {
      this.jobApplications.update(list => list.filter(a => a.id !== id));
    });
  }

}
