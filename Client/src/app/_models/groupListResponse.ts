import { GroupData } from "./groupData";

export interface GroupListResponse{
    data: GroupData[];
    code: number;
    errors: string[];
}