import {useAppSelector} from "@/redux";
import {Button, Flex} from "antd";
import {ToHomeButton} from "@/components/ToHomeButton";
import {useNavigate} from "react-router-dom";
import {useActions} from "@/redux/actions";
import {formatOrderTime} from "@/utils/formatOrderTime";
import ym from "react-yandex-metrika";

export const CheckoutPage = () => {
  const {order} = useAppSelector(state => state.order);

  const {address, date, place, timeFrom, timeTo} = order!;

  const navigate = useNavigate();

  const {addOrder} = useActions();
  const handleSubmit = () => {
    addOrder({
      ...order!,
      status: 'оплачен',
      cost: 400,
    })
    // ym('97166984','reachGoal','create-order');
    navigate('/payment-result');
  }
  return (
    <div style={{paddingInline: 30}}>
      <ToHomeButton/>

      <h1 style={{marginBlock: 20}}>Оформление заказа</h1>
      <Flex gap={10} vertical align='start'>
        <div>{formatOrderTime(timeFrom, timeTo, date)}</div>
        <div>{address}</div>
        <div>Место: {place}</div>
        <div><b>400р.</b></div>
        <Button onClick={handleSubmit}>Оплатить заказ</Button>
      </Flex>
    </div>
  );
};
