import styles from "./AuthPage.module.css";
import Button from "../../Components/Button/Button.tsx";

export default function AuthPage() {
  return (
    <div className={styles.authContainer}>
      <form className={styles.auth}>
        <div className={styles.authInputBlock}>
          <p>Электронная почта</p>
          <input type="email" placeholder="example@gmail.com" />
        </div>

        <div className={styles.authInputBlock}>
          <p>Пароль</p>
          <input type="password" placeholder="********" />
        </div>

        <Button variant="primary" font="header">
          Войти
        </Button>
      </form>
    </div>
  );
}
