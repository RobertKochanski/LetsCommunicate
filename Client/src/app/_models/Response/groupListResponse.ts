import { GroupData } from "../Data/groupData";

export interface GroupListResponse{
    data: GroupData[];
    code: number;
    errors: string[];
}