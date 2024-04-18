import {Container} from "@/styles/Container";
import {ToHomeButton} from "@/components/ToHomeButton";
import {useAuthorizedUser} from "@/redux/userSlice";
import {Flex, Input} from "antd";

export const ProfilePage = () => {
  const user = useAuthorizedUser();

  return (
    <Container>
      <ToHomeButton/>

      <h1 style={{marginBlock: 20}}>Личный кабинет</h1>

      <Flex style={{fontSize: 24}} vertical gap={12}>
        <div>
          <label htmlFor="name">Имя:</label>
          <Input
            style={{fontSize: 20}}
            disabled
            value={user.name}
            id="name"
            name="name"
            type="text"
          />
        </div>
        <div>
          <label htmlFor="surname">Фамилия:</label>
          <Input
            style={{fontSize: 20}}
            disabled
            value={user.surname}
            id="surname"
            name="surname"
            type="text"
          />
        </div>
        <div>
          <label htmlFor="phone">Телефон:</label>
          <Input
            style={{fontSize: 20}}
            disabled
            value={user.phone}
            id="phone"
            name="phone"
            type="text"
          />
        </div>
        <div>
          <label htmlFor="email">Почта:</label>
          <Input
            style={{fontSize: 20}}
            disabled
            value={user.email}
            id="email"
            name="email"
            type="text"
          />
        </div>
      </Flex>
    </Container>
  );
};
