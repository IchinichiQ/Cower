import { Input, Modal } from "antd";
import { FC, useState } from "react";
import axios from "axios";
import { baseUrl } from "@/api";
import { Coworking } from "@/types/Coworking";
import {colors} from "@/styles/constants";

interface Props {
  coworking: Coworking;

  open: boolean;

  onSubmit(): void;

  close(): void;
}

export const EditCoworkingModal: FC<Props> = ({
  coworking,
  onSubmit,
  close,
  open,
}) => {
  const [address, setAddress] = useState(coworking.address);
  const [loading, setLoading] = useState(false);

  const handleSubmit = () => {
    setLoading(true);
    axios
      .patch(`${baseUrl}/v1/coworkings/${coworking.id}`, {
        address,
        workingTime: coworking.workingTime,
      })
      .then(() => {
        setLoading(false);
        onSubmit();
        close();
      });
  };

  return (
    <Modal
      title="Редактировать коворкинг"
      open={open}
      onOk={handleSubmit}
      onCancel={close}
      okButtonProps={{ loading }}
      okText="Сохранить"
      okType="default"
      cancelText="Отмена"
      cancelButtonProps={{
        style: { background: "transparent", color: colors.dark },
      }}
    >
      <Input value={address} onChange={(e) => setAddress(e.target.value)} />
    </Modal>
  );
};
