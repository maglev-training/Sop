import { createActionGroup, props } from "@ngrx/store";
import { UserState } from ".";


export const UserDocuments = createActionGroup({
    source: 'User Documents',
    events: {
        'User': props<{payload: UserState}>(),
    }
})