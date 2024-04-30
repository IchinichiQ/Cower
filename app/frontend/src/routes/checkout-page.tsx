import {useAppSelector} from "@/redux";
import {Container} from "@/styles/Container";
import {Button, Flex} from "antd";
import {ToHomeButton} from "@/components/ToHomeButton";

export const CheckoutPage = () => {
  const {order} = useAppSelector(state => state.order);

  const {address, date, place, timeFrom, timeTo} = order!;
  const timeString = `${String(timeFrom).padStart(2, '0')}:00 - ${String(timeTo).padStart(2, '0')}:00`

  return (
    <div style={{paddingInline: 30}}>
      <ToHomeButton />

      <h1 style={{marginBlock: 20}}>Оформление заказа</h1>
      <Flex gap={10} vertical align='start'>
        <div>{new Date(date).toLocaleDateString()}, {timeString}</div>
        <div>{address}</div>
        <div>Место: {place}</div>
        <div><b>400р.</b></div>
        <Button>Оплатить заказ</Button>
      </Flex>
    </div>
  );
};
