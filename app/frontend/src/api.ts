import axios from 'axios';
import {store} from '@/redux';
import {userActions} from '@/redux/userSlice';

export const baseUrl = process.env.NODE_ENV === 'production' ? 'https://185.233.187.57:8081/api' : '/api';

axios.interceptors.request.use(config => {
  config.headers['Authorization'] = store.getState().user ? 'Bearer ' + store.getState().user?.jwt : null;
  return config;
});

axios.interceptors.response.use(
  response => response,
  error => {
    if (error.response.status === 401) {
      store.dispatch(userActions.clearUser())
    }
    return Promise.reject(error);
  }
);
