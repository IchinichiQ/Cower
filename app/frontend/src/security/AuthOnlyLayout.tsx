import ProtectedLayout from '@/security/ProtectedLayout';
import {FC} from "react";
import {useAppSelector} from "@/redux";

interface Props {
  redirectPath: string;
}

const AuthOnlyLayout: FC<Props> = ({redirectPath}) => {
  const {user} = useAppSelector(state => state.user);
  return <ProtectedLayout isAllowed={user !== null} redirectPath={redirectPath}/>;
};

export default AuthOnlyLayout;
