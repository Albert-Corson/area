export interface Response<T = undefined> {
  successful: boolean
  error?: string
  data?: T
}
