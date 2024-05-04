import styled from "styled-components";
import {colors} from "@/styles/constants";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCaretDown, faCalendarDays} from '@fortawesome/free-solid-svg-icons';
import {ConfigProvider, DatePicker, Dropdown, Flex, Popover} from "antd";
import {useState} from "react";
import {PlaceInfoModal} from "@/components/modals/PlaceInfoModal";
import locale from 'antd/locale/ru_RU';
import {useActions} from "@/redux/actions";
import {useNavigate} from "react-router-dom";

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

const Table = styled('div')`
  cursor: pointer;
  position: absolute;
  width: 25px;
  height: 60px;
  //background: red;
`

const createTimeOptions = (onClick: (time: number) => void) => {
  const timeOptions = [];
  for (let i = 0; i < 24; i++) {
    timeOptions.push({
      label: <div onClick={() => onClick(i)}>{String(i).padStart(2, '0')}:00</div>,
      key: i,
    });
  }
  return timeOptions;
}
const address = "ул. Пушкина, д. 10";
export const HomePage = () => {
  const [timeFrom, setTimeFrom] = useState(11);
  const [timeTo, setTimeTo] = useState(12);
  const [date, setDate] = useState('')

  const {setOrder} = useActions();
  const navigate = useNavigate();
  const handleCreateOrder = (place: number) => {
    setOrder({
      place,
      timeFrom,
      timeTo,
      address,
      date,
      cost: 400,
      status: 'не оплачен'
    });
    navigate('/checkout');
  }

  return (
    <StyledPage>
      <Wrapper>
        <SidePanel vertical gap={14}>
          <Flex style={{fontSize: 24}} gap={10} align='center'>
            <span>1 этаж</span>
            <FontAwesomeIcon icon={faCaretDown}/>
          </Flex>

          <Flex style={{fontSize: 16}} gap={10} align='center'>
            <ConfigProvider locale={locale}>
              <DatePicker bordered={false} style={{padding: 0, fontFamily: 'inherit', fontWeight: 700}}
                          onChange={(_, dateStr) => setDate(dateStr.toString())}/>
            </ConfigProvider>
            {/*<FontAwesomeIcon icon={faCalendarDays}/>*/}
          </Flex>

          <div>
            <Flex justify='space-between' style={{maxWidth: 170}}>
              <div>
                начало:
              </div>
              <div>
                <Dropdown
                  overlayClassName={'time-dropdown'}
                  trigger={['click']}
                  menu={{selectable: true, items: createTimeOptions(setTimeFrom)}}
                >
                  <div>
                    <span style={{marginRight: 10}}>{String(timeFrom).padStart(2, '0')}:00</span>
                    <FontAwesomeIcon icon={faCaretDown}/>
                  </div>
                </Dropdown>
              </div>
            </Flex>
            <Flex justify='space-between' style={{maxWidth: 170}}>
              <div>
                конец:
              </div>
              <div>
                <Dropdown
                  overlayClassName={'time-dropdown'}
                  trigger={['click']}
                  menu={{selectable: true, items: createTimeOptions(setTimeTo)}}
                >
                  <div>
                    <span style={{marginRight: 10}}>{String(timeTo).padStart(2, '0')}:00</span>
                    <FontAwesomeIcon icon={faCaretDown}/>
                  </div>
                </Dropdown>
              </div>
            </Flex>
          </div>
        </SidePanel>
        <Map>
          <MapWrapper style={{pointerEvents: date ? 'all' : 'none'}}>
            <PlaceInfoModal onSubmit={() => handleCreateOrder(1)} info={{timeFrom, timeTo, date, place: 1}}>
              <Table style={{top: 50, left: 58}}/>
            </PlaceInfoModal>
            <PlaceInfoModal onSubmit={() => handleCreateOrder(2)} info={{timeFrom, timeTo, date, place: 2}}>
              <Table style={{top: 50, left: 95}}/>
            </PlaceInfoModal>
            <img src="mock-map.png" alt="map"/>
          </MapWrapper>
        </Map>
      </Wrapper>
    </StyledPage>
  );
};
