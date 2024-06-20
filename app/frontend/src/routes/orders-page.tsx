import { Container } from "@/styles/Container";
import { ToHomeButton } from "@/components/ToHomeButton";
import { Button, ConfigProvider, DatePicker, Flex, Select } from "antd";
import { BookingItem } from "@/components/BookingItem";
import { useEffect, useState } from "react";
import { Booking, PaymentStatus, PaymentStatusLabel } from "@/types/Booking";
import axios from "axios";
import { baseUrl } from "@/api";
import { useAuthorizedUser } from "@/redux/userSlice";
import { UserRole } from "@/types/User";
import { Coworking } from "@/types/Coworking";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCalendarDays } from "@fortawesome/free-solid-svg-icons";
import locale from "antd/es/locale/ru_RU";
import dayjs from "dayjs";
import updateLocale from "dayjs/plugin/updateLocale";

dayjs.extend(updateLocale);
dayjs.updateLocale("ru-ru", {
  weekStart: 0,
});
export const OrdersPage = () => {
  const [bookings, setBookings] = useState<Booking[]>([]);
  const [loading, setLoading] = useState(true);

  const user = useAuthorizedUser();

  const fetchBookings = () => {
    axios.get(`${baseUrl}/v1/bookings`).then((res) => {
      if ("data" in res) {
        setBookings(res.data.bookings);
        setFilteredBookings(res.data.bookings);
        setLoading(false);
      }
    });
  };

  const [coworkingList, setCoworkingList] = useState<Coworking[]>([]);
  const [selectedCoworkingId, setSelectedCoworkingId] = useState(-1);
  const [selectedFloorId, setSelectedFloorId] = useState(-1);

  useEffect(() => {
    fetchBookings();
    axios.get(`${baseUrl}/v1/coworkings`).then((response) => {
      if ("data" in response) {
        setCoworkingList(response.data.coworkings);
      }
    });
  }, []);

  const [dateFrom, setDateFrom] = useState("");
  const [dateTo, setDateTo] = useState("");
  const [time1From, setTime1From] = useState("");
  const [time1To, setTime1To] = useState("");
  const [time2From, setTime2From] = useState("");
  const [time2To, setTime2To] = useState("");

  const timeOptions = [];
  for (let i = 0; i < 24; i++) {
    for (let j = 0; j < 60; j += 10) {
      const t = `${String(i).padStart(2, "0")}:${String(j).padStart(2, "0")}`;
      timeOptions.push({
        label: t,
        value: t,
      });
    }
  }

  const [paymentStatus, setPaymentStatus] = useState<
    PaymentStatus | undefined
  >();

  const [filteredBookings, setFilteredBookings] = useState(bookings);

  const handleApplyFilters = () => {
    setFilteredBookings(
      bookings.filter((booking) => {
        if (
          selectedCoworkingId !== -1 &&
          coworkingList.find((c) => c.id === selectedCoworkingId)?.address !==
            booking.coworkingAddress
        ) {
          return false;
        }
        if (
          selectedFloorId !== -1 &&
          coworkingList
            .find((c) => c.id === selectedCoworkingId)
            ?.floors.find((f) => f.id === selectedFloorId)?.number !==
            booking.floor
        ) {
          return false;
        }
        if (paymentStatus && booking.status !== paymentStatus) {
          return false;
        }
        return true;
      })
    );
  };

  const handleResetFilters = () => {
    setSelectedCoworkingId(-1);
    setSelectedFloorId(-1);
    setPaymentStatus(undefined);
  };

  useEffect(() => {
    handleApplyFilters();
  }, [selectedCoworkingId, selectedFloorId, paymentStatus]);

  return (
    <Container>
      <ToHomeButton />

      <h1 style={{ marginBlock: 20 }}>
        {user.role === UserRole.ADMIN ? "Управление заказами" : "Мои заказы"}
      </h1>

      {/* Filters */}
      <Flex align="start" style={{ marginBottom: 40 }} vertical gap={20}>
        <Flex gap={10}>
          <div>
            <p>Коворкинг:</p>
            <Select
              style={{ width: 300 }}
              value={selectedCoworkingId}
              onChange={(v) => {
                setSelectedCoworkingId(v);
                setSelectedFloorId(-1);
              }}
              options={[
                { value: -1, label: "-" },
                ...coworkingList.map((item) => ({
                  label: item.address,
                  value: item.id,
                })),
              ]}
            />
          </div>
          <div>
            <p>Этаж:</p>
            <Select
              style={{ width: 300 }}
              value={selectedFloorId}
              onChange={setSelectedFloorId}
              options={[
                { value: -1, label: "-" },
                ...(coworkingList
                  ?.find((item) => item.id === selectedCoworkingId)
                  ?.floors.map((item) => ({
                    label: item.number,
                    value: item.id,
                  })) || []),
              ]}
            />
          </div>
        </Flex>

        <div>
          <p>Статус оплаты:</p>
          <Select
            style={{ width: 300 }}
            value={paymentStatus}
            onChange={setPaymentStatus}
            options={[
              { value: "", label: "-" },
              ...Object.keys(PaymentStatus)
                .filter((value) => isNaN(Number(value)))
                .map((key) => {
                  const status =
                    PaymentStatus[key as keyof typeof PaymentStatus];
                  return {
                    value: status,
                    label: PaymentStatusLabel[status],
                  };
                }),
            ]}
          />
        </div>

        <Flex gap={10}>
          {/*<Button onClick={handleApplyFilters}>Применить</Button>*/}
          <Button onClick={handleResetFilters}>Очистить</Button>
        </Flex>
      </Flex>

      {loading ? (
        <h3>Загрузка...</h3>
      ) : (
        <Flex vertical gap={24}>
          {filteredBookings &&
            filteredBookings
              .sort(
                (a, b) =>
                  new Date(b.createdAt).getTime() -
                  new Date(a.createdAt).getTime()
              )
              .map((booking) => (
                <BookingItem
                  key={booking.id}
                  booking={booking}
                  onCancel={fetchBookings}
                  onPaymentTimeExpired={fetchBookings}
                />
              ))}
          {!bookings.length && "Заказов пока нет"}
        </Flex>
      )}
    </Container>
  );
};
