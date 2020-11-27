export default class AuthToken {
    accessToken: string
    refreshToken: string
    expiresIn: number

    constructor(json: any) {
        const body = JSON.parse(json)

        this.accessToken = body.access_token
        this.refreshToken = body.refresh_token
        this.expiresIn = body.expires_in
    }
}
