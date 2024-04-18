import {bindActionCreators} from '@reduxjs/toolkit';
import {useDispatch} from 'react-redux';
import {userActions} from '@/redux/userSlice';

const actions = {
  ...userActions
};

export function useActions() {
  return bindActionCreators(actions, useDispatch());
}
