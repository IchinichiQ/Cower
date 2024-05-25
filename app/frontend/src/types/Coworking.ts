export interface Coworking {
  id: number;
  address: string;
  floors: number;
  workingTime: { day: string; open: string; close: string; }[];
}

export interface Seat {
  id: number;
  coworkingId: number;
  price: number;
  description: string;
  image: string;
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
  coworkingId: number;
  floor: number;
  backgroundImage: string;
  seats: Seat[]
}
