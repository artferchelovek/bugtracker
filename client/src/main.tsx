import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./styles/index.css";
import "./styles/material-typography.css";
import { BrowserRouter, Route, Routes } from "react-router";
import MainPage from "./Pages/MainPage/MainPage.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path={"/"} element={<MainPage />} />
      </Routes>
    </BrowserRouter>
  </StrictMode>,
);
