import ProtectedLayout from '@/security/ProtectedLayout';
import {FC} from "react";
import {useAppSelector} from "@/redux";

interface Props {
  redirectPath: string;
}

export const OrderPresentLayout: FC<Props> = ({redirectPath}) => {
  const {order} = useAppSelector(state => state.order);
  return <ProtectedLayout isAllowed={order !== null} redirectPath={redirectPath}/>;
};
