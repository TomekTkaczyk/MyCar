import { useAuthStore } from '@/stores/AuthStore';

interface RequestOptions {
    method: string;
    headers: Record<string, string>;
    body?: string;
}

export const fetchWrapper = {
    get: request('GET'),
    post: request('POST'),
    put: request('PUT'),
    delete: request('DELETE')
};

function request(method: string) {
    return (url: string, body?: any) => {
        const requestOptions: RequestOptions = {
            method,
            headers: authHeader(url),
            body,
        };

        if (body) {
            requestOptions.headers['Content-Type'] = 'content/type';
            requestOptions.body = JSON.stringify(body);
        }

        console.log(url);
        console.log(requestOptions);

        url = 'http://localhost:5000/users-module/Account/sign-in';
        return fetch(url, requestOptions);
    }
}

// helper functions

function authHeader(url: string): Record<string, string> {

    const authStore = useAuthStore();

    const isLoggedIn = !!authStore.isAuthenticated;
    const isApiUrl = url.startsWith(import.meta.env.VITE_API_URL);


    if (isLoggedIn && isApiUrl) {
        return { Authorization: `Bearer ${authStore.token}` };
    } else {
        return {};
    }
}

async function handleResponse(response: Response) {

    console.log("handleResponse");
    console.log(response);
    const text = await response.text();
    const data = text && JSON.parse(text);

    if (!response.ok) {
        console.log("handleResponse not Ok.");
    }

    return response.text().then(text => {
        const data = text && JSON.parse(text);

        if (!response.ok) {
            const authStore = useAuthStore();
            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }

        return data;
    });
}
