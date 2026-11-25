import styles from "./AdminPage.module.css";
import { useEffect, useState } from "react";
import type { FormEvent } from "react";
import type { User } from "../../logic/auth.ts";
import { admin } from "../../logic/admin.ts";
import type { ICreateUserForm } from "../../logic/admin.ts";
import Button from "../../Components/Button/Button.tsx";

const emptyForm: ICreateUserForm = {
  fullName: "",
  login: "",
  password: "",
  role: "",
};

// страница временная, для тестов, юай юикс появится позже
export default function AdminPage() {
  const [users, setUsers] = useState<User[]>([]);
  const [formData, setFormData] = useState<ICreateUserForm>(emptyForm);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await admin.getUsers();
        setUsers(response.data);
      } catch (e) {
        console.log(e);
      }
    };

    fetchUsers();
  }, []);

  const registerUser = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setIsSubmitting(true);

    try {
      await admin.createUser(formData);
      setFormData(emptyForm);

      try {
        const response = await admin.getUsers();
        setUsers(response.data);
      } catch (error) {
        console.error(error);
      }
    } catch (error) {
      console.error(error);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className={styles.adminContainer}>
      <div className={styles.usersList}>
        <p>Пользователи</p>
        {users.map((user) => (
          <div key={user.Id} className={styles.user}>
            <p>
              {user.FullName} {user.Role}
            </p>
            <p>{user.Login}</p>
          </div>
        ))}
      </div>

      <form className={styles.createUser} onSubmit={registerUser}>
        <Button font="body">Выйти из аккаунта</Button>
        <p>Создать пользователя</p>

        <div className={styles.userField}>
          <label htmlFor="fullName">Фамилия Имя</label>
          <input
            id="fullName"
            type="text"
            required
            value={formData.fullName}
            onChange={(event) =>
              setFormData({ ...formData, fullName: event.target.value })
            }
          />
        </div>

        <div className={styles.userField}>
          <label htmlFor="login">Логин</label>
          <input
            id="login"
            type="text"
            required
            value={formData.login}
            onChange={(event) =>
              setFormData({ ...formData, login: event.target.value })
            }
          />
        </div>

        <div className={styles.userField}>
          <label htmlFor="password">Пароль</label>
          <input
            id="password"
            type="password"
            required
            value={formData.password}
            onChange={(event) =>
              setFormData({ ...formData, password: event.target.value })
            }
          />
        </div>

        <div className={styles.userField}>
          <label htmlFor="role">Роль</label>
          <select
            id="role"
            required
            value={formData.role}
            onChange={(event) =>
              setFormData({ ...formData, role: event.target.value })
            }
          >
            <option value="" disabled>
              Выберите роль
            </option>
            <option value="Developer">Developer</option>
            <option value="Tester">Tester</option>
          </select>
        </div>

        <Button type="submit" disabled={isSubmitting}>
          {isSubmitting ? "Создание..." : "Создать пользователя"}
        </Button>
      </form>
    </div>
  );
}
