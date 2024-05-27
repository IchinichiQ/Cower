export interface Coworking {
  id: number;
  address: string;
  floors: Floor[];
  workingTime: { day: string; open: string; close: string; }[];
}

export interface Seat {
  id: number;
  coworkingId: number;
  price: number;
  description: string;
  image: { url: string };
  number: number;
  position: {
    x: number;
    y: number;
    width: number;
    height: number;
    angle: number;
  }
}

export interface Floor {
  id: number;
  coworkingId: number;
  number: number;
  image: { url: string };
  seats?: Seat[];
}
