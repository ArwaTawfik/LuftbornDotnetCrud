import { Routes } from '@angular/router';
import { JobBoard } from './components/job-board/job-board';
import { JobForm } from './components/job-form/job-form';

export const routes: Routes = [
  { path: '', component: JobBoard },
  { path: 'applications/new', component: JobForm },
  { path: 'applications/:id/edit', component: JobForm },
];
