import styled from "styled-components";
import {colors} from "@/styles/constants";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCaretDown, faCalendarDays} from '@fortawesome/free-solid-svg-icons';
import {ConfigProvider, DatePicker, Dropdown, Flex, Popover, Select} from "antd";
import {useEffect, useState} from "react";
import {PlaceInfoModal} from "@/components/modals/PlaceInfoModal";
import locale from 'antd/locale/ru_RU';
import {useActions} from "@/redux/actions";
import {useNavigate} from "react-router-dom";
import axios from "axios";
import {Coworking, Floor, Seat} from "@/types/Coworking";
import {StyledSelect} from "@/components/ui/StyledSelect";
import {ErrorText} from "@/styles/styles";
import {baseUrl} from "@/api";

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
    return timeFrom < timeTo;
  }

  const {setOrder} = useActions();
  const navigate = useNavigate();
  const handleCreateOrder = (seat: Seat) => {
    setOrder({
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
          setCoworking(response.data);
          handleFloorChange(1);
        }
      });
  }

  const [coworking, setCoworking] = useState<Coworking>();
  const [floor, setFloor] = useState<Floor>();
  const [selectedFloor, setSelectedFloor] = useState(-1)
  const [seatsAvailability, setSeatsAvailability] = useState<Record<string, { from: string; to: string }[]>>({})

  const handleFloorChange = (newFloor: number) => {
    setSelectedFloor(newFloor);
    axios.get(`${baseUrl}/coworking/1/floor/${newFloor}`)
      .then(response => {
        if ('data' in response) {
          setFloor(response.data);
          return response.data;
        }
      });
  }

  const handleDateChange = (dateStr: string) => {
    setDate(dateStr);
    axios.get(`${baseUrl}/coworking/${coworkingId}/seat/availability?date=${dateStr}&${floor!.seats.map(seat => `seatIds=${seat.id}`).join('&')}`)
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

          {coworking &&
            <StyledSelect
              suffixIcon={<FontAwesomeIcon fill={'blue'} icon={faCaretDown}/>}
              value={selectedFloor}
              options={Array(coworking.floors).fill(null).map((_, idx) => ({
                label: `${idx + 1} этаж`,
                value: idx + 1
              }))}
              onChange={handleFloorChange}
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
                      return !coworking?.workingTime.find(t => t.day === weekday);
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

                {!checkTimeValid() && <ErrorText>Указало некорректное время</ErrorText>}
              </div>}
            </>
          }
        </SidePanel>

        {floor &&
          <Map>
            <MapWrapper style={{pointerEvents: date ? 'all' : 'none'}}>
              {floor.seats.map(seat =>
                <PlaceInfoModal
                  key={seat.id}
                  onSubmit={() => handleCreateOrder(seat)}
                  info={{timeFrom, timeTo, date, place: seat.number, description: seat.description}}
                >
                  <SeatComponent
                    $active={checkTimeValid() && checkSeatAvailable(seat)}
                    style={{
                      top: seat.position.y,
                      left: seat.position.x,
                      width: seat.position.width,
                      height: seat.position.height,
                      backgroundImage: `url(${seat.image})`,
                      transform: `rotate(${seat.position.angle})`,
                    }}/>
                </PlaceInfoModal>
              )}
              <img src={floor.backgroundImage} alt="map"/>
            </MapWrapper>
          </Map>
        }
      </Wrapper>
    </StyledPage>
  );
};
