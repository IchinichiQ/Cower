import styled from "styled-components";
import {colors} from "@/styles/constants";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCaretDown, faCalendarDays} from '@fortawesome/free-solid-svg-icons';
import {ConfigProvider, DatePicker, Dropdown, Flex, Popover, Select} from "antd";
import {useEffect, useState} from "react";
import {PlaceInfoModal} from "@/components/modals/PlaceInfoModal";
import dayjs from 'dayjs'
import 'dayjs/locale/ru.js';
import updateLocale from 'dayjs/plugin/updateLocale';
import {useActions} from "@/redux/actions";
import {useNavigate} from "react-router-dom";
import axios from "axios";
import {Coworking, Floor, Seat} from "@/types/Coworking";
import {StyledSelect} from "@/components/ui/StyledSelect";
import {ErrorText} from "@/styles/styles";
import {baseUrl} from "@/api";
import ym from "react-yandex-metrika";
import locale from "antd/es/locale/ru_RU";

dayjs.extend(updateLocale);
dayjs.updateLocale('ru-ru', {
  weekStart: 0,
});

const StyledPage = styled('div')`
  flex-grow: 1;
  margin-top: -30px;
  position: relative;
`

const Wrapper = styled('div')`
  display: flex;
  height: 100%;
`

const SidePanel = styled(Flex)`
  width: 300px;
  border-right: 1px solid ${colors.grid};
  padding: 20px;
`

const Map = styled('div')`
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;

  -webkit-user-select: none;
  -khtml-user-select: none;
  -moz-user-select: none;
  -o-user-select: none;
  user-select: none;
`

const MapWrapper = styled('div')`
  position: relative;

`

const SeatComponent = styled('div')<{ $active: boolean }>`
  cursor: pointer;
  position: absolute;
  opacity: ${p => p.$active ? 1 : .5};
  pointer-events: ${p => p.$active ? 'all' : 'none'};
  //background: red;
`

