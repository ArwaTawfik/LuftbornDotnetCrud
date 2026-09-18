import { Routes } from '@angular/router';
import { authGuardFn } from '@auth0/auth0-angular';
import { authConfig } from './auth/auth.config';
import { JobBoard } from './components/job-board/job-board';
import { JobForm } from './components/job-form/job-form';

const canActivate = authConfig.enabled ? [authGuardFn] : [];

export const routes: Routes = [
  { path: '', component: JobBoard, canActivate },
  { path: 'applications/new', component: JobForm, canActivate },
  { path: 'applications/:id/edit', component: JobForm, canActivate },
];
