import { UserInfoData } from "../Data/userInfoData";

export interface UserInfoResponse{
    data: UserInfoData;
    code: number;
    errors: string[];
}