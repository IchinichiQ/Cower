import {useAppSelector} from "@/redux";
import {ToHomeButton} from "@/components/ToHomeButton";
import {Button, Flex} from "antd";
import {useEffect, useState} from "react";
import {Container} from "@/styles/Container";

export const PaymentResultPage = () => {
  const {order} = useAppSelector(state => state.order);

  const {address, date, place, timeFrom, timeTo} = order!;
  const timeString = `${String(timeFrom).padStart(2, '0')}:00 - ${String(timeTo).padStart(2, '0')}:00`

  const [loading, setLoading] = useState(true);
  useEffect(() => {
    setTimeout(() => setLoading(false), 1000);
  }, []);

  return (
    <div style={{paddingInline: 30}}>
      {loading ? 'Загрузка...' :
        <>
          <ToHomeButton />

          <h1 style={{marginBlock: 20}}>Заказ успешно оплачен!</h1>

          <Flex gap={10} vertical align='start'>
            <div>{new Date(date).toLocaleDateString()}, {timeString}</div>
            <div>{address}</div>
            <div>Место: {place}</div>
            <div><b>400р.</b></div>
          </Flex>
        </>
      }
    </div>
  );
};
