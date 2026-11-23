import { api } from "./api.ts";

export const auth = {
  login: async (username: string, password: string) => {
    const response = await api.post("/auth/login", { username, password });
    localStorage.setItem("accessToken", response.data.accessToken);
  },
};
