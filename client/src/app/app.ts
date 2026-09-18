import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthStatus } from './components/auth-status/auth-status';

@Component({
  imports: [RouterOutlet, AuthStatus],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App {}
