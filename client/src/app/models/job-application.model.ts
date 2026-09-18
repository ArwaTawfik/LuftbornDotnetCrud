export const APPLICATION_STATUSES = ['Applied', 'Interviewing', 'Offer', 'Rejected', 'Withdrawn'] as const;

export type ApplicationStatus = (typeof APPLICATION_STATUSES)[number];

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
