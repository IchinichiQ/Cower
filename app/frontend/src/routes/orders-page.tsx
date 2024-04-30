import {Container} from "@/styles/Container";
import {ToHomeButton} from "@/components/ToHomeButton";
import {Flex} from "antd";
import {OrderItem} from "@/components/OrderItem";
import {useEffect, useState} from "react";

export const OrdersPage = () => {
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setTimeout(() => setLoading(false), 1000);
  }, []);

  return (
    <Container>
      <ToHomeButton/>

      <h1 style={{marginBlock: 20}}>Мои заказы</h1>

      {loading ? <h3>Загрузка...</h3> :
        <Flex vertical gap={24}>
          <OrderItem address='ул. Пушкина 22, 2 этаж' cost={400} place={10} status='оплачен' time='Понедельник, 11 марта, 10:00 - 15:00' />
          <OrderItem address='ул. Пушкина 22, 2 этаж' cost={400} place={10} status='ожидает оплаты' time='Вторник, 12 марта, 17:00 - 18:00' />
        </Flex>
      }

    </Container>
  );
};
