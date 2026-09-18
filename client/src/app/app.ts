import { Component } from '@angular/core';
import { JobBoard } from './components/job-board/job-board'
@Component({
  imports: [JobBoard],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App {}
