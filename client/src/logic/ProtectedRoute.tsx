import { Navigate, Outlet } from "react-router";
import { useAuth } from "./authContext.ts";
import { isAdmin } from "./auth.ts";

export default function ProtectedRoute({ adminOnly = false }: {
  adminOnly?: boolean;
}) {
  const { user, loading } = useAuth();

  if (loading) return null;

  if (!user) {
    return <Navigate to={"/login"} replace />;
  }

  if (adminOnly && !isAdmin(user)) {
    return <Navigate to={"/"} replace />;
  }

  return <Outlet />;
}
