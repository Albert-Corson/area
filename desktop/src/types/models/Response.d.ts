import Status from "./Status.d.ts"

export interface Response<T = undefined> extends Status {
  successful: boolean
  error?: string
}
