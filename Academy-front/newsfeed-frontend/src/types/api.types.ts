export interface ApiResponse<T = any> {
  data?: T;
  message?: string;
}

export interface PaginationParams {
  limit: number;
  offset: number;
}

export interface PaginatedResponse<T> {
  data: T[];
  total: number;
}