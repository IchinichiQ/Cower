import {Order} from "@/types/Order";
import {createSlice, PayloadAction} from "@reduxjs/toolkit";

export type OrdersState = Order[];

const initialState: OrdersState = [

];

const ordersSlice = createSlice({
  name: 'orders',
  initialState,
  reducers: {
    addOrder(state, action: PayloadAction<Order>) {
      state.push(action.payload);
    },
  }
})

export const ordersActions = ordersSlice.actions;
export const ordersReducer = ordersSlice.reducer;
