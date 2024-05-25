import {NewBookingInfo} from "@/types/Booking";
import {createSlice, PayloadAction} from "@reduxjs/toolkit";

export type OrdersState = NewBookingInfo[];

const initialState: OrdersState = [];

const ordersSlice = createSlice({
  name: 'orders',
  initialState,
  reducers: {
    addOrder(state, action: PayloadAction<NewBookingInfo>) {
      state.push(action.payload);
    },
  }
})

export const ordersActions = ordersSlice.actions;
export const ordersReducer = ordersSlice.reducer;
