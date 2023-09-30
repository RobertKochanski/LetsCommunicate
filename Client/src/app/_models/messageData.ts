import * as uuid from 'uuid';
import { UserData } from './userData';

export interface MessageData{
    senderId: uuid;
    sender: UserData;
    groupId: uuid;
    content: string;
    messageSent: Date;
}