export const HomePage = () => {
  const [timeFrom, setTimeFrom] = useState(-1);
  const [timeTo, setTimeTo] = useState(-1);
  const [date, setDate] = useState('')

  const checkTimeValid = () => {
    return timeFrom < timeTo || timeFrom === -1 || timeTo === -1;
  }

  const {setOrder} = useActions();
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
    navigate('/checkout');
  }

  const [coworkingList, setCoworkingList] = useState<Coworking[]>([]);
  const [coworkingId, setCoworkingId] = useState<number | undefined>();

  useEffect(() => {
    axios.get(`${baseUrl}/coworking`)
      .then(response => {
        if ('data' in response) {
          setCoworkingList(response.data.coworkings);
          handleCoworkingChange(response.data.coworkings[0].id);
        }
      })
  }, []);

  const handleCoworkingChange = (coworkingId: number) => {
    setCoworkingId(coworkingId);
    axios.get(`${baseUrl}/coworking/${coworkingId}`)
      .then(response => {
        if ('data' in response) {
          const coworking = response.data;
          coworking.floors.sort((a: Floor, b: Floor) => a.number - b.number);
          setCoworking(coworking);
          console.log(coworking)
          handleFloorChange(coworking.floors[0].id, coworking.floors);
        }
      });
  }

  const [coworking, setCoworking] = useState<Coworking>();
  const [floor, setFloor] = useState<Floor>();
  const [selectedFloor, setSelectedFloor] = useState(-1)
  const [seatsAvailability, setSeatsAvailability] = useState<Record<string, { from: string; to: string }[]>>({})

  const handleFloorChange = (id: number, floors?: Floor[]) => {
    setSelectedFloor(id);
    setFloor((floors || coworking?.floors || []).find(floor => floor.id === id));
  }

  const handleDateChange = (dateStr: string) => {
    setDate(dateStr);
    if (!dateStr) {
      return;
    }
    ym('reachGoal', 'register');
    axios.get(`${baseUrl}/v1/floors/${floor?.id}/availability?date=${dateStr}&${floor!.seats!.map(seat => `seatIds=${seat.id}`).join('&')}`)
      .then(response => {
        if ('data' in response) {
          setSeatsAvailability(response.data.availability);
          const weekday = new Date(dateStr).toLocaleDateString('en', {weekday: 'long'});
          const dayWorkingTime = coworking?.workingTime.find(t => t.day === weekday);
          if (dayWorkingTime) {
            setTimeFrom(+dayWorkingTime.open.split(':')[0]);
            setTimeTo(+dayWorkingTime.close.split(':')[0]);
          }
        }
      })
  }

  const checkSeatAvailable = (seat: Seat) => {
    if (!seatsAvailability[String(seat.id)]) {
      return false;
    }
    for (const period of seatsAvailability[String(seat.id)]) {
      const periodFrom = +period.from.split(':')[0];
      const periodTo = +period.to.split(':')[0];
      const from = timeFrom;
      const to = timeTo === -1 ? 24 : timeTo;
      if (periodFrom <= from && to <= periodTo) {
        return true;
      }
    }
    return false;
  }

  const createTimeOptions = (onClick: (time: number) => void) => {
    const weekday = new Date(date).toLocaleDateString('en', {weekday: 'long'});
    const dayWorkingTime = coworking?.workingTime.find(t => t.day === weekday);
    if (!dayWorkingTime) {
      return [];
    }
    const timeOptions = [];
    for (let i = 0; i < 24; i++) {
      const start = +dayWorkingTime.open.split(':')[0];
      const end = +dayWorkingTime.close.split(':')[0];
      if (start <= i && i <= end) {
        timeOptions.push({
          label: <div onClick={() => onClick(i)}>{String(i).padStart(2, '0')}:00</div>,
          key: i,
        });
      }
    }
    return timeOptions;
  }

  return (
    <StyledPage>
      <Wrapper>
        <SidePanel vertical gap={14}>
          {coworkingList.length > 0 &&
            <StyledSelect
              style={{fontSize: 20}}
              suffixIcon={<FontAwesomeIcon fill={'blue'} icon={faCaretDown}/>}
              value={coworkingId}
              options={coworkingList.map((coworking) => ({
                label: coworking.address,
                value: coworking.id
              }))}
              onChange={handleCoworkingChange}
            />
          }

          {coworking?.floors &&
            <StyledSelect
              suffixIcon={<FontAwesomeIcon fill={'blue'} icon={faCaretDown}/>}
              value={selectedFloor}
              options={coworking.floors.map((floor) => ({
                label: `${floor.number} этаж`,
                value: floor.id
              }))}
              onChange={(v: number) => handleFloorChange(v)}
            />
          }

          {floor &&
            <>
              <Flex style={{fontSize: 16}} gap={10} align='center'>
                <ConfigProvider locale={locale}>
                  <DatePicker
                    disabledDate={date => {
                      const d = date.toDate();
                      const weekday = d.toLocaleDateString('en', {weekday: 'long'});
                      const today = new Date();
                      today.setHours(0, 0, 0, 0);

                      const diffTime = Math.abs(d.getTime() - today.getTime());
                      const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24));

                      return !coworking?.workingTime.find(t => t.day === weekday) || d < today || diffDays > 31;
                    }}
                    suffixIcon={<FontAwesomeIcon fill={'blue'} icon={faCalendarDays}/>}
                    bordered={false}
                    style={{padding: 0, fontFamily: 'inherit', fontWeight: 700}}
                    onChange={(_, dateStr) => handleDateChange(dateStr.toString())}/>
                </ConfigProvider>
              </Flex>

              {date && <div style={{fontSize: 14}}>
                <Flex justify='space-between'>
                  <div>
                    начало:
                  </div>
                  <div>
                    <Dropdown
                      overlayClassName={'time-dropdown'}
                      trigger={['click']}
                      menu={{selectable: true, items: createTimeOptions(setTimeFrom)}}
                    >
                      <Flex style={{cursor: "pointer"}} align="center">
                        <span
                          style={{marginRight: 10}}>{timeFrom === -1 ? '-' : `${String(timeFrom).padStart(2, '0')}:00`}</span>
                        <FontAwesomeIcon style={{fontSize: 12}} icon={faCaretDown}/>
                      </Flex>
                    </Dropdown>
                  </div>
                </Flex>
                <Flex justify='space-between'>
                  <div>
                    конец:
                  </div>
                  <div>
                    <Dropdown
                      overlayClassName={'time-dropdown'}
                      trigger={['click']}
                      menu={{selectable: true, items: createTimeOptions(setTimeTo)}}
                    >
                      <Flex style={{cursor: "pointer"}} align="center">
                        <span
                          style={{marginRight: 10}}>{timeTo === -1 ? '-' : `${String(timeTo).padStart(2, '0')}:00`}</span>
                        <FontAwesomeIcon style={{fontSize: 12}} icon={faCaretDown}/>
                      </Flex>
                    </Dropdown>
                  </div>
                </Flex>

                {!checkTimeValid() && <ErrorText style={{marginTop: 10}}>Указано некорректное время</ErrorText>}
              </div>}
            </>
          }
        </SidePanel>

        {floor?.seats &&
          <Map>
            <MapWrapper style={{pointerEvents: date ? 'all' : 'none'}}>
              {floor.seats.map(seat =>
                <PlaceInfoModal
                  key={seat.id}
                  onSubmit={() => handleCreateOrder(seat)}
                  info={{timeFrom, timeTo, date, place: seat.number, price: seat.price, description: seat.description}}
                >
                  <SeatComponent
                    $active={checkTimeValid() && checkSeatAvailable(seat)}
                    style={{
                      top: seat.position.y,
                      left: seat.position.x,
                      width: seat.position.width,
                      height: seat.position.height,
                      backgroundImage: `url(${seat.image.url})`,
                      transform: `rotate(${seat.position.angle})`,
                    }}/>
                </PlaceInfoModal>
              )}
              <img src={floor.image.url} alt="map"/>
            </MapWrapper>
          </Map>
        }
      </Wrapper>
    </StyledPage>
  );
};
