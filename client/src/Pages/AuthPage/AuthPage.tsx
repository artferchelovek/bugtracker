import styles from "./AuthPage.module.css";
import Button from "../../Components/Button/Button.tsx";
import { useState } from "react";
import { auth } from "../../logic/auth.ts";

export default function AuthPage() {
  const [login, setLogin] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = async () => {
    try {
      const response = await auth.login(login, password);
      console.log(response);
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
