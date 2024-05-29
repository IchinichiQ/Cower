import {useState} from 'react';
import {FormikErrors, useFormik} from 'formik';
import {Button, Flex, Input} from 'antd';
import axios from 'axios';
import {useActions} from '@/redux/actions';
import {baseUrl} from '@/api';
import {ErrorText} from "@/styles/styles";
import ym from "react-yandex-metrika";

interface FormValues {
  name: string;
  surname: string;
  phone: string;
  email: string;
  password: string;
}

const validate = (values: FormValues) => {
  const errors: FormikErrors<FormValues> = {};

  if (!values.email.length) {
    errors.email = 'Поле обязательно';
  }

  if (!values.password.length) {
    errors.password = 'Поле обязательно';
  } else if (values.password.length < 8) {
    errors.password = 'Пароль должен содержать минимум 8 символов';
  }

  return errors;
}

const SignUpForm = () => {
  const [errors, setErrors] = useState<string[]>([]);
  const {setUser, setJwt} = useActions();
  const [submitAttempted, setSubmitAttempted] = useState(false);

  const formik = useFormik({
    initialValues: {
      name: '',
      surname: '',
      phone: '',
      email: '',
      password: ''
    },
    validate(values) {
      setSubmitAttempted(true);
      return validate(values);
    },
    validateOnChange: submitAttempted,
    onSubmit(values) {
      axios.post(baseUrl + '/v1/users/register', values)
        .then(res => {
          setUser(res.data.user);
          setJwt(res.data.jwt);
          setErrors([]);
          ym('reachGoal','register');
        })
        .catch(e => {
          if (e.response) {
            setErrors(e.response.data.error.details);
          } else {
            setErrors(['Не удалось авторизоваться']);
          }
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
          <ErrorText>{formik.errors.email}</ErrorText>
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
          <ErrorText>{formik.errors.password}</ErrorText>
        </div>
        <Button htmlType="submit">Зарегистрироваться</Button>
        {errors.map((error, index) =>
          <ErrorText key={index}>{error}</ErrorText>
        )}
      </Flex>
    </form>
  );
};

export default SignUpForm;
