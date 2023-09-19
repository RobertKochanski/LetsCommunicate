import { UserData } from "./userData";

export interface UserResponse{
    data: UserData;
    code: number;
    errors: string[];
}