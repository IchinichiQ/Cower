import {PlaceInfo} from "@/types/PlaceInfo";
import {FC, ReactNode} from "react";
import {Button, Popover} from "antd";
import {formatOrderTime} from "@/utils/formatOrderTime";

interface Props {
  info: PlaceInfo;
  onSubmit(): void;
  children: ReactNode;
}

export const PlaceInfoModal: FC<Props> = ({info, children, onSubmit}) => {
  const {date, place, timeFrom, timeTo} = info;
  return (
    <Popover
      trigger={['click']}
      content={<div style={{minWidth: 250}}>
        <h2>Выбор места</h2>
        <div>{formatOrderTime(timeFrom, timeTo, date)}</div>
        <div>Место {place}</div>
        <Button onClick={onSubmit} style={{width: '100%', marginTop: 15}}>Забронировать</Button>
      </div>}
    >
      {children}
    </Popover>
  );
};
