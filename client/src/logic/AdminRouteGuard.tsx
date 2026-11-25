import type { ReactNode } from "react";
import { Navigate, useLocation } from "react-router";
import { useAuth } from "./authContext.ts";
import { isAdmin } from "./auth.ts";

export default function AdminRouteGuard({ children }: { children: ReactNode }) {
  const { user, loading } = useAuth();
  const { pathname } = useLocation();

  if (loading) return null;

  if (isAdmin(user) && pathname !== "/admin") {
    return <Navigate to="/admin" replace />;
  }

  return children;
}
