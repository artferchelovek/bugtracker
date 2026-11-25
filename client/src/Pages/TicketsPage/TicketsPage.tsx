import styles from "./TicketsPage.module.css";
import SideMenu from "../../Components/SideMenu/SideMenu.tsx";

export default function TicketsPage() {
  return (
    <div className={styles.ticketContainer}>
      <SideMenu />
      <div className={styles.content}>mainContent</div>
    </div>
  );
}
