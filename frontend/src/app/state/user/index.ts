import { createFeature, createReducer, on } from "@ngrx/store";
import { UserDocuments } from "./actions";

export interface UserState {
    isAuthenticated: boolean;
    sub: string | undefined;
    streamId: string | undefined
}

const initialState: UserState = {
    isAuthenticated: false,
    sub: undefined,
    streamId: undefined
}

export const userFeature = createFeature({
    name:'userFeature',
    reducer: createReducer(initialState,
        on(UserDocuments.user, (state, { payload }) => ({ ...state, ...payload }))
        )
})