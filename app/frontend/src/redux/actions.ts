import {bindActionCreators} from '@reduxjs/toolkit';
import {userActions} from '@/redux/userSlice';
import {orderActions} from "@/redux/orderSlice";
import {useAppDispatch} from "@/redux/index";
import {ordersActions} from "@/redux/ordersSlice";

const actions = {
  ...userActions,
  ...orderActions,
  ...ordersActions,
};

export function useActions() {
  return bindActionCreators(actions, useAppDispatch());
}
