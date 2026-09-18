export type ApplicationStatus = 'Applied' | 'Interviewing' | 'Offer' | 'Rejected' | 'Withdrawn';

export interface JobApplication {
  id: number;
  title: string;
  description: string;
  applicationStartDate: string;
  applicationEndDate: string | null;
  status: ApplicationStatus;
}

export interface CreateJobApplicationRequest {
  title: string;
  description: string;
  applicationStartDate: string;
  applicationEndDate: string | null;
}

export interface UpdateJobApplicationRequest {
  title: string;
  description: string;
  applicationStartDate: string;
  applicationEndDate: string | null;
  status: ApplicationStatus;
}
