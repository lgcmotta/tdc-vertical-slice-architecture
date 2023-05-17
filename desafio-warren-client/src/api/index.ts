import { api } from './api';

export async function getAsync<TResponse>(route: string): Promise<Response<TResponse>> {
    const response = await api.get<Response<TResponse>>(route);

    return response.data;
}

export async function postAsync<TResponse>(route: string, body: any): Promise<Response<TResponse>> {
    const response = await api.post<Response<TResponse>>(route, body);

    return response.data;
}

export async function putAsync<TResponse>(route: string, body: any): Promise<Response<TResponse>> {
    const response = await api.put<Response<TResponse>>(route, body);

    return response.data;
}
