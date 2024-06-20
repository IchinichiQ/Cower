import { useAppSelector } from "@/redux";
import { Button, Checkbox, Flex } from "antd";
import { ToHomeButton } from "@/components/ToHomeButton";
import { formatOrderTime } from "@/utils/formatOrderTime";
import axios from "axios";
import { useState } from "react";
import { ErrorText } from "@/styles/styles";
import { baseUrl } from "@/api";

export const CheckoutPage = () => {
  const { order } = useAppSelector((state) => state.order);
  const [errors, setErrors] = useState<string[]>([]);

  const { address, date, place, timeFrom, timeTo, price, seatId } = order!;

  const [isStudent, setIsStudent] = useState(false);
  const initialPrice = price * (timeTo - timeFrom);
  const finalPrice = isStudent ? Math.floor(initialPrice * 0.9) : initialPrice;

  const handleSubmit = () => {
    setErrors([]);
    axios
      .post(`${baseUrl}/v1/bookings`, {
        seatId,
        bookingDate: date,
        startTime: `${String(timeFrom).padStart(2, "0")}:00`,
        endTime: `${String(timeTo).padStart(2, "0")}:00`,
        isStudent,
      })
      .then((res) => {
        window.open(res.data.booking.paymentUrl, "_self");
      })
      .catch((e) => {
        if (e.response) {
          setErrors(e.response.data.error.details);
        } else {
          setErrors(["Не удалось авторизоваться"]);
        }
      });
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
            Я студент (необходимо предоставить студ. билет или зачётную книжку)
          </label>
        </Flex>
        <Button onClick={handleSubmit}>Оплатить заказ</Button>
        {errors.map((error) => (
          <ErrorText>{error}</ErrorText>
        ))}
      </Flex>
    </div>
  );
};
