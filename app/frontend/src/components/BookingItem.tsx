import styled from "styled-components";
import { colors } from "@/styles/constants";
import { Button, Flex } from "antd";
import { FC, useEffect, useRef, useState } from "react";
import { Booking, PaymentStatus, PaymentStatusLabel } from "@/types/Booking";
import axios from "axios";
import { baseUrl } from "@/api";
import { UserRole } from "@/types/User";
import { getTimeDelta } from "@/utils/getTimeDelta";

const StyledItem = styled(Flex)`
  border-radius: 5px;
  border: 2px solid ${colors.dark};
  padding: 12px;
  font-size: 24px;
`;

interface Props {
  booking: Booking;
  onCancel(): void;
  onPaymentTimeExpired(): void;
}

export const BookingItem: FC<Props> = ({
  booking,
  onCancel,
  onPaymentTimeExpired,
}) => {
  const {
    bookingDate,
    startTime,
    endTime,
    coworkingAddress,
    seatNumber,
    status,
    price,
    paymentUrl,
    floor,
    paymentExpireAt,
    isDiscountApplied,
    initialPrice,
    user,
  } = booking;
  const [paymentTimeRemaining, setPaymentTimeRemaining] = useState<
    number | undefined
  >();

  const initTimer = () => {
    if (timer.current === null && status === PaymentStatus.AwaitingPayment) {
      timer.current = setInterval(() => {
        setPaymentTimeRemaining(
          Math.round(
            (new Date(paymentExpireAt!).getTime() - new Date().getTime()) / 1000
          )
        );
      }, 1000);
    }
  };

  useEffect(() => {
    if (Number(paymentTimeRemaining) <= 0) {
      if (timer.current) {
        clearInterval(timer.current);
      }
      onPaymentTimeExpired();
    }
  }, [paymentTimeRemaining]);

  const timer = useRef<NodeJS.Timeout | null>(null);
  useEffect(() => {
    initTimer();

    return () => {
      if (timer.current) {
        clearInterval(timer.current);
      }
    };
  }, []);

  const timeStr = `${new Date(bookingDate).toLocaleDateString()}, ${startTime} - ${endTime}`;
  let paymentTimeRemainingStr;
  if (paymentTimeRemaining && Number(paymentTimeRemaining) > 0) {
    const t = Number(paymentTimeRemaining);
    const m = Math.floor(t / 60);
    const s = t % 60;
    paymentTimeRemainingStr = `(${String(m).padStart(2, "0")}:${String(s).padStart(2, "0")})`;
  }

  const [loading, setLoading] = useState(false);
  const handleCancel = () => {
    setLoading(true);
    if (timer.current) {
      clearInterval(timer.current);
      setPaymentTimeRemaining(0);
    }
    axios
      .post(`${baseUrl}/v1/bookings/${booking.id}/cancel`)
      .then(() => {
        axios
          .get(`${baseUrl}/v1/bookings/${booking.id}`)
          .then(() => {
            onCancel();
          })
          .catch(() => {
            initTimer();
          });
      })
      .finally(() => {
        setLoading(false);
      });
  };

  const cancelBtnVisible =
    status === PaymentStatus.AwaitingPayment ||
    status === PaymentStatus.Paid ||
    (![PaymentStatus.Success, PaymentStatus.Cancelled].includes(status) &&
      user?.role === UserRole.ADMIN);

  return (
    <StyledItem justify="space-between">
      <Flex gap={5} vertical>
        <div>{timeStr}</div>
        <div>{coworkingAddress}</div>
        <div>место {seatNumber}, этаж {floor}</div>
        {user && (
          <div>
            Пользователь:{" "}
            <i>
              {user.email} {user.role === UserRole.ADMIN && "(администратор)"}
            </i>
          </div>
        )}
        <div>
          статус:{" "}
          <b>
            <i>
              {PaymentStatusLabel[status as keyof typeof PaymentStatusLabel]}{" "}
              {paymentTimeRemainingStr}
            </i>
          </b>
        </div>
      </Flex>
      <Flex vertical align="end" justify="space-between">
        <Flex vertical>
          <Flex gap={10} justify="end">
            {isDiscountApplied && <s>{initialPrice}р.</s>} {price}р.{" "}
          </Flex>
          {isDiscountApplied && (
            <span style={{ fontSize: 14, whiteSpace: "nowrap" }}>
              (студ. скидка)
            </span>
          )}
        </Flex>
        <Flex gap={4}>
          {status === PaymentStatus.AwaitingPayment &&
            Number(paymentTimeRemaining) > 0 && (
              <Button onClick={() => window.open(paymentUrl)}>Оплатить</Button>
            )}
          {cancelBtnVisible && (
            <Button loading={loading} onClick={handleCancel}>
              Отменить
            </Button>
          )}
        </Flex>
      </Flex>
    </StyledItem>
  );
};
