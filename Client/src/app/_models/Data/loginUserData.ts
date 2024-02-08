import * as uuid from 'uuid';

export interface LoginUserData{
    id: uuid;
    userName: string;
    email: string;
    photoUrl: string | undefined;
    token: string;
}