import apiClient from './axios';
import type { ApiResponse, PaginationParams } from '../types/api.types';
import type {
  NewsOutDto,
  CreateNewsCommand,
  PutNewsCommand,
  NewsFilterParams,
  PaginatedNewsDto,
} from '../types/news.types';

export const newsApi = {
  async getAll(params: PaginationParams): Promise<ApiResponse<PaginatedNewsDto>> {
    const response = await apiClient.get<ApiResponse<PaginatedNewsDto>>(
      '/v1/news',
      { params }
    );
    return response.data;
  },

  async create(data: CreateNewsCommand): Promise<ApiResponse<NewsOutDto>> {
    const response = await apiClient.post<ApiResponse<NewsOutDto>>(
      '/v1/news',
      data
    );
    return response.data;
  },

  async update(id: number, data: PutNewsCommand): Promise<ApiResponse> {
    const response = await apiClient.put<ApiResponse>(
      `/v1/news/${id}`,
      data
    );
    return response.data;
  },

  async delete(id: number): Promise<ApiResponse> {
    const response = await apiClient.delete<ApiResponse>(`/v1/news/${id}`);
    return response.data;
  },

  async find(params: NewsFilterParams): Promise<ApiResponse> {
    const response = await apiClient.get<ApiResponse>('/v1/news/find', {
      params,
    });
    return response.data;
  },

  async getByUser(
    userId: string,
    params: PaginationParams
  ): Promise<ApiResponse> {
    const response = await apiClient.get<ApiResponse>(
      `/v1/news/user/${userId}`,
      { params }
    );
    return response.data;
  },
};