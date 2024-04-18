import {useNavigate} from "react-router-dom";

export const ToHomeButton = () => {
  const navigate = useNavigate();

  return (
    <span
      onClick={() => navigate('/home')}
      style={{fontSize: 21, fontWeight: 700, textDecoration: 'underline', cursor: 'pointer'}}>На главную</span>
  );
};
