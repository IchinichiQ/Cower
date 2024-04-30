import {bindActionCreators} from '@reduxjs/toolkit';
import {userActions} from '@/redux/userSlice';
import {orderActions} from "@/redux/orderSlice";
import {useAppDispatch} from "@/redux/index";

const actions = {
  ...userActions,
  ...orderActions,
};

export function useActions() {
  return bindActionCreators(actions, useAppDispatch());
}
