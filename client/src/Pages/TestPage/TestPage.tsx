import styles from "./TestPage.module.css";
import SideMenu from "../../Components/SideMenu/SideMenu.tsx";

export default function TestPage() {
  return (
    <div className={styles.testContainer}>
      <SideMenu />
      <div className={styles.content}>mainContent</div>
    </div>
  );
}
