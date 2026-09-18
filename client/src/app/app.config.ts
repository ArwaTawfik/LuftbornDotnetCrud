import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { authHttpInterceptorFn, provideAuth0 } from '@auth0/auth0-angular';
import { routes } from './app.routes';
import { authConfig } from './auth/auth.config';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptors(authConfig.enabled ? [authHttpInterceptorFn] : [])),
    provideRouter(routes),
    ...(authConfig.enabled
      ? [
          provideAuth0({
            domain: authConfig.domain,
            clientId: authConfig.clientId,
            authorizationParams: {
              redirect_uri: window.location.origin,
              audience: authConfig.audience,
            },
            httpInterceptor: { allowedList: [authConfig.protectedApiUrl] },
          }),
        ]
      : []),
  ],
};
