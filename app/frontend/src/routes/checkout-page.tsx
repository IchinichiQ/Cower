import {useAppSelector} from "@/redux";
import {Button, Flex} from "antd";
import {ToHomeButton} from "@/components/ToHomeButton";
import {formatOrderTime} from "@/utils/formatOrderTime";
import axios from "axios";
import {useState} from "react";
import {ErrorText} from "@/styles/styles";

export const CheckoutPage = () => {
  const {order} = useAppSelector(state => state.order);
  const [errors, setErrors] = useState<string[]>([])

  const {address, date, place, timeFrom, timeTo, price, seatId} = order!;

  const handleSubmit = () => {
    setErrors([]);
    axios.post('/api/v1/bookings', {
      seatId,
      bookingDate: date,
      startTime: `${String(timeFrom).padStart(2, '0')}:00`,
      endTime: `${String(timeTo).padStart(2, '0')}:00`,
    })
      .then(res => {
        window.open(res.data.booking.paymentUrl, '_self');
      })
      .catch(e => {
        if (e.response) {
          setErrors(e.response.data.error.details);
        } else {
          setErrors(['Не удалось авторизоваться']);
        }
      });
  }
  return (
    <div style={{paddingInline: 30}}>
      <ToHomeButton/>

      <h1 style={{marginBlock: 20}}>Оформление заказа</h1>
      <Flex gap={10} vertical align='start'>
        <div>{formatOrderTime(timeFrom, timeTo, date)}</div>
        <div>{address}</div>
        <div>Место: {place}</div>
        <div><b>{price * (timeTo - timeFrom)}р.</b></div>
        <Button onClick={handleSubmit}>Оплатить заказ</Button>
        {errors.map(error =>
          <ErrorText>{error}</ErrorText>
        )}
      </Flex>
    </div>
  );
};
