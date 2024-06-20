import { Flex, Input, Modal, Select } from "antd";
import { FC, useState } from "react";
import axios from "axios";
import { baseUrl } from "@/api";
import { workingDays } from "@/types/WorkingDays";
import { colors } from "@/styles/constants";
import { hourOptions, minuteOptions } from "@/constants/timeOptions";

interface Props {
  open: boolean;
  onSubmit(): void;
  close(): void;
}

export const CreateCoworkingModal: FC<Props> = ({ close, onSubmit, open }) => {
  const [address, setAddress] = useState("");
  const [loading, setLoading] = useState(false);

  const [workingTime, setWorkingTime] = useState(
    workingDays.map((day) => ({
      day,
      open: "08:00",
      close: "18:00",
    }))
  );

  const handleHourChange = (
    day: string,
    action: "open" | "close",
    hour: number
  ) => {
    setWorkingTime(
      workingTime.map((time) => {
        if (time.day === day) {
          return {
            ...time,
            [action]:
              String(hour).padStart(2, "0") + ":" + time[action].split(":")[1],
          };
        }
        return time;
      })
    );
  };

  const handleMinuteChange = (
    day: string,
    action: "open" | "close",
    minute: number
  ) => {
    setWorkingTime(
      workingTime.map((time) => {
        if (time.day === day) {
          return {
            ...time,
            [action]:
              time[action].split(":")[0] +
              ":" +
              String(minute).padStart(2, "0"),
          };
        }
        return time;
      })
    );
  };

  const handleSubmit = () => {
    setLoading(true);
    axios
      .post(`${baseUrl}/v1/coworkings`, {
        address,
        workingTimes: workingTime,
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
      <Flex style={{ marginBlock: 20 }} vertical gap={5}>
        {workingTime.map(({ day, open, close }) => (
          <Flex gap={5} align="center" key={day}>
            <span style={{ width: 100 }}>{day}</span>

            <Select
              value={Number(open.split(":")[0])}
              options={hourOptions}
              onChange={(v) => handleHourChange(day, "open", v)}
            />
            <Select
              value={Number(open.split(":")[1])}
              options={minuteOptions}
              onChange={(v) => handleMinuteChange(day, "open", v)}
            />

            <span style={{ marginInline: 5 }}>-</span>

            <Select
              value={Number(close.split(":")[0])}
              options={hourOptions}
              onChange={(v) => handleHourChange(day, "close", v)}
            />
            <Select
              value={Number(close.split(":")[1])}
              options={minuteOptions}
              onChange={(v) => handleMinuteChange(day, "close", v)}
            />
          </Flex>
        ))}
      </Flex>
    </Modal>
  );
};
