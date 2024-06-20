import { ToHomeButton } from "@/components/ToHomeButton";
import { Flex } from "antd";
import { useNavigate } from "react-router-dom";

export const PaymentResultPage = () => {
  const navigate = useNavigate();

  return (
    <div style={{ paddingInline: 30 }}>
      <h1 style={{ marginBlock: 20 }}>Заказ успешно оплачен!</h1>
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
          Мои заказы
        </span>
      </Flex>
    </div>
  );
};
