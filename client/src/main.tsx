import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./styles/index.css";
import "./styles/material-typography.css";
import { BrowserRouter, Route, Routes } from "react-router";
import MainPage from "./Pages/MainPage/MainPage.tsx";
import ProtectedRoute from "./logic/ProtectedRoute.tsx";
import AuthPage from "./Pages/AuthPage/AuthPage.tsx";
import TestPage from "./Pages/TestPage/TestPage.tsx";
import TicketsPage from "./Pages/TicketsPage/TicketsPage.tsx";
import AuthProvider from "./logic/AuthProvider.tsx";
import AdminRouteGuard from "./logic/AdminRouteGuard.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <AuthProvider>
        <AdminRouteGuard>
          <Routes>
            <Route path={"/login"} element={<AuthPage />} />
            <Route path={"/test"} element={<TestPage />} />

            <Route element={<ProtectedRoute />}>
              <Route path={"/"} element={<MainPage />} />
              <Route path={"/tickets"} element={<TicketsPage />} />
            </Route>
            <Route element={<ProtectedRoute adminOnly />}>
              <Route path={"/admin"} element={<MainPage />} />
            </Route>
          </Routes>
        </AdminRouteGuard>
      </AuthProvider>
    </BrowserRouter>
  </StrictMode>,
);
