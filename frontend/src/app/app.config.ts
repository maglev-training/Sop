import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideState, provideStore } from '@ngrx/store';
import { reducers } from './state';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { secureApiInterceptor } from './auth/secure-api.interceptor';
import { userFeature } from './state/user';
import { UserEffects } from './state/user/effects';
import { provideEffects } from '@ngrx/effects';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes),
    provideHttpClient(withInterceptors([secureApiInterceptor])),
  provideStore(reducers),
  provideState(userFeature),
  provideEffects([UserEffects]),
  provideStoreDevtools()
  ]
};
