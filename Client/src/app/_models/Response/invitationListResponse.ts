import { InvitationData } from "../Data/invitationData";

export interface InvitationListResponse{
    data: InvitationData[];
    code: number;
    errors: string[];
}