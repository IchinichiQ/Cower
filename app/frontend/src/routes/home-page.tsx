import styled from "styled-components";
import { colors } from "@/styles/constants";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCalendarDays, faCaretDown } from "@fortawesome/free-solid-svg-icons";
import { ConfigProvider, DatePicker, Dropdown, Flex } from "antd";
import { useEffect, useState } from "react";
import { PlaceInfoModal } from "@/components/modals/PlaceInfoModal";
import dayjs from "dayjs";
import "dayjs/locale/ru.js";
import updateLocale from "dayjs/plugin/updateLocale";
import { useActions } from "@/redux/actions";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { Coworking, Floor, Seat } from "@/types/Coworking";
import { StyledSelect } from "@/components/ui/StyledSelect";
import { ErrorText } from "@/styles/styles";
import { baseUrl } from "@/api";
import locale from "antd/es/locale/ru_RU";
import { MapLoader } from "@/components/ui/MapLoader";
import { UserRole } from "@/types/User";
import { useAppSelector } from "@/redux";

dayjs.extend(updateLocale);
dayjs.updateLocale("ru-ru", {
  weekStart: 0,
});

const StyledPage = styled("div")`
  flex-grow: 1;
  margin-top: -30px;
  position: relative;
`;

const Wrapper = styled("div")`
  display: flex;
  height: 100%;
`;

const SidePanel = styled(Flex)`
  flex: 0 0 400px;
  border-right: 1px solid ${colors.grid};
  padding: 20px;
  display: flex !important;
`;

const Map = styled("div")`
  position: relative;
  flex-grow: 1;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;

  -webkit-user-select: none;
  -khtml-user-select: none;
  -moz-user-select: none;
  -o-user-select: none;
  user-select: none;
`;

const MapWrapper = styled("div")`
  position: relative;
`;

const SeatComponent = styled("div")<{ $active: boolean }>`
  cursor: pointer;
  position: absolute;
  opacity: ${(p) => (p.$active ? 1 : 0.5)};
  pointer-events: ${(p) => (p.$active ? "all" : "none")};
  background-repeat: no-repeat;
  background-size: 100% 100%;
  background-position: center;
`;

