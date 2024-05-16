import styled from "styled-components";
import {colors} from "@/styles/constants";
import {Button, Flex} from "antd";
import {FC, useEffect, useRef, useState} from "react";
import {Booking, PaymentStatus, PaymentStatusLabel} from "@/types/Booking";

const StyledItem = styled(Flex)`
  border-radius: 5px;
  border: 2px solid ${colors.dark};
  padding: 12px;
  font-size: 24px;
`

interface Props {
  booking: Booking;
}

export const BookingItem: FC<Props> = ({booking}) => {
  const {bookingDate, startTime, endTime, coworkingAddress, seatNumber, status, price, paymentUrl, paymentExpireAt} = booking;
  const [paymentTimeRemaining, setPaymentTimeRemaining] = useState<number | undefined>();

  const timer = useRef<NodeJS.Timeout | null>(null);
  useEffect(() => {
    if (timer.current === null && status === PaymentStatus.AwaitingPayment) {
      timer.current = setInterval(() => {
        setPaymentTimeRemaining(Math.round((new Date(paymentExpireAt!).getTime() - new Date().getTime()) / 1000));
      }, 1000);
    }
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

  return (
    <StyledItem justify='space-between'>
      <div>
        <div>{timeStr}</div>
        <div>{coworkingAddress}</div>
        <div>место {seatNumber}</div>
        <div>статус: <b><i>{PaymentStatusLabel[(status === PaymentStatus.AwaitingPayment && Number(paymentTimeRemaining) <= 0 ? PaymentStatus.PaymentTimeout : status) as PaymentStatus]} {paymentTimeRemainingStr}</i></b></div>
      </div>
      <Flex vertical align='end' justify='space-between'>
        <div>{price}р.</div>
        <Flex gap={4}>
          {status === PaymentStatus.AwaitingPayment && Number(paymentTimeRemaining) > 0 && <Button onClick={() => window.open(paymentUrl)}>Оплатить</Button>}
          <Button>Отменить</Button>
        </Flex>
      </Flex>
    </StyledItem>
  );
};
