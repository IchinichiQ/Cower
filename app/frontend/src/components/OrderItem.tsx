import styled from "styled-components";
import {colors} from "@/styles/constants";
import {Button, Flex} from "antd";
import {FC} from "react";

const StyledItem = styled(Flex)`
  border-radius: 5px;
  border: 2px solid ${colors.dark};
  padding: 12px;
  font-size: 24px;
`

interface Props {
  time: string;
  address: string;
  place: number;
  status: string;
  cost: number;
}

export const OrderItem: FC<Props> = ({time, address, place, status, cost}) => {
  return (
    <StyledItem justify='space-between'>
      <div>
        <div>{time}</div>
        <div>{address}</div>
        <div>место {place}</div>
        <div>статус: <b><i>{status}</i></b></div>
      </div>
      <Flex vertical align='end' justify='space-between'>
        <div>{cost}р.</div>
        <Flex gap={4}>
          {status === 'ожидает оплаты' && <Button>Оплатить</Button>}
          <Button>Отменить</Button>
        </Flex>
      </Flex>
    </StyledItem>
  );
};
