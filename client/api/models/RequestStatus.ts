export default class RequestStatus<T> {
  successful: boolean
  error?: string
  data?: T

  constructor(DataType: (new (json: any) => T), json: any) {
    const body = JSON.parse(json)

    this.successful = body.successful || false
    this.error = body.error || null
    this.data = body.data ? new DataType(body.data) : undefined
  }
}
