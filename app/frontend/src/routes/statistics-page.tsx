import { useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faCalendarDays,
  faCircleNotch,
} from "@fortawesome/free-solid-svg-icons";
import {
  Button,
  ConfigProvider,
  DatePicker,
  Flex,
  Segmented,
  Select,
} from "antd";
import axios from "axios";
import { baseUrl } from "@/api";
import { ErrorText } from "@/styles/styles";
import styled, { keyframes } from "styled-components";
import { Line } from "@ant-design/charts";
import locale from "antd/es/locale/ru_RU";
import dayjs from "dayjs";
import updateLocale from "dayjs/plugin/updateLocale";

dayjs.extend(updateLocale);
dayjs.updateLocale("ru-ru", {
  weekStart: 0,
});

enum Step {
  DAY,
  WEEK,
  MONTH,
  YEAR,
}

const stepLabels = {
  [Step.DAY]: "День",
  [Step.WEEK]: "Неделя",
  [Step.MONTH]: "Месяц",
  [Step.YEAR]: "Год",
};

enum BookingsStatistics {
  COUNT,
  PRICE,
}

const bookingsStatisticsLabels = {
  [BookingsStatistics.COUNT]: "Количество заказов",
  [BookingsStatistics.PRICE]: "Прибыль",
};

const kf = keyframes`
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
`;

const LoaderWrapper = styled("div")`
  display: flex;
  animation: ${kf} 1s linear infinite;
`;

export const StatisticsPage = () => {
  const [data, setData] = useState<any>();
  const [statisticsType, setStatisticsType] = useState(
    BookingsStatistics.COUNT
  );
  const [step, setStep] = useState<Step>(Step.DAY);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<string[]>([]);

  const fetchStatistics = () => {
    setLoading(true);
    axios
      .get(
        `${baseUrl}/v1/statistics/bookings-${statisticsType === BookingsStatistics.COUNT ? "count" : "price"}?startDate=${startDate}&endDate=${endDate}&step=${Step[step]}`
      )
      .then((res) => {
        if (res.status === 200) {
          setData(res.data);
          setErrors([]);
        }
      })
      .catch((e) => {
        setData(undefined);
        if (e.response) {
          setErrors(e.response.data.error.details);
        } else {
          setErrors(["Не удалось загрузить статистику"]);
        }
      })
      .finally(() => setLoading(false));
  };

  const getChartConfig = () => {
    if (!data) {
      return null;
    }

    const d = new Date(data.startDate);
    const chartData = [];
    for (const value of data.values) {
      chartData.push({
        date: d.toLocaleDateString(),
        value,
        title:
          statisticsType === BookingsStatistics.COUNT
            ? "Количество заказов"
            : "Прибыль (руб)",
      });
      if (step === Step.DAY) {
        d.setDate(d.getDate() + 1);
      } else if (step === Step.WEEK) {
        d.setDate(d.getDate() + 7);
      } else if (step === Step.MONTH) {
        d.setMonth(d.getMonth() + 1);
      } else {
        d.setFullYear(d.getFullYear() + 1);
      }
    }

    return {
      locale,
      data: chartData,
      xField: (d: any) => d.date,
      yField: "value",
      meta: {
        value: {
          alias: " ",
        },
      },
      colorField: "title",
      interaction: {
        tooltip: {
          marker: false,
        },
      },
    };
  };

  const config = getChartConfig();

  return (
    <div style={{ paddingInline: 30 }}>
      <h1 style={{ marginBottom: 30 }}>Статистика заказов</h1>

      <Flex style={{ alignItems: "flex-start" }} vertical gap={20}>
        <Flex>
          <label style={{ width: 200 }}>Начало периода:</label>
          <DatePicker
            suffixIcon={<FontAwesomeIcon fill={"blue"} icon={faCalendarDays} />}
            bordered={false}
            style={{
              padding: 0,
              fontFamily: "inherit",
              fontWeight: 700,
            }}
            onChange={(_, dateStr) => setStartDate(dateStr.toString())}
          />
        </Flex>
        <Flex>
          <label style={{ width: 200 }}>Конец периода:</label>
          <DatePicker
            suffixIcon={<FontAwesomeIcon fill={"blue"} icon={faCalendarDays} />}
            bordered={false}
            style={{
              padding: 0,
              fontFamily: "inherit",
              fontWeight: 700,
            }}
            onChange={(_, dateStr) => setEndDate(dateStr.toString())}
          />
        </Flex>

        <Flex>
          <label style={{ width: 200 }}>Шаг: </label>
          <Select
            style={{ width: 200 }}
            value={step}
            onChange={setStep}
            options={Object.keys(Step)
              .filter((value) => isNaN(Number(value)))
              .map((key) => {
                const step = Step[key as keyof typeof Step];
                return {
                  value: step,
                  label: stepLabels[step],
                };
              })}
          />
        </Flex>

        <Segmented
          options={Object.keys(BookingsStatistics)
            .filter((value) => isNaN(Number(value)))
            .map((key) => {
              const stat =
                BookingsStatistics[key as keyof typeof BookingsStatistics];
              return {
                value: stat,
                label: bookingsStatisticsLabels[stat],
              };
            })}
          onChange={setStatisticsType}
        />
      </Flex>

      {!!errors.length && (
        <div style={{ marginTop: 30 }}>
          {errors.map((error, index) => (
            <ErrorText key={index}>{error}</ErrorText>
          ))}
        </div>
      )}

      <Flex style={{ marginTop: 30 }} align="center" gap={20}>
        <Button onClick={fetchStatistics}>Загрузить статистику</Button>
        {loading && (
          <LoaderWrapper>
            <FontAwesomeIcon icon={faCircleNotch} />
          </LoaderWrapper>
        )}
      </Flex>

      {config && (
        <ConfigProvider locale={locale}>
          <Line {...config} />
        </ConfigProvider>
      )}
    </div>
  );
};
