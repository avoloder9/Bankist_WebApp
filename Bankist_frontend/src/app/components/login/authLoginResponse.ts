import { AutentificationToken } from "src/app/helpers/auth/autentificationToken"

export interface AuthLoginResponse {
    autentificationToken: AutentificationToken
    isLogin: boolean
}