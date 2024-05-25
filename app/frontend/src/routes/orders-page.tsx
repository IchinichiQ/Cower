import {Container} from "@/styles/Container";
import {ToHomeButton} from "@/components/ToHomeButton";
import {Flex} from "antd";
import {BookingItem} from "@/components/BookingItem";
import {useEffect, useState} from "react";
import {useAppSelector} from "@/redux";
import {formatOrderTime} from "@/utils/formatOrderTime";
import {Booking, PaymentStatus, PaymentStatusLabel} from "@/types/Booking";
import axios from "axios";
import {baseUrl} from "@/api";

export const OrdersPage = () => {
  const [bookings, setBookings] = useState<Booking[]>([])
  const [loading, setLoading] = useState(true);

  const fetchBookings = () => {
    axios.get(`${baseUrl}/v1/bookings`)
      .then(res => {
        if ('data' in res) {
          setBookings(res.data.bookings);
          setLoading(false);
        }
      })
  }

  useEffect(() => {
    fetchBookings();
  }, []);

  return (
    <Container>
      <ToHomeButton/>

      <h1 style={{marginBlock: 20}}>Мои заказы</h1>

      {loading ? <h3>Загрузка...</h3> :
        <Flex vertical gap={24}>
          {bookings.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()).map(booking =>
            <BookingItem
              key={booking.id}
              booking={booking}
              onCancel={fetchBookings}
            />
          )}
          {!bookings.length && 'Заказов пока нет'}
        </Flex>
      }

    </Container>
  );
};
