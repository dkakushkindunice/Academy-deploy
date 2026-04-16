import apiClient from './axios';
import type { ApiResponse } from '../types/api.types';

export const fileApi = {
  async upload(file: File): Promise<ApiResponse<string>> {
    const formData = new FormData();
    formData.append('file', file);

    const response = await apiClient.post<ApiResponse<string>>(
      '/v1/file/uploadFile',
      formData,
      {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      }
    );
    return response.data;
  },

  getFileUrl(fileName: string): string {
    const baseUrl = import.meta.env.VITE_API_BASE_URL;
    return `${baseUrl}/v1/file/${fileName}`;
  },
};