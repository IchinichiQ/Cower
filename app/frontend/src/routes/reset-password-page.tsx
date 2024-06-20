import { useNavigate, useParams, useSearchParams } from "react-router-dom";
import { Button, Flex, Input } from "antd";
import { ToHomeButton } from "@/components/ToHomeButton";
import { useState } from "react";
import axios from "axios";
import { baseUrl } from "@/api";
import { ErrorText } from "@/styles/styles";

export const ResetPasswordPage = () => {
  const [params, _] = useSearchParams();
  const token = params.get("token");

  const [email, setEmail] = useState("");
  const [requestSent, setRequestSent] = useState(false);
  const [errors, setErrors] = useState<string[]>([]);

  const handleSendRequest = () => {
    axios
      .post(`${baseUrl}/v1/users/send-password-reset-token`, { email })
      .then(() => {
        setRequestSent(true);
      })
      .catch((e) => {
        if (e.response) {
          setErrors(e.response.data.error.details);
        } else {
          setErrors(["Не удалось отправить запрос"]);
        }
      });
  };

  const [newPassword, setNewPassword] = useState("");
  const [confirmNewPassword, setConfirmNewPassword] = useState("");
  const [passwordReset, setPasswordReset] = useState(false);

  const handleResetPassword = () => {
    if (newPassword.length < 8) {
      setErrors(["Пароль должен содержать минимум 8 символов"]);
      return;
    }
    setErrors([]);
    axios
      .post(`${baseUrl}/v1/users/reset-password`, {
        passwordResetToken: token,
        newPassword,
      })
      .then(() => {
        setPasswordReset(true);
      })
      .catch((e) => {
        if (e.response) {
          setErrors(e.response.data.error.details);
        } else {
          setErrors(["Не удалось сбрость пароль"]);
        }
      });
  };

  const navigate = useNavigate();

  return (
    <div style={{ paddingInline: 30 }}>
      <h1 style={{ marginBottom: 30 }}>Восстановление пароля</h1>

      {token ? (
        passwordReset ? (
          <>
            <p style={{ marginBottom: 15 }}>Пароль успешно обновлён</p>
            <span
              onClick={() => navigate("/home")}
              style={{
                fontSize: 21,
                fontWeight: 700,
                textDecoration: "underline",
                cursor: "pointer",
              }}
            >
              Авторизоваться
            </span>
          </>
        ) : (
          <Flex gap={10} vertical>
            <p>Введите новый пароль:</p>
            <Input
              type="password"
              style={{ maxWidth: 400 }}
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
            />
            <p>Подтвердите новый пароль:</p>
            <Input
              type="password"
              style={{ maxWidth: 400 }}
              value={confirmNewPassword}
              onChange={(e) => setConfirmNewPassword(e.target.value)}
            />
            {errors.map((error) => (
              <ErrorText key={error}>{error}</ErrorText>
            ))}
            <Button
              onClick={handleResetPassword}
              disabled={!(newPassword && newPassword === confirmNewPassword)}
              style={{ maxWidth: 400 }}
            >
              Сохранить
            </Button>
          </Flex>
        )
      ) : requestSent ? (
        <div>
          Вам на почту отправлено письмо с инструкцией для восстановления пароля
        </div>
      ) : (
        <Flex gap={10} vertical>
          <p>Введите почту, которую вы использовали при регистрации:</p>
          <Input
            style={{ maxWidth: 400 }}
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          {errors.map((error) => (
            <ErrorText key={error}>{error}</ErrorText>
          ))}
          <Button
            onClick={handleSendRequest}
            disabled={!email}
            style={{ maxWidth: 400 }}
          >
            Отправить письмо на почту
          </Button>
        </Flex>
      )}
    </div>
  );
};
