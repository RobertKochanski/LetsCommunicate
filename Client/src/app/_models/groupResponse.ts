import { GroupData } from "./groupData";

export interface GroupResponse{
    data: GroupData;
    code: number;
    errors: string[];
}