export interface PublicUserView {
  userId: string;
  avatar?: string;
  email: string;
  name: string;
}

export interface PutUserCommand {
  avatar?: string;
  email: string;
  name: string;
}