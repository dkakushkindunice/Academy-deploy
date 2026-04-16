import apiClient from './axios';
import type { ApiResponse } from '../types/api.types';
import type { RegisterRequest, LoginRequest, UserResponse } from '../types/auth.types';

export const authApi = {
  async register(data: RegisterRequest): Promise<ApiResponse<UserResponse>> {
    const response = await apiClient.post<ApiResponse<UserResponse>>(
      '/v1/Auth/signUp',
      data
    );
    return response.data;
  },

  async login(data: LoginRequest): Promise<ApiResponse<UserResponse>> {
    const response = await apiClient.post<ApiResponse<UserResponse>>(
      '/v1/Auth/signIn',
      data
    );
    return response.data;
  },
};

apiClient.defaults.headers.common['Authorization'] = 'Bearer' + localStorage.getItem('token');
