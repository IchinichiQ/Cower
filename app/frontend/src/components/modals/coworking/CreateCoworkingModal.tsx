import { Input, Modal } from "antd";
import { FC, useState } from "react";
import axios from "axios";
import { baseUrl } from "@/api";
import { workingDays } from "@/types/WorkingDays";
import {colors} from "@/styles/constants";

interface Props {
  open: boolean;

  onSubmit(): void;

  close(): void;
}

export const CreateCoworkingModal: FC<Props> = ({ close, onSubmit, open }) => {
  const [address, setAddress] = useState("");
  const [loading, setLoading] = useState(false);

  const workingTimes = workingDays.map((day) => ({
    day,
    open: "08:00",
    close: "18:00",
  }));

  const handleSubmit = () => {
    setLoading(true);
    axios
      .post(`${baseUrl}/v1/coworkings`, {
        address,
        workingTimes,
      })
      .then(() => {
        setLoading(false);
        onSubmit();
        close();
      });
  };

  return (
    <Modal
      title="Добавить коворкинг"
      open={open}
      onOk={handleSubmit}
      onCancel={close}
      okButtonProps={{ loading }}
      okText="Создать"
      cancelText="Отмена"
      okType="default"
      cancelButtonProps={{
        style: { background: "transparent", color: colors.dark },
      }}
    >
      <Input value={address} onChange={(e) => setAddress(e.target.value)} />
    </Modal>
  );
};
