import {Order} from "@/types/Order";
import {createSlice, PayloadAction} from "@reduxjs/toolkit";

export interface OrderState {
  order: Order | null;
}

const initialState: OrderState = {
  order: null
}

const orderSlice = createSlice({
  name: 'order',
  initialState,
  reducers: {
    setOrder(state, action: PayloadAction<Order | null>) {
      state.order = action.payload;
    },
  }
})

export const orderActions = orderSlice.actions;
export const orderReducer = orderSlice.reducer;
