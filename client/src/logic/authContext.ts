import { createContext, useContext } from "react";
import type { User } from "./auth.ts";

export interface AuthContextValue {
  user: User | null;
  loading: boolean;
  signIn: (login: string, password: string) => Promise<User>;
  signOut: () => void;
}

export const AuthContext = createContext<AuthContextValue | null>(null);

export function useAuth() {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error("useAuth must be used within AuthProvider");
  }

  return context;
}
