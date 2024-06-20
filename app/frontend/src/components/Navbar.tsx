import { Flex, Popover } from "antd";
import { NavLink, useNavigate } from "react-router-dom";
import styled from "styled-components";
import { useAppSelector } from "@/redux";
import { colors } from "@/styles/constants";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faArrowRightFromBracket,
  faUser,
} from "@fortawesome/free-solid-svg-icons";
import { useActions } from "@/redux/actions";
import { useState } from "react";

const Link = styled(NavLink)`
  font-size: 1.2rem;
  font-weight: 700;
  padding: 10px 15px;
  color: black;

  &.active {
    text-decoration: underline;
  }
`;

const StyledNavbar = styled(Flex)`
  padding: 5px 30px;
  background-color: ${colors.darker};
  margin-bottom: 30px;
`;

const PopoverOption = styled("div")`
  font-size: 20px;
  font-weight: 600;
  cursor: pointer;
  transition: 0.3s;

  &:hover {
    color: ${colors.tableBorder};
  }
`;

export const Navbar = () => {
  const { user } = useAppSelector((state) => state.user);
  const navigate = useNavigate();

  const { clearUser } = useActions();
  const handleLogout = () => {
    clearUser();
    setPopoverOpen(false);
  };

  const [popoverOpen, setPopoverOpen] = useState(false);

  return (
    <StyledNavbar align="center" justify="space-between">
      <Flex
        onClick={() => navigate("/home")}
        style={{ cursor: "pointer" }}
        gap={20}
        align="center"
      >
        <img src="logo.svg" alt="cower" />
        <h1 style={{ fontFamily: "Saira Condensed", fontWeight: 700 }}>
          COWËR
        </h1>
      </Flex>
      <Flex align="center">
        {user ? (
          <Popover
            open={popoverOpen}
            onOpenChange={setPopoverOpen}
            content={
              <Flex vertical gap={5}>
                <PopoverOption
                  onClick={() => {
                    navigate("profile");
                    setPopoverOpen(false);
                  }}
                >
                  Личный кабинет
                </PopoverOption>
                <PopoverOption
                  onClick={() => {
                    navigate("bookings");
                    setPopoverOpen(false);
                  }}
                >
                  Мои заказы
                </PopoverOption>
                <PopoverOption onClick={handleLogout}>
                  <Flex gap={12} align="center">
                    <FontAwesomeIcon icon={faArrowRightFromBracket} />
                    <span>Выйти</span>
                  </Flex>
                </PopoverOption>
              </Flex>
            }
            trigger={["click"]}
          >
            <FontAwesomeIcon
              icon={faUser}
              style={{ fontSize: 30, cursor: "pointer" }}
            />
          </Popover>
        ) : (
          <>
            <Link to="/sign-in">Вход</Link>
            <Link to="/sign-up">Регистрация</Link>
          </>
        )}
      </Flex>
    </StyledNavbar>
  );
};
