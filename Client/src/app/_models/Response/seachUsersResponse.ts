import { SearchUserData } from "../Data/searchUserData";

export interface SearchUsersResponse{
    data: SearchUserData[];
    code: number;
    errors: string[];
}