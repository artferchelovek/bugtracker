import * as axios from "axios";

const apiUrl = import.meta.env.API_URL;

export const api = axios.create({
  baseURL: apiUrl,
  timeout: 10_000,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});
