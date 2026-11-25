import styles from "./Button.module.css";
import type { ComponentProps } from "react";

type ButtonProps = ComponentProps<"button"> & {
  variant?: "primary" | "secondary";
  font?: "body" | "header";
};

export default function Button({
  variant = "primary",
  font = "body",
  type = "button",
  children,
  ...props
}: ButtonProps) {
  return (
    <button
      type={type}
      className={`${styles.button} ${styles[variant]} ${styles[font]}`}
      {...props}
    >
      {children}
    </button>
  );
}
