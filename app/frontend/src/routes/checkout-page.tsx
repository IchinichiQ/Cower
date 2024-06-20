import { useAppSelector } from "@/redux";
import { Button, Checkbox, Flex } from "antd";
import { ToHomeButton } from "@/components/ToHomeButton";
import { formatOrderTime } from "@/utils/formatOrderTime";
import axios from "axios";
import { useState } from "react";
import { ErrorText } from "@/styles/styles";
import { baseUrl } from "@/api";
import ym from "react-yandex-metrika";
import { getTimeDelta } from "@/utils/getTimeDelta";
import { useAuthorizedUser } from "@/redux/userSlice";
import { UserRole } from "@/types/User";
import { useNavigate } from "react-router-dom";
import styled, { keyframes } from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleNotch } from "@fortawesome/free-solid-svg-icons";

const kf = keyframes`
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
`;

const LoaderWrapper = styled("div")`
  display: flex;
  animation: ${kf} 1s linear infinite;
`;

export const CheckoutPage = () => {
  const user = useAuthorizedUser();
  const { order } = useAppSelector((state) => state.order);
  const [errors, setErrors] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);

  const { address, date, place, timeFrom, timeTo, price, seatId } = order!;

  const [isStudent, setIsStudent] = useState(false);

  const timeDelta = getTimeDelta(timeFrom, timeTo);
  const initialPrice = Math.floor(price * timeDelta);
  const finalPrice = isStudent ? Math.floor(initialPrice * 0.9) : initialPrice;

  const navigate = useNavigate();
  const handleSubmit = () => {
    setLoading(true);
    setErrors([]);
    axios
      .post(`${baseUrl}/v1/bookings`, {
        seatId,
        bookingDate: date,
        startTime: timeFrom,
        endTime: timeTo,
        applyDiscount: isStudent,
      })
      .then((res) => {
        if (user.role === UserRole.ADMIN) {
          navigate("/payment-result");
        } else {
          window.open(res.data.booking.paymentUrl, "_self");
        }
        ym("reachGoal", "create_order");
      })
      .catch((e) => {
        if (e.response) {
          setErrors(e.response.data.error.details);
        } else {
          setErrors(["Не удалось авторизоваться"]);
        }
      })
      .finally(() => setLoading(false));
  };

  return (
    <div style={{ paddingInline: 30 }}>
      <ToHomeButton />

      <h1 style={{ marginBlock: 20 }}>Оформление заказа</h1>
      <Flex gap={10} vertical align="start">
        <div>{formatOrderTime(timeFrom, timeTo, date)}</div>
        <div>{address}</div>
        <div>Место: {place}</div>
        <div>
          <b>
            {finalPrice}р. {isStudent && <s>{initialPrice}р.</s>}
          </b>
        </div>
        <Flex gap={10} style={{ marginBlock: 10 }}>
          <Checkbox
            id="student"
            checked={isStudent}
            onChange={(e) => setIsStudent(e.target.checked)}
          />
          <label htmlFor="student">
            {user.role === UserRole.ADMIN
              ? "Студенческая скидка (необходимо предоставить студ. билет или зачётную книжку)"
              : "Я студент (необходимо предоставить студ. билет или зачётную книжку)"}
          </label>
        </Flex>

        <Flex align="center" gap={20}>
          <Button onClick={handleSubmit}>
            {user.role === UserRole.ADMIN ? "Создать заказ" : "Оплатить заказ"}
          </Button>
          {loading && (
            <LoaderWrapper>
              <FontAwesomeIcon icon={faCircleNotch} />
            </LoaderWrapper>
          )}
        </Flex>
        {errors.map((error, index) => (
          <ErrorText key={index}>{error}</ErrorText>
        ))}
      </Flex>
    </div>
  );
};
