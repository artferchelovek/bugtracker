import { useEffect, useState } from "react";
import type { ReactNode } from "react";
import { auth } from "./auth.ts";
import type { User } from "./auth.ts";
import { AuthContext } from "./authContext.ts";

export default function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(() =>
    Boolean(localStorage.getItem("accessToken")),
  );

  useEffect(() => {
    let active = true;

    if (!localStorage.getItem("accessToken")) {
      return;
    }

    auth
      .me()
      .then((currentUser) => {
        if (active) setUser(currentUser);
      })
      .catch(() => {
        if (active) setUser(null);
      })
      .finally(() => {
        if (active) setLoading(false);
      });

    return () => {
      active = false;
    };
  }, []);

  const signIn = async (login: string, password: string) => {
    await auth.login(login, password);
    const currentUser = await auth.me();
    console.log(currentUser);
    setUser(currentUser);
    return currentUser;
  };

  return (
    <AuthContext.Provider value={{ user, loading, signIn }}>
      {children}
    </AuthContext.Provider>
  );
}
