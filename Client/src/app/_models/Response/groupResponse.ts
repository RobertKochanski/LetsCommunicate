import { GroupData } from "../Data/groupData";

export interface GroupResponse{
    data: GroupData;
    code: number;
    errors: string[];
}