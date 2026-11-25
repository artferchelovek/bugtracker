import { api } from "./api.ts";

export interface User {
  id: string;
  login: string;
  fullName: string;
  email: string;
  position: string;
  role: string;
  isActive: boolean;
  registeredByLogin: string | null;
}

export function isAdmin(user: User | null | undefined) {
  return typeof user?.role === "string" && user.role.toLowerCase() === "admin";
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

    const promise = api.get<User & { Role?: string }>("/auth/me").then((response) => {
      const user = response.data;
      const role = user.role ?? user.Role;

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
};
