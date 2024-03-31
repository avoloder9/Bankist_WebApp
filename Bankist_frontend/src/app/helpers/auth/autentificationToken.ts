import { Account } from "./account"

export interface AutentificationToken {
    id: number
    value: string
    accountId: number
    account: Account
    autentificationTimestamp: string
    ipAddress: string
}