export const HomePage = () => {
  const [timeFrom, setTimeFrom] = useState("");
  const [timeTo, setTimeTo] = useState("");
  const [date, setDate] = useState("");
  const [loading, setLoading] = useState(true);

  const { user } = useAppSelector((state) => state.user);
  const isAdmin = user?.role === UserRole.ADMIN;

  const checkTimeValid = () => {
    if (!timeFrom || !timeTo) {
      return true;
    }
    const spFrom = timeFrom.split(":");
    const spTo = timeTo.split(":");

    const openHour = +spFrom[0];
    const openMinute = +spFrom[1];
    const closeHour = +spTo[0];
    const closeMinute = +spTo[1];

    return openHour * 60 + openMinute < closeHour * 60 + closeMinute;
  };

  const { setOrder } = useActions();
  const navigate = useNavigate();
  const handleCreateOrder = (seat: Seat) => {
    setOrder({
      seatId: seat.id,
      place: seat.number,
      timeFrom,
      timeTo,
      address: coworking!.address,
      date,
      price: seat.price,
    });
    navigate("/checkout");
  };

  const [coworkingList, setCoworkingList] = useState<Coworking[]>([]);
  const [coworkingId, setCoworkingId] = useState<number | undefined>();

  useEffect(() => {
    axios.get(`${baseUrl}/v1/coworkings`).then((response) => {
      if ("data" in response) {
        setCoworkingList(response.data.coworkings);
        handleCoworkingChange(response.data.coworkings[0].id);
      }
    });
  }, []);

  const handleCoworkingChange = (coworkingId: number) => {
    setCoworkingId(coworkingId);
    setLoading(true);
    axios
      .get(`${baseUrl}/v1/coworkings/${coworkingId}`)
      .then((response) => {
        if ("data" in response) {
          const coworking = response.data;
          coworking.floors.sort((a: Floor, b: Floor) => a.number - b.number);
          setCoworking(coworking);
          handleFloorChange(coworking.floors[0].id, coworking.floors);
        }
      })
      .finally(() => setLoading(false));
  };

  const [coworking, setCoworking] = useState<Coworking>();
  const [floor, setFloor] = useState<Floor>();
  const [selectedFloor, setSelectedFloor] = useState(-1);
  const [seatsAvailability, setSeatsAvailability] = useState<
    Record<string, { from: string; to: string }[]>
  >({});

  const handleFloorChange = (id: number, floors?: Floor[]) => {
    setSelectedFloor(id);
    setFloor(
      (floors || coworking?.floors || []).find((floor) => floor.id === id)
    );
  };

  const handleDateChange = (dateStr: string) => {
    setDate(dateStr);
    if (!dateStr) {
      return;
    }
    setLoading(true);
    axios
      .get(
        `${baseUrl}/v1/floors/${floor?.id}/availability?date=${dateStr}&${floor!.seats!.map((seat) => `seatIds=${seat.id}`).join("&")}`
      )
      .then((response) => {
        if ("data" in response) {
          setSeatsAvailability(response.data.availability);
          const weekday = new Date(dateStr).toLocaleDateString("en", {
            weekday: "long",
          });
          const dayWorkingTime = coworking?.workingTime.find(
            (t) => t.day === weekday
          );
          if (dayWorkingTime) {
            setTimeFrom(dayWorkingTime.open);
            setTimeTo(dayWorkingTime.close);
          }
        }
      })
      .finally(() => setLoading(false));
  };

  const checkSeatAvailable = (seat: Seat) => {
    if (!seatsAvailability[String(seat.id)]) {
      return false;
    }
    for (const { from, to } of seatsAvailability[String(seat.id)]) {
      const spFrom = from.split(":");
      const spTo = to.split(":");
      const hourFrom = +spFrom[0];
      const minuteFrom = +spFrom[1];
      const hourTo = +spTo[0];
      const minuteTo = +spTo[1];
      const tFrom = hourFrom * 60 + minuteFrom;
      const tTo = hourTo * 60 + minuteTo;

      const spSelectedFrom = timeFrom.split(":");
      const spSelectedTo = timeTo.split(":");
      const selectedHourFrom = +spSelectedFrom[0];
      const selectedMinuteFrom = +spSelectedFrom[1];
      const selectedHourTo = +spSelectedTo[0];
      const selectedMinuteTo = +spSelectedTo[1];
      const selectedTimeFrom = selectedHourFrom * 60 + selectedMinuteFrom;
      const selectedTimeTo = selectedHourTo * 60 + selectedMinuteTo;

      if (selectedTimeFrom <= tFrom && tTo <= selectedTimeTo) {
        return true;
      }
    }
    return false;
  };

  const createTimeOptions = (onClick: (time: string) => void) => {
    const weekday = new Date(date).toLocaleDateString("en", {
      weekday: "long",
    });
    const dayWorkingTime = coworking?.workingTime.find(
      (t) => t.day === weekday
    );
    if (!dayWorkingTime) {
      return [];
    }

    const openHour = +dayWorkingTime.open.split(":")[0];
    const openMinute = +dayWorkingTime.open.split(":")[1];
    const closeHour = +dayWorkingTime.close.split(":")[0];
    const closeMinute = +dayWorkingTime.close.split(":")[1];

    const timeOptions = [];
    for (let i = openHour; i <= closeHour; i++) {
      for (let j = 0; j < 60; j += 10) {
        if (i === openHour && j < openMinute) {
          continue;
        }
        if (i === closeHour && j > closeMinute) {
          continue;
        }
        const t = `${String(i).padStart(2, "0")}:${String(j).padStart(2, "0")}`;
        timeOptions.push({
          label: <div onClick={() => onClick(t)}>{t}</div>,
          key: t,
        });
      }
    }
    return timeOptions;
  };

  return (
    <StyledPage>
      <Wrapper>
        <SidePanel vertical gap={14}>
          {coworkingList.length > 0 && (
            <StyledSelect
              style={{ fontSize: 20 }}
              suffixIcon={<FontAwesomeIcon fill={"blue"} icon={faCaretDown} />}
              value={coworkingId}
              options={coworkingList.map((coworking) => ({
                label: coworking.address,
                value: coworking.id,
              }))}
              onChange={handleCoworkingChange}
            />
          )}

          {coworking?.floors && (
            <StyledSelect
              suffixIcon={<FontAwesomeIcon fill={"blue"} icon={faCaretDown} />}
              value={selectedFloor}
              options={coworking.floors.map((floor) => ({
                label: `${floor.number} этаж`,
                value: floor.id,
              }))}
              onChange={(v: number) => handleFloorChange(v)}
            />
          )}

          {floor && (
            <>
              <Flex style={{ fontSize: 16 }} gap={10} align="center">
                <ConfigProvider locale={locale}>
                  <DatePicker
                    disabledDate={(date) => {
                      const d = date.toDate();
                      const weekday = d.toLocaleDateString("en", {
                        weekday: "long",
                      });
                      const today = new Date();
                      today.setHours(0, 0, 0, 0);

                      const diffTime = Math.abs(d.getTime() - today.getTime());
                      const diffDays = Math.floor(
                        diffTime / (1000 * 60 * 60 * 24)
                      );

                      return (
                        !coworking?.workingTime.find(
                          (t) => t.day === weekday
                        ) ||
                        d < today ||
                        diffDays > 31
                      );
                    }}
                    suffixIcon={
                      <FontAwesomeIcon fill={"blue"} icon={faCalendarDays} />
                    }
                    bordered={false}
                    style={{
                      padding: 0,
                      fontFamily: "inherit",
                      fontWeight: 700,
                    }}
                    onChange={(_, dateStr) =>
                      handleDateChange(dateStr.toString())
                    }
                  />
                </ConfigProvider>
              </Flex>

              {date && (
                <div style={{ fontSize: 14 }}>
                  <Flex justify="space-between">
                    <div>начало:</div>
                    <div>
                      <Dropdown
                        overlayClassName={"time-dropdown"}
                        trigger={["click"]}
                        menu={{
                          selectable: true,
                          items: createTimeOptions(setTimeFrom),
                        }}
                      >
                        <Flex style={{ cursor: "pointer" }} align="center">
                          <span style={{ marginRight: 10 }}>
                            {timeFrom || "-"}
                          </span>
                          <FontAwesomeIcon
                            style={{ fontSize: 12 }}
                            icon={faCaretDown}
                          />
                        </Flex>
                      </Dropdown>
                    </div>
                  </Flex>
                  <Flex justify="space-between">
                    <div>конец:</div>
                    <div>
                      <Dropdown
                        overlayClassName={"time-dropdown"}
                        trigger={["click"]}
                        menu={{
                          selectable: true,
                          items: createTimeOptions(setTimeTo),
                        }}
                      >
                        <Flex style={{ cursor: "pointer" }} align="center">
                          <span style={{ marginRight: 10 }}>
                            {timeTo || "-"}
                          </span>
                          <FontAwesomeIcon
                            style={{ fontSize: 12 }}
                            icon={faCaretDown}
                          />
                        </Flex>
                      </Dropdown>
                    </div>
                  </Flex>

                  {!checkTimeValid() && (
                    <ErrorText style={{ marginTop: 10 }}>
                      Указано некорректное время
                    </ErrorText>
                  )}
                </div>
              )}
            </>
          )}

          {isAdmin && (
            <span
              onClick={() => navigate("/edit")}
              style={{
                fontSize: 14,
                fontWeight: 700,
                textDecoration: "underline",
                cursor: "pointer",
                marginTop: 30,
              }}
            >
              Изменить данные
            </span>
          )}
        </SidePanel>

        <Map>
          {loading && <MapLoader dark={false} />}
          <MapWrapper style={{ pointerEvents: date ? "all" : "none" }}>
            {floor?.seats &&
              floor.seats.map((seat) => (
                <PlaceInfoModal
                  key={seat.id}
                  onSubmit={() => handleCreateOrder(seat)}
                  info={{
                    timeFrom,
                    timeTo,
                    date,
                    place: seat.number,
                    price: seat.price,
                    description: seat.description,
                  }}
                >
                  <div
                    style={{
                      display: "grid",
                      placeContent: "center",
                      position: "absolute",
                      width: 100,
                      height: 100,
                      top: seat.position.y - 50,
                      left: seat.position.x - 50,
                      zIndex: 1,
                    }}
                  >
                    <SeatComponent
                      $active={
                        checkTimeValid() &&
                        !!timeFrom &&
                        !!timeTo &&
                        checkSeatAvailable(seat)
                      }
                      style={{
                        width: seat.position.width,
                        height: seat.position.height,
                        backgroundImage: `url(${seat.image.url})`,
                        transform: `rotate(${seat.position.angle}deg)`,
                        transformOrigin: "center",
                        position: "relative",
                        zIndex: 5,
                      }}
                    />
                  </div>
                </PlaceInfoModal>
              ))}
            {floor && !loading && (
              <img
                draggable={false}
                style={{ maxWidth: 800 }}
                src={floor.image.url}
                alt="map"
              />
            )}
          </MapWrapper>
        </Map>
      </Wrapper>
    </StyledPage>
  );
};
