import ReactDOM from 'react-dom/client';
import {createHashRouter, Navigate, RouterProvider} from 'react-router-dom';
import {Root} from '@/routes/root';
import {Provider} from 'react-redux';
import {persistor, store} from '@/redux';
import NoAuthOnlyLayout from '@/security/NoAuthOnlyLayout';
import AuthOnlyLayout from '@/security/AuthOnlyLayout';
import SignUpPage from '@/routes/sign-up-page';
import SignInPage from '@/routes/sign-in-page';
import {PersistGate} from 'redux-persist/integration/react';
import {HomePage} from "@/routes/home-page";
import {OrdersPage} from "@/routes/orders-page";
import {ProfilePage} from "@/routes/profile-page";
import {OrderPresentLayout} from "@/security/OrderPresentLayout";
import {CheckoutPage} from "@/routes/checkout-page";

const router = createHashRouter([
  {
    path: '/',
    element: <Root/>,
    children: [
      {
        index: true,
        element: <Navigate to="/sign-in"/>
      },
      {
        element: <NoAuthOnlyLayout redirectPath="/home"/>,
        children: [
          {
            path: 'sign-up',
            element: <SignUpPage/>
          },
          {
            path: 'sign-in',
            element: <SignInPage/>
          }
        ]
      },
      {
        element: <AuthOnlyLayout redirectPath="/sign-in"/>,
        children: [
          {
            path: 'home',
            element: <HomePage/>
          },
          {
            path: 'checkout',
            element: <OrderPresentLayout redirectPath='/home'/>,
            children: [
              {
                index: true,
                element: <CheckoutPage/>
              },
            ]
          },
          {
            path: 'orders',
            element: <OrdersPage/>
          },
          {
            path: 'profile',
            element: <ProfilePage/>
          },
        ]
      }
    ]
  }
]);

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <Provider store={store}>
    <PersistGate persistor={persistor}>
      <RouterProvider router={router}/>
    </PersistGate>
  </Provider>
);
