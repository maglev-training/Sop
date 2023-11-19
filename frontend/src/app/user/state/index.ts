import { createFeature, createReducer, on } from "@ngrx/store";
import { UserDocuments } from "./actions";

export interface UserState {
    id: string | undefined;
    lastLogin: string | undefined;
    sub: string | undefined;
    logins: number  | undefined;
   version: number | undefined;
}

const initialState: UserState = { 
    id: undefined,
    lastLogin: undefined,
    sub: undefined,
    logins: undefined,
    version: undefined
};

export const userFeature = createFeature({
    name: 'userFeature',
    reducer: createReducer(initialState, on(UserDocuments.user, (_, {payload})=> payload  ))
})