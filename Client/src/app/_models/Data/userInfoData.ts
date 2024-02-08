import * as uuid from 'uuid';

export interface UserInfoData{
    id: uuid;
    userName: string;
    email: string;
    description: string | undefined;
    city: string;
    country: string;
    registerDate: Date;
    age: number;
    photoUrl: string | undefined;
}