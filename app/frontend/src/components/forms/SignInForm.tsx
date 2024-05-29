import {FormikErrors, useFormik} from 'formik';
import {Button, Flex, Input} from 'antd';
import {useState} from 'react';
import axios from 'axios';
import {useActions} from '@/redux/actions';
import {baseUrl} from '@/api';
import {ErrorText} from '@/styles/styles';
import ym from "react-yandex-metrika";

interface FormValues {
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
  }

  return errors;
}

const SignInForm = () => {
  const [errors, setErrors] = useState<string[]>([]);
  const {setUser, setJwt} = useActions();
  const [submitAttempted, setSubmitAttempted] = useState(false);

  const formik = useFormik<FormValues>({
    initialValues: {
      email: '',
      password: ''
    },
    validate(values) {
      setSubmitAttempted(true);
      return validate(values);
    },
    validateOnChange: submitAttempted,
    onSubmit(values) {
      setErrors([]);
      axios.post(baseUrl + '/v1/user/login', values)
        .then(res => {
          if (res.status === 200) {
            setUser(res.data.user);
            setJwt(res.data.jwt);
            setErrors([]);
          }
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
        {errors.map((error, index) =>
          <ErrorText key={index}>{error}</ErrorText>
        )}
      </Flex>
    </form>
  );
};

export default SignInForm;
