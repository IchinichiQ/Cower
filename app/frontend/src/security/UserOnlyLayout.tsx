import ProtectedLayout from '@/security/ProtectedLayout';
import {useAuthorizedUser} from '@/redux/userSlice.js';
import {FC} from "react";
import {UserRole} from "@/types/User";

interface Props {
  redirectPath: string;
}

export const UserOnlyLayout: FC<Props> = ({redirectPath}) => {
  const user = useAuthorizedUser();
  return <ProtectedLayout isAllowed={user.role === UserRole.USER} redirectPath={redirectPath}/>;
};
