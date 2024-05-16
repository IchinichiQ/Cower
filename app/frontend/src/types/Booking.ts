export interface Booking {
  id: number,
  useId: number,
  seaId: number,
  createdAt: string,
  bookingDate: string,
  startTime: string,
  endTime: string,
  status: string,
  paymentUrl?: string,
  paymentExpireAt?: string;
  price: number,
  seatNumber: number,
  floor: number,
  coworkingAddress: string,
}

export interface NewBookingInfo {
  seatId: number;
  date: string;
  timeFrom: number;
  timeTo: number;
  address: string;
  place: number;
  price: number;
}

export enum PaymentStatus {
  AwaitingPayment = 'awaiting_payment',
  Paid = 'paid',
  InProgress = 'in_progress',
  Success = 'success',
  Cancelled = 'cancelled',
  PaymentTimeout = 'payment_timeout'
}

export const PaymentStatusLabel = {
  [PaymentStatus.AwaitingPayment]: 'Ожидает оплаты',
  [PaymentStatus.Paid]: 'Оплачен',
  [PaymentStatus.InProgress]: 'В процессе',
  [PaymentStatus.Success]: 'Завершён',
  [PaymentStatus.Cancelled]: 'Отменён',
  [PaymentStatus.PaymentTimeout]: 'Время оплаты истекло',
}
