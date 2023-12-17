export interface AuthLoginResponse {
    autentificationToken: AutentificationToken
    isLogin: boolean
}

export interface AutentificationToken {
    id: number
    value: string
    accountId: number
    account: Account
    autentificationTimestamp: string
    ipAddress: string
}
export interface Account {
    id: number
    username: string
    isBank: boolean
    isUser: boolean
}