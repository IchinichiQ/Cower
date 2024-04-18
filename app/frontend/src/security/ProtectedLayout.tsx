import {Navigate, Outlet} from 'react-router-dom';
import {FC} from "react";

interface Props {
  redirectPath: string;
  isAllowed: boolean;
}

const ProtectedLayout: FC<Props> = ({redirectPath, isAllowed}) => {
  if (!isAllowed) {
    return <Navigate to={redirectPath} replace/>;
  }
  return <Outlet/>;
};

export default ProtectedLayout;
