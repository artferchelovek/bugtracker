import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./styles/index.css";
import "./styles/material-typography.css";
import { BrowserRouter, Route, Routes } from "react-router";
import MainPage from "./Pages/MainPage/MainPage.tsx";
import ProtectedRoute from "./logic/ProtectedRoute.tsx";
import AuthPage from "./Pages/AuthPage/AuthPage.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path={"/login"} element={<AuthPage />} />

        <Route element={<ProtectedRoute />}>
          <Route path={"/"} element={<MainPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  </StrictMode>,
);
