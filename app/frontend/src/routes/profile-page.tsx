import { Container } from "@/styles/Container";
import { ToHomeButton } from "@/components/ToHomeButton";
import { useAuthorizedUser } from "@/redux/userSlice";
import { Button, Flex, Input } from "antd";
import { useEffect, useState } from "react";
import axios from "axios";
import { baseUrl } from "@/api";
import { useActions } from "@/redux/actions";
import { ErrorText } from "@/styles/styles";

export const ProfilePage = () => {
  const user = useAuthorizedUser();
  const { setUser } = useActions();

  // edit info
  const [editing, setEditing] = useState(false);
  const [info, setInfo] = useState({
    phone: "",
    name: "",
    email: "",
    surname: "",
  });

  useEffect(() => {
    setInfo({
      name: user.name,
      surname: user.surname,
      phone: user.phone,
      email: user.email,
    });
  }, [user]);

  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<string[]>([]);

  const handleChange = (key: string, value: string) => {
    setInfo({ ...info, [key as keyof typeof info]: value });
  };

  const handleCancel = () => {
    setInfo(user);
    setErrors([]);
    setEditing(false);
  };

  const handleSave = () => {
    setLoading(true);
    axios
      .patch(
        baseUrl + "/v1/users/me",
        Object.fromEntries(
          Object.entries(info).filter(
            ([key, value]) => user[key as keyof typeof user] !== value,
          ),
        ),
      )
      .then((res) => {
        if (res.status === 200) {
          setUser(res.data.user);
          setErrors([]);
          setEditing(false);
        }
      })
      .catch((e) => {
        if (e.response) {
          setErrors(e.response.data.error.details);
        } else {
          setErrors(["Не удалось изменить данные"]);
        }
      })
      .finally(() => setLoading(false));
  };

  // reset password
  const [resettingPassword, setResettingPassword] = useState(false);
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [passwordLoading, setPasswordLoading] = useState(false);
  const [passwordErrors, setPasswordErrors] = useState<string[]>([]);

  const handlePasswordCancel = () => {
    setResettingPassword(false);
    setNewPassword("");
    setConfirmPassword("");
    setPasswordErrors([]);
  };

  const handlePasswordSave = () => {
    if (newPassword.length < 8) {
      setPasswordErrors(["Пароль должен содержать минимум 8 символов"]);
      return;
    }
    setErrors([]);
    axios
      .patch(baseUrl + "/v1/users/me", { password: newPassword })
      .then((res) => {
        if (res.status === 200) {
          setPasswordErrors([]);
          setPasswordLoading(false);
          setResettingPassword(false);
          setNewPassword("");
          setConfirmPassword("");
        }
      })
      .catch((e) => {
        if (e.response) {
          setPasswordErrors(e.response.data.error.details);
        } else {
          setPasswordErrors(["Не удалось изменить данные"]);
        }
      })
      .finally(() => setPasswordLoading(false));
  };

  return (
    <Container>
      <ToHomeButton />

      <Flex justify="space-between" align="center">
        <h1 style={{ marginBlock: 20 }}>Личный кабинет</h1>
        <Button onClick={() => setEditing(true)}>Редактировать</Button>
      </Flex>

      <Flex style={{ fontSize: 24 }} vertical gap={12}>
        <div>
          <label htmlFor="name">Имя:</label>
          <Input
            style={{ fontSize: 20 }}
            disabled={!editing}
            value={info.name}
            onChange={(e) => handleChange("name", e.target.value)}
            id="name"
            name="name"
            type="text"
          />
        </div>
        <div>
          <label htmlFor="surname">Фамилия:</label>
          <Input
            style={{ fontSize: 20 }}
            disabled={!editing}
            value={info.surname}
            onChange={(e) => handleChange("surname", e.target.value)}
            id="surname"
            name="surname"
            type="text"
          />
        </div>
        <div>
          <label htmlFor="phone">Телефон:</label>
          <Input
            style={{ fontSize: 20 }}
            disabled={!editing}
            value={info.phone}
            onChange={(e) => handleChange("phone", e.target.value)}
            id="phone"
            name="phone"
            type="text"
          />
        </div>
        <div>
          <label htmlFor="email">Почта:</label>
          <Input
            style={{ fontSize: 20 }}
            disabled={!editing}
            value={info.email}
            onChange={(e) => handleChange("email", e.target.value)}
            id="email"
            name="email"
            type="text"
          />
        </div>
      </Flex>

      {editing && (
        <Flex style={{ marginTop: 30 }} gap={30} vertical>
          {errors.map((error, index) => (
            <ErrorText key={index}>{error}</ErrorText>
          ))}

          <Flex gap={10} justify="end">
            <Button disabled={loading} onClick={handleCancel}>
              Отмена
            </Button>
            <Button
              loading={loading}
              disabled={Object.keys(info).every(
                (key) =>
                  user[key as keyof typeof user] ===
                  info[key as keyof typeof info],
              )}
              onClick={handleSave}
            >
              Сохранить
            </Button>
          </Flex>
        </Flex>
      )}

      {!resettingPassword && (
        <Flex style={{ marginTop: 50 }} justify="center">
          <Button onClick={() => setResettingPassword(true)}>
            Сбросить пароль
          </Button>
        </Flex>
      )}

      {resettingPassword && (
        <Flex style={{ fontSize: 24, marginTop: 50 }} gap={10} vertical>
          <label>Введите новый пароль:</label>
          <Input
            type="password"
            style={{ fontSize: 20 }}
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
          />
          <label>Подтвердите новый пароль:</label>
          <Input
            type="password"
            style={{ fontSize: 20 }}
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
          />
        </Flex>
      )}

      {resettingPassword && (
        <Flex style={{ marginTop: 30 }} gap={30} vertical>
          {passwordErrors.map((error, index) => (
            <ErrorText key={index}>{error}</ErrorText>
          ))}

          <Flex gap={10} justify="end">
            <Button disabled={passwordLoading} onClick={handlePasswordCancel}>
              Отмена
            </Button>
            <Button
              loading={passwordLoading}
              disabled={!(newPassword && newPassword === confirmPassword)}
              onClick={handlePasswordSave}
            >
              Сохранить
            </Button>
          </Flex>
        </Flex>
      )}
    </Container>
  );
};
