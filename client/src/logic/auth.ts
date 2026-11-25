import { api } from "./api.ts";

export interface User {
  Id: string;
  Login: string;
  FullName: string;
  Email: string;
  Position: string;
  Role: string;
  IsActive: boolean;
  RegisteredByLogin: string | null;
}

export function isAdmin(user: User | null | undefined) {
  return typeof user?.Role === "string" && user.Role.toLowerCase() === "admin";
}

let pendingMe: { token: string | null; promise: Promise<User> } | null = null;

export const auth = {
  login: async (login: string, password: string) => {
    const response = await api.post("/auth/login", { login, password });
    localStorage.setItem("accessToken", response.data.Token);
  },
  me: () => {
    const token = localStorage.getItem("accessToken");

    if (pendingMe?.token === token) {
      return pendingMe.promise;
    }

    const promise = api
      .get<User & { Role?: string }>("/auth/me")
      .then((response) => {
        const user = response.data;
        const role = user.Role ?? user.Role;

        if (typeof role !== "string") {
          throw new Error("Ответ /auth/me не содержит роль пользователя");
        }

        return { ...user, role };
      });
    pendingMe = { token, promise };
    void promise.then(
      () => {
        if (pendingMe?.promise === promise) pendingMe = null;
      },
      () => {
        if (pendingMe?.promise === promise) pendingMe = null;
      },
    );

    return promise;
  },

  logout: () => {
    localStorage.removeItem("accessToken");
  },
};
