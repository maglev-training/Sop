import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Actions, concatLatestFrom, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { getApiUrl } from '../../auth/secure-api.interceptor';
import { AuthEvents } from '../../state/auth/actions';
import { map, switchMap, tap } from 'rxjs';
import { authFeature } from '../../state/auth';
import { UserState } from '.';
import { UserDocuments } from './actions';

@Injectable()
export class UserEffects {
  private readonly url = getApiUrl();

  getUserData$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthEvents.userLoggedIn),
        concatLatestFrom(() => this.store.select(authFeature.selectStreamId)),
        map(([_, streamId]) => streamId),
        switchMap((streamId) => this.http.get<UserState>(this.url + `users/${streamId}`)
            .pipe(
                map((payload) => UserDocuments.user({payload}))
            )
        )
      ),
    { dispatch: true }
  );
  constructor(
    private readonly actions$: Actions,
    private readonly http: HttpClient,
    private readonly store: Store
  ) {}
}
