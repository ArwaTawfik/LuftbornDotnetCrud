import { Component, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { AuthService } from '@auth0/auth0-angular';
import { authConfig } from '../../auth/auth.config';

@Component({
  imports: [],
  selector: 'app-auth-status',
  styleUrl: './auth-status.scss',
  templateUrl: './auth-status.html',
})
export class AuthStatus {
  private readonly auth = authConfig.enabled ? inject(AuthService) : null;

  readonly user = this.auth ? toSignal(this.auth.user$) : signal(null);

  logout(): void {
    this.auth?.logout({ logoutParams: { returnTo: window.location.origin } });
  }
}
