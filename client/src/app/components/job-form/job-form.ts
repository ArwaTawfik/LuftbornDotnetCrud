import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { JobApplicationService } from '../../services/job-application.service';
import { CreateJobApplicationRequest, JobApplication } from '../../models/job-application.model';
import { endNotBeforeStart } from '../../validators/date-range.validator';

@Component({
  imports: [ReactiveFormsModule, RouterLink],
  selector: 'app-job-form',
  styleUrl: './job-form.scss',
  templateUrl: './job-form.html',
})
export class JobForm implements OnInit {
  private readonly formBuilder = inject(FormBuilder);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly jobApplicationService = inject(JobApplicationService);

  private readonly id = this.route.snapshot.paramMap.get('id');
  private existing: JobApplication | null = null;

  readonly isEdit = this.id !== null;

  readonly form = this.formBuilder.nonNullable.group(
    {
      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.maxLength(2000)]],
      applicationStartDate: ['', Validators.required],
      applicationEndDate: [''],
    },
    { validators: endNotBeforeStart },
  );

  ngOnInit(): void {
    if (!this.isEdit) {
      return;
    }

    this.jobApplicationService.getById(Number(this.id)).subscribe({
      next: application => {
        this.existing = application;
        this.form.patchValue({
          title: application.title,
          description: application.description,
          applicationStartDate: application.applicationStartDate.slice(0, 10),
          applicationEndDate: application.applicationEndDate?.slice(0, 10) ?? '',
        });
      },
      error: () => this.goToBoard(),
    });
  }

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

    if (this.isEdit) {
      this.update(request);
    } else {
      this.create(request);
    }
  }

  private create(request: CreateJobApplicationRequest): void {
    this.jobApplicationService.create(request).subscribe(() => this.goToBoard());
  }

  private update(request: CreateJobApplicationRequest): void {
    if (!this.existing) {
      return;
    }

    this.jobApplicationService
      .update(this.existing.id, { ...request, status: this.existing.status })
      .subscribe(() => this.goToBoard());
  }

  private goToBoard(): void {
    this.router.navigate(['/']);
  }
}
