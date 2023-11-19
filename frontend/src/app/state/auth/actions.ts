import { createActionGroup, emptyProps, props } from "@ngrx/store";
import { UserState } from ".";

export const AuthCommands = createActionGroup({
    source: 'Auth Commands',
    events: {
        'CheckAuth': emptyProps(),
        'Log In': emptyProps(),
        'Log Out': emptyProps(),
    }
});

export const AuthDocuments = createActionGroup({
    source: 'Auth Documents',
    events: {
        User: props<{payload: UserState}>(),
    }
});

export const AuthEvents = createActionGroup({
    source: 'Auth Events',
    events: {
       
        'User Logged In': emptyProps(),
        'User Logged Out': emptyProps(),
    }
})