import { Flex, Input, Modal, Select } from "antd";
import { FC, useState } from "react";
import axios from "axios";
import { baseUrl } from "@/api";
import { Coworking } from "@/types/Coworking";
import { colors } from "@/styles/constants";
import { hourOptions, minuteOptions } from "@/constants/timeOptions";
import { weekdayLabels } from "@/constants/weekdayLabels";

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
  const [workingTimes, setWorkingTimes] = useState(coworking.workingTime);
  const [loading, setLoading] = useState(false);

  const handleHourChange = (
    day: string,
    action: "open" | "close",
    hour: number
  ) => {
    setWorkingTimes(
      workingTimes.map((time) => {
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
    setWorkingTimes(
      workingTimes.map((time) => {
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
      .patch(`${baseUrl}/v1/coworkings/${coworking.id}`, {
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
      <Flex style={{ marginBlock: 20 }} vertical gap={5}>
        {workingTimes.map(({ day, open, close }) => (
          <Flex gap={5} align="center" key={day}>
            <span style={{ width: 100 }}>
              {weekdayLabels[day as keyof typeof weekdayLabels]}
            </span>

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
