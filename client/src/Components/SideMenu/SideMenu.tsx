import styles from "./SideMenu.module.css";
import { useLocation, useNavigate } from "react-router";

interface SideMenuProps {
  type?: "worker" | "tester";
}

export default function SideMenu({ type = "worker" }: SideMenuProps) {
  const navigate = useNavigate();
  const location = useLocation();
  const path = location.pathname.split("/")[1];

  if (type === "worker") {
    return (
      <div className={styles.sideMenu}>
        <div className={styles.menu}>
          <p className={styles.menuTitle}>ЗАДАЧИ РАЗРАБОТЧИКА</p>

          <p
            className={`${styles.menuLabel} ${path === "tickets" ? styles.active : ""}`}
            onClick={() => navigate("/tickets")}
          >
            <span className="material-symbols-outlined">list</span>Все тикеты
            проекта
          </p>
        </div>

        <div className={styles.menu}>
          <p className={styles.menuTitle}>РАЗРАБОТКА & РЕЛИЗЫ</p>

          <p className={styles.menuLabel}>
            <span className="material-symbols-outlined">forward</span>
            Мои Merge Requests
          </p>
          <p className={styles.menuLabel}>
            <span className="material-symbols-outlined">reply</span>
            Ветки и коммиты
          </p>
          <p className={styles.menuLabel}>
            <span className="material-symbols-outlined">settings</span>
            Метрики сборки
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className={styles.sideMenu}>
      <p>SideMenu</p>
    </div>
  );
}
