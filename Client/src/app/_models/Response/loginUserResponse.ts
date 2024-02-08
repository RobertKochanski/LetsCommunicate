import { LoginUserData } from "../Data/loginUserData";

export interface LoginUserResponse{
    data: LoginUserData;
    code: number;
    errors: string[];
}