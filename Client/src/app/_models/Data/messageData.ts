import * as uuid from 'uuid';
import { LoginUserData } from './loginUserData';

export interface MessageData{
    senderId: uuid;
    sender: LoginUserData;
    groupId: uuid;
    content: string;
    messageSent: Date;
}