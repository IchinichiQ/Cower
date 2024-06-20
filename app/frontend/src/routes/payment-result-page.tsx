import { ToHomeButton } from "@/components/ToHomeButton";
import { Flex } from "antd";
import { useNavigate } from "react-router-dom";
import { useAuthorizedUser } from "@/redux/userSlice";
import { UserRole } from "@/types/User";

export const PaymentResultPage = () => {
  const navigate = useNavigate();
  const user = useAuthorizedUser();

  return (
    <div style={{ paddingInline: 30 }}>
      <h1 style={{ marginBlock: 20 }}>
        Заказ успешно {user.role === UserRole.ADMIN ? "создан" : "оплачен"}!
      </h1>
      <Flex gap={20}>
        <ToHomeButton />
        <span
          onClick={() => navigate("/bookings")}
          style={{
            fontSize: 21,
            fontWeight: 700,
            textDecoration: "underline",
            cursor: "pointer",
          }}
        >
          {user.role === UserRole.ADMIN ? "Управление заказами" : "Мои заказы"}
        </span>
      </Flex>
    </div>
  );
};
