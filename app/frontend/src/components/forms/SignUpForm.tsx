import {useState} from 'react';
import {useFormik} from 'formik';
import {Button, Flex, Input} from 'antd';
import axios from 'axios';
import {useActions} from '@/redux/actions';
import {baseUrl} from '@/api';
import {ErrorText} from "@/styles/styles";

const SignUpForm = () => {
  const {setUser, setJwt} = useActions();
  const [error, setError] = useState('');

  const formik = useFormik({
    initialValues: {
      name: '',
      surname: '',
      phone: '',
      email: '',
      password: ''
    },
    onSubmit(values) {
      axios.post(baseUrl + '/user/register', values)
        .then(res => {
          setUser(res.data.user);
          setJwt(res.data.jwt);
          setError('');
        })
        .catch(e => {
          setError(e.detail || e.message);
        });
    },
  });

  return (
    <form onSubmit={formik.handleSubmit}>
      <Flex vertical gap={14}>
        <div>
          <label htmlFor="name">Имя:</label>
          <Input
            value={formik.values.name}
            onChange={formik.handleChange}
            id="name"
            name="name"
            type="text"
          />
        </div>
        <div>
          <label htmlFor="surname">Фамилия:</label>
          <Input
            value={formik.values.surname}
            onChange={formik.handleChange}
            id="surname"
            name="surname"
            type="text"
          />
        </div>
        <div>
          <label htmlFor="phone">Телефон:</label>
          <Input
            value={formik.values.phone}
            onChange={formik.handleChange}
            id="phone"
            name="phone"
            type="text"
          />
        </div>
        <div>
          <label htmlFor="email">Почта:</label>
          <Input
            value={formik.values.email}
            onChange={formik.handleChange}
            id="email"
            name="email"
            type="text"
          />
        </div>
        <div>
          <label htmlFor="password">Пароль:</label>
          <Input
            value={formik.values.password}
            onChange={formik.handleChange}
            id="password"
            name="password"
            type="password"
          />
        </div>
        <Button htmlType="submit">Зарегистрироваться</Button>
        <ErrorText>{error}</ErrorText>
      </Flex>
    </form>
  );
};

export default SignUpForm;
