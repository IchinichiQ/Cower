import {useFormik} from 'formik';
import {Button, Flex, Input} from 'antd';
import {useState} from 'react';
import axios from 'axios';
import {useActions} from '@/redux/actions';
import {baseUrl} from '@/api';
import {ErrorText} from '@/styles/styles';

const SignInForm = () => {
  const [error, setError] = useState('');
  const {setUser, setJwt} = useActions();

  const formik = useFormik({
    initialValues: {
      email: '',
      password: ''
    },
    onSubmit(values) {
      axios.post(baseUrl + '/user/login', values)
        .then(res => {
          if (res.status === 200) {
            setUser(res.data.user);
            setJwt(res.data.jwt);
            setError('');
          }
        })
        .catch(e => {
          setError('Не удалось авторизоваться');
        });
    },
  });

  return (
    <form onSubmit={formik.handleSubmit}>
      <Flex gap={14} vertical>
        <div>
          <label htmlFor="email">Почта:</label>
          <Input
            type="text"
            name="email"
            id="email"
            value={formik.values.email}
            onChange={formik.handleChange}
          />
          <ErrorText>{formik.errors.email}</ErrorText>
        </div>
        <div>
          <label htmlFor="password">Пароль:</label>
          <Input
            type="password"
            name="password"
            id="password"
            value={formik.values.password}
            onChange={formik.handleChange}
          />
          <ErrorText>{formik.errors.password}</ErrorText>
        </div>
        <Button htmlType="submit">Войти</Button>
        <ErrorText>{error}</ErrorText>
      </Flex>
    </form>
  );
};

export default SignInForm;
