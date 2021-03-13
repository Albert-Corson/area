const API_HOST = '192.168.1.99'
const API_PORT = '8080'

type Method = 'get' | 'post' | 'put' | 'delete';

interface RequestProps {
  route: string;
  method?: Method;
  body?: BodyInit;
  headers?: HeadersInit;
}

export default ({
  route, method = 'get', body, headers,
}: RequestProps): Promise<Response> => fetch(`http://${API_HOST}:${API_PORT}/api${route}`, {
  method,
  body,
  headers: {...headers, 'Content-Type': 'application/json'},
})
