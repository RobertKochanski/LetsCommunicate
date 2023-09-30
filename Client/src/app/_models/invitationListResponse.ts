import { InvitationData } from "./invitationData";

export interface InvitationListResponse{
    data: InvitationData[];
    code: number;
    errors: string[];
}