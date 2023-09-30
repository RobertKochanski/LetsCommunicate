import * as uuid from 'uuid';

export interface UserData{
    id: uuid;
    userName: string;
    email: string;
    token: string;
}