import {API_HOST, API_PORT} from '@env';

type Method = 'get' | 'post' | 'put' | 'delete';

interface RequestProps {
    route: string;
    method?: Method;
    body?: BodyInit;
    headers?: HeadersInit;
}

export default ({route, method = 'get', body, headers}: RequestProps): Promise<Response> => {
  return fetch(`http://${API_HOST}:${API_PORT}/api${route}`, {
    method,
    body,
    headers: {...headers, 'Content-Type': 'application/json'}
  });
};