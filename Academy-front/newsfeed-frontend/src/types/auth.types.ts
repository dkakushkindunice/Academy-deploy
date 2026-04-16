export interface RegisterRequest {
  email: string;
  password: string;
  name: string;
  avatar?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface UserResponse {
  id: string;
  email: string;
  name: string;
  avatar?: string;
  token: string;
}