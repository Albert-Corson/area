export interface Response<T = any> {
  error?: string;
  successful: boolean;
  data: T;
}
