import { User } from "@/types/User";

export interface Booking {
  id: number;
  useId: number;
  seaId: number;
  createdAt: string;
  bookingDate: string;
  startTime: string;
  endTime: string;
  status: PaymentStatus;
  paymentUrl?: string;
  paymentExpireAt?: string;
  initialPrice?: number;
  price: number;
  seatNumber: number;
  floor: number;
  coworkingAddress: string;
  isDiscountApplied: boolean;
  user?: User;
}

export interface NewBookingInfo {
  seatId: number;
  date: string;
  timeFrom: string;
  timeTo: string;
  address: string;
  place: number;
  price: number;
}

export enum PaymentStatus {
  AwaitingPayment = "awaiting_payment",
  Paid = "paid",
  InProgress = "in_progress",
  Success = "success",
  Cancelled = "cancelled",
  PaymentTimeout = "payment_timeout",
}

export const PaymentStatusLabel = {
  [PaymentStatus.AwaitingPayment]: "Ожидает оплаты",
  [PaymentStatus.Paid]: "Оплачен",
  [PaymentStatus.InProgress]: "В процессе",
  [PaymentStatus.Success]: "Завершён",
  [PaymentStatus.Cancelled]: "Отменён",
  [PaymentStatus.PaymentTimeout]: "Время оплаты истекло",
};
