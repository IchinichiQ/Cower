import {Outlet} from 'react-router-dom';
import {GlobalStyle} from '@/styles/GlobalStyle';
import {Navbar} from '@/components/Navbar';
import {useEffect} from 'react';
import {useActions} from "@/redux/actions";
import {ConfigProvider} from "antd";
import {colors} from "@/styles/constants";
import {baseUrl} from "@/api";
import axios from "axios";

export const Root = () => {
  const {setUser, clearUser} = useActions();

  useEffect(() => {
    axios.get(baseUrl + '/user/me')
      .then(res => {
        setUser(res.data.user);
      })
      .catch(e => {
        clearUser();
      });
  }, []);

  return (
    <ConfigProvider theme={{
      token: {
        colorBgBase: colors.light,
        colorText: colors.dark,
        borderRadius: 5,
        colorBorder: colors.grid,
        colorPrimaryHover: colors.tableBorder,
        colorPrimary: 'rgb(171 171 171)',
      },
      components: {
        Button: {
          defaultActiveBg: colors.dark,
          defaultBg: colors.dark,
          defaultColor: colors.darker,
          defaultHoverBg: colors.dark
        },
        Popover: {
          colorBgElevated: colors.darker
        }
      }
    }}>
      <GlobalStyle/>
      <Navbar/>
      <Outlet/>
    </ConfigProvider>
  );
};
