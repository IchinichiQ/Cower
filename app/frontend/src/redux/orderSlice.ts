import {NewBookingInfo} from "@/types/Booking";
import {createSlice, PayloadAction} from "@reduxjs/toolkit";

export interface OrderState {
  order: NewBookingInfo | null;
}

const initialState: OrderState = {
  order: null
}

const orderSlice = createSlice({
  name: 'order',
  initialState,
  reducers: {
    setOrder(state, action: PayloadAction<NewBookingInfo | null>) {
      state.order = action.payload;
    },
  }
})

export const orderActions = orderSlice.actions;
export const orderReducer = orderSlice.reducer;
