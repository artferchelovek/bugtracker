import styles from "./AuthPage.module.css";
import Button from "../../Components/Button/Button.tsx";
import { useState } from "react";
import { useNavigate } from "react-router";
import { useAuth } from "../../logic/authContext.ts";
import { isAdmin } from "../../logic/auth.ts";

export default function AuthPage() {
  const navigate = useNavigate();
  const { signIn } = useAuth();

  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = async () => {
    try {
      const user = await signIn(login, password);
      navigate(isAdmin(user) ? "/admin" : "/", { replace: true });
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div className={styles.authContainer}>
      <form className={styles.auth}>
        <div className={styles.authInputBlock}>
          <p>Имя пользователя</p>
          <input
            value={login}
            onChange={(e) => setLogin(e.target.value)}
            type="text"
            placeholder="ivanov"
          />
        </div>

        <div className={styles.authInputBlock}>
          <p>Пароль</p>
          <input
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            type="password"
            placeholder="********"
          />
        </div>

        <Button onClick={handleLogin} variant="primary" font="header">
          Войти
        </Button>
      </form>
    </div>
  );
}
