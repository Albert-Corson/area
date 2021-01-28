import store from './Store';

const baseUrl = 'http://192.168.1.100:5000';

interface APIResponse {
    status: number;
    body: Record<string, number| string> | undefined;
}

export default async function(route: string, method: string, body: Record<string, number| string>): Promise<APIResponse> {
  const headers = {
    'Content-type': 'application/json'
  };

  if (store.token) {
    headers['Authorization'] = store.token;
  }
  console.log('before request');

  try {
    const res = await fetch(`${baseUrl}${route}`, {
      method,
      headers,
      body: JSON.stringify(body),
    });
    console.log('after request');

    return {
      status: res.status,
      body: await res.json(),
    };
  } catch (e) {
    console.log(e);
    return {
      status: 500,
      body: {error: 'Internal error'},
    };
  }
}

export {APIResponse};
