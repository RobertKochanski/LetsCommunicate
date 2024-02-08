import * as uuid from 'uuid';

export interface SearchUserData{
    id: uuid;
    userName: string;
    email: string;
    photoUrl: string | undefined;
}