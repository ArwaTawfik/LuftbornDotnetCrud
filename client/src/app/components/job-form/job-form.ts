import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { JobApplicationService } from '../../services/job-application.service';
import { CreateJobApplicationRequest } from '../../models/job-application.model';

@Component({
  imports: [ReactiveFormsModule, RouterLink],
  selector: 'app-job-form',
  styleUrl: './job-form.scss',
  templateUrl: './job-form.html',
})
export class JobForm {
  private readonly formBuilder = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly jobApplicationService = inject(JobApplicationService);

  readonly form = this.formBuilder.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.required, Validators.maxLength(2000)]],
    applicationStartDate: ['', Validators.required],
    applicationEndDate: [''],
  });

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { title, description, applicationStartDate, applicationEndDate } = this.form.getRawValue();
    const request: CreateJobApplicationRequest = {
      title,
      description,
      applicationStartDate,
      applicationEndDate: applicationEndDate || null,
    };

    this.jobApplicationService.create(request).subscribe(() => this.router.navigate(['/']));
  }
}
