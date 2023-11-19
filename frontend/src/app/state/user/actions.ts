import { createActionGroup, emptyProps, props } from "@ngrx/store";
import { UserState } from ".";

export const UserCommands = createActionGroup({
    source: 'User Commands',
    events: {
        'CheckAuth': emptyProps(),
        'Log In': emptyProps(),
        'Log Out': emptyProps(),
    }
});

export const UserDocuments = createActionGroup({
    source: 'User Documents',
    events: {
        User: props<{payload: UserState}>(),
    }
})