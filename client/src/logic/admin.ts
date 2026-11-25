import { api } from "./api.ts";
import type { User } from "./auth.ts";

export interface ICreateUserForm {
  fullName: string;
  login: string;
  password: string;
  role: string;
}

export const admin = {
  getUsers: async () => {
    return await api.get<User[]>("/admin/users");
  },

  createUser: async (data: ICreateUserForm) => {
    return await api.post<User>("/admin/users", data);
  },
};
