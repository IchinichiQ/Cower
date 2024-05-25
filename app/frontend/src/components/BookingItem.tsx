import styled from "styled-components";
import {colors} from "@/styles/constants";
import {Button, Flex} from "antd";
import {FC, useEffect, useRef, useState} from "react";
import {Booking, PaymentStatus, PaymentStatusLabel} from "@/types/Booking";
import axios from "axios";
import {baseUrl} from "@/api";

const StyledItem = styled(Flex)`
  border-radius: 5px;
  border: 2px solid ${colors.dark};
  padding: 12px;
  font-size: 24px;
`

interface Props {
  booking: Booking;

  onCancel(): void;
}

export const BookingItem: FC<Props> = ({booking, onCancel}) => {
  const {
    bookingDate,
    startTime,
    endTime,
    coworkingAddress,
    seatNumber,
    status,
    price,
    paymentUrl,
    paymentExpireAt
  } = booking;
  const [paymentTimeRemaining, setPaymentTimeRemaining] = useState<number | undefined>();

  const initTimer = () => {
    if (timer.current === null && status === PaymentStatus.AwaitingPayment) {
      timer.current = setInterval(() => {
        setPaymentTimeRemaining(Math.round((new Date(paymentExpireAt!).getTime() - new Date().getTime()) / 1000));
      }, 1000);
    }
  }

  const timer = useRef<NodeJS.Timeout | null>(null);
  useEffect(() => {
    initTimer();
    return () => {
      if (timer.current) {
        clearInterval(timer.current);
      }
    }
  }, []);

  const timeStr = `${new Date(bookingDate).toLocaleDateString()}, ${startTime} - ${endTime}`;
  let paymentTimeRemainingStr;
  if (paymentTimeRemaining && Number(paymentTimeRemaining) > 0) {
    const t = Number(paymentTimeRemaining);
    const m = Math.floor(t / 60);
    const s = t % 60;
    paymentTimeRemainingStr = `(${String(m).padStart(2, '0')}:${String(s).padStart(2, '0')})`;
  }

  const [loading, setLoading] = useState(false);
  const handleCancel = () => {
    setLoading(true);
    if (timer.current) {
      clearInterval(timer.current);
      setPaymentTimeRemaining(0);
    }
    axios.post(`${baseUrl}/v1/bookings/${booking.id}/cancel`)
      .then(() => {
        axios.get(`${baseUrl}/v1/bookings/${booking.id}`)
          .then(() => {
            onCancel();
          })
          .catch(() => {
            initTimer();
          })
      })
      .finally(() => {
        setLoading(false);
      });
  }

  const correctedStatus = (status === PaymentStatus.AwaitingPayment && Number(paymentTimeRemaining) <= 0 ? PaymentStatus.PaymentTimeout : status) as PaymentStatus;
  const cancelBtnVisible = correctedStatus === PaymentStatus.AwaitingPayment || correctedStatus === PaymentStatus.Paid;
  console.log(correctedStatus)
  return (
    <StyledItem justify='space-between'>
      <div>
        <div>{timeStr}</div>
        <div>{coworkingAddress}</div>
        <div>место {seatNumber}</div>
        <div>статус: <b><i>{PaymentStatusLabel[correctedStatus]} {paymentTimeRemainingStr}</i></b></div>
      </div>
      <Flex vertical align='end' justify='space-between'>
        <div>{price}р.</div>
        <Flex gap={4}>
          {status === PaymentStatus.AwaitingPayment && Number(paymentTimeRemaining) > 0 &&
            <Button onClick={() => window.open(paymentUrl)}>Оплатить</Button>}
          {cancelBtnVisible && <Button loading={loading} onClick={handleCancel}>Отменить</Button>}
        </Flex>
      </Flex>
    </StyledItem>
  );
};
