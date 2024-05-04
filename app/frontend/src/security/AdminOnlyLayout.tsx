import ProtectedLayout from '@/security/ProtectedLayout.js';
import {FC} from "react";
import {UserRole} from "@/types/User";
import {useAuthorizedUser} from "@/redux/userSlice";

interface Props {
  redirectPath: string;
}

export const AdminOnlyLayout: FC<Props> = ({redirectPath}) => {
  const user = useAuthorizedUser();
  return <ProtectedLayout isAllowed={user.role === UserRole.ADMIN} redirectPath={redirectPath}/>;
};
