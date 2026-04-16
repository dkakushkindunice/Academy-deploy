import apiClient from './axios';
import type { ApiResponse } from '../types/api.types';
import type { PublicUserView, PutUserCommand } from '../types/user.types';

export const userApi = {
  async getById(id: string): Promise<ApiResponse<PublicUserView>> {
    const response = await apiClient.get<ApiResponse<PublicUserView>>(
      `/v1/user/${id}`
    );
    return response.data;
  },

  async getCurrentUser(): Promise<ApiResponse<PublicUserView>> {
    const response = await apiClient.get<ApiResponse<PublicUserView>>(
      '/v1/user/info'
    );
    return response.data;
  },

  async update(data: PutUserCommand): Promise<ApiResponse<PublicUserView>> {
    const response = await apiClient.put<ApiResponse<PublicUserView>>(
      '/v1/user',
      data
    );
    return response.data;
  },

  async delete(): Promise<void> {
    await apiClient.delete('/v1/user');
  },
};