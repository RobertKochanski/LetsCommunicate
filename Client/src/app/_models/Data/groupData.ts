import { MessageData } from "./messageData";
import { LoginUserData } from "./loginUserData";
import * as uuid from 'uuid';

export interface GroupData{
    id: uuid;
    name: string;
    ownerEmail: string;
    users: LoginUserData[];
    messages: MessageData[];
    permissionEmails: string[];
}