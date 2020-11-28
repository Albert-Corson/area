export default interface ResponseModel<T = undefined> {
  successful: boolean
  error?: string
  data?: T
}