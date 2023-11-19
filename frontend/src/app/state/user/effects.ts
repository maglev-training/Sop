import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { UserCommands, UserDocuments } from './actions';
import { catchError, map, of, switchMap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { UserState } from '.';
import { getApiUrl } from '../../auth/secure-api.interceptor';

/* When the home component loads, we'll see if they are logged in by selecting the isAuthenticated from the userFeature. 
If they aren't, we'll call the /api/user. If we get a 401, we'll navigate to the login.
*/
export type UserClaim = { type: string; value: string };
@Injectable()
export class UserEffects {
    private readonly url = getApiUrl();
  checkAuth$ = createEffect(() =>
    this.actions$.pipe(
      ofType(UserCommands.checkAuth),
      switchMap(() =>
        this.http.get<UserClaim[]>(this.url + 'user').pipe(
          map((claims) => {
            const sub = claims.find((x) => x.type === 'sub')?.value;
            const streamId = claims.find((x) => x.type === 'stream_id')?.value;
            const payload = {
              isAuthenticated: true,
              sub,
              streamId,
            } as UserState;
            return UserDocuments.user({ payload });
          }),
         catchError((r) => of(UserCommands.logIn()))
        )
      )
    )
  );

    logIn$ = createEffect(() =>
        this.actions$.pipe(
        ofType(UserCommands.logIn),
        map(() => {
            window.location.href = '/api/login';
           
        })
        ), { dispatch: false}
    );
    
    logOut$ = createEffect(() => this.actions$.pipe(
        ofType(UserCommands.logOut),
        map(() => {
            window.location.href = '/api/logout';
        })
    ), { dispatch: false });
    

  constructor(
    private readonly actions$: Actions,
    private readonly http: HttpClient
  ) {}
}
