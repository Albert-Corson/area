import RequestBuilder, {APIResponse} from './RequestBuilder';
import store from './Store';

interface SignInProps {
    email: string;
    password: string;
}

interface SignUpProps {
    email: string;
    userName: string;
    firstName: string;
    lastName: string;
    password: string;
}

export default {
  SignIn: async ({email, password}: SignInProps): Promise<APIResponse> => {
    const {status, body} = await RequestBuilder(
      '/user/signin',
      'post',
      {email, password},
    );

    store.token = body?.accessToken;
    store.id = body?.userId;

    return {status, body};
  },
  SignUp: async ({email, userName, firstName, lastName, password}: SignUpProps): Promise<APIResponse> => {
    return await RequestBuilder(
      '/user/signup',
      'post',
      {email, userName, password, firstName, lastName},
    );
  },
};
