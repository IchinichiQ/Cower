import {createSlice, PayloadAction} from '@reduxjs/toolkit';
import {User} from "@/types/User";
import {useAppSelector} from "@/redux/index";

export interface UserState {
  user: User | null;
  jwt: string;
  loading: boolean;
}

const initialState: UserState = {
  user: null,
  jwt: '',
  loading: true,
};

const userSlice = createSlice({
  name: 'user',
  initialState,
  reducers: {
    setUser(state, action: PayloadAction<User | null>) {
      state.user = action.payload;
    },
    setUserLoading(state, action: PayloadAction<boolean>) {
      state.loading = action.payload;
    },
    setJwt(state, action: PayloadAction<string>) {
      state.jwt = action.payload;
    },
    clearUser() {
      return {
        user: null,
        jwt: '',
        loading: false,
      }
    },
  }
});

export const userActions = userSlice.actions;
export const userReducer = userSlice.reducer;

export function useAuthorizedUser() {
  const {user} = useAppSelector(state => state.user);
  if (user === null) {
    throw new Error('User is null');
  }
  return user;
}
