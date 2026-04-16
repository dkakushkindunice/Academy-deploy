export interface TagDto {
  title: string;
}

export interface NewsOutDto {
  id: number;
  title: string;
  description: string;
  image?: string;
  tags: TagDto[];
  userId: string;
  username: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface CreateNewsCommand {
  title: string;
  description: string;
  tags: TagDto[];
  image?: string;
}

export interface PutNewsCommand {
  title: string;
  description: string;
  image?: string;
  tags: TagDto[];
}

export interface NewsFilterParams {
  author?: string;
  keywords?: string;
  tags?: string[];
  limit?: number;
  offset?: number;
}

export interface PaginatedNewsDto {
  newsOutDtos: NewsOutDto[];
  numberOfElements: number;
}