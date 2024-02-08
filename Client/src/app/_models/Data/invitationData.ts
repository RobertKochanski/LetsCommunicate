import { GroupData } from "./groupData";
import * as uuid from 'uuid';

export interface InvitationData{
    id: uuid;
    senderEmail: string;
    invitedEmail: string;
    groupId: uuid;
    group: GroupData;
    invitedAt: Date;
}