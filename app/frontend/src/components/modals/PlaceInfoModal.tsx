import { PlaceInfo } from "@/types/PlaceInfo";
import { FC, ReactNode } from "react";
import { Button, Popover } from "antd";
import { formatOrderTime } from "@/utils/formatOrderTime";
import { useAppSelector } from "@/redux";
import { useNavigate } from "react-router-dom";

interface Props {
  info: PlaceInfo;

  onSubmit(): void;

  children: ReactNode;
}

export const PlaceInfoModal: FC<Props> = ({ info, children, onSubmit }) => {
  const { date, place, timeFrom, timeTo } = info;
  const { user } = useAppSelector((state) => state.user);
  const navigate = useNavigate();

  return (
    <Popover
      trigger={["click"]}
      content={
        <div style={{ minWidth: 250, maxWidth: 400 }}>
          <h2>Выбор места</h2>
          {date && timeFrom && timeTo && (
            <div>{formatOrderTime(timeFrom, timeTo, date)}</div>
          )}
          <div>Место {place}</div>
          <div style={{ whiteSpace: "normal" }}>{info.description}</div>
          <div>
            <b>Цена: {info.price}р/час</b>
          </div>

          {user ? (
            <Button onClick={onSubmit} style={{ width: "100%", marginTop: 15 }}>
              Забронировать
            </Button>
          ) : (
            <Button
              onClick={() => navigate("/sign-in")}
              style={{ width: "100%", marginTop: 15 }}
            >
              Авторизоваться
            </Button>
          )}
        </div>
      }
    >
      {children}
    </Popover>
  );
};
