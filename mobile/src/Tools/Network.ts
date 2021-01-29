// @ts-ignore
import {API_HOST, API_PORT} from '@env';

type Method = 'get' | 'post' | 'put' | 'del';

interface RequestProps {
    route: string;
    method?: Method;
    body?: BodyInit;
    headers?: HeadersInit;
}

export default ({route, method = 'get', body, headers}: RequestProps): Promise<Response> => {
    return fetch(`http://${API_HOST}:${API_PORT}${route}`, {
        method,
        body,
        headers: {...headers, 'Content-Type': 'application/json'}
    });
};