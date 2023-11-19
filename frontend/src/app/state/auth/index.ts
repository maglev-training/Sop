import { createFeature, createReducer, on } from "@ngrx/store";
import { AuthDocuments } from "./actions";

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

export const authFeature = createFeature({
    name:'authFeature',
    reducer: createReducer(initialState,
        on(AuthDocuments.user, (state, { payload }) => ({ ...state, ...payload }))
        )
})