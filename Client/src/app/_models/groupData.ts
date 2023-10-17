import { MessageData } from "./messageData";
import { UserData } from "./userData";
import * as uuid from 'uuid';

export interface GroupData{
    id: uuid;
    name: string;
    ownerEmail: string;
    users: UserData[];
    messages: MessageData[];
    permissionEmails: string[];
}