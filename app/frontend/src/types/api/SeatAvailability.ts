export interface SeatAvailabilityRequest {
  date: string;
  seatIds: number[];
}

export interface SeatAvailabilityResponse {
  date: string;
  availability: Record<string, { from: string; to: string; }[]>
}
