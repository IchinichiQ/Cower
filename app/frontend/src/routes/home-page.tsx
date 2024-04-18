import styled from "styled-components";
import {colors} from "@/styles/constants";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faCaretDown, faCalendarDays} from '@fortawesome/free-solid-svg-icons';
import {Flex} from "antd";

const StyledPage = styled('div')`
  flex-grow: 1;
  margin-top: -30px;
  position: relative;
`

const SidePanel = styled(Flex)`
  width: 300px;
  height: 100%;
  border-right: 1px solid ${colors.grid};
  padding: 20px;
`

export const HomePage = () => {
  return (
    <StyledPage>
      <SidePanel vertical gap={14}>
        <Flex style={{fontSize: 24}} gap={10} align='center'>
          <span>1 этаж</span>
          <FontAwesomeIcon icon={faCaretDown}/>
        </Flex>

        <Flex style={{fontSize: 16}} gap={10} align='center'>
          <span>13.03.2024</span>
          <FontAwesomeIcon icon={faCalendarDays}/>
        </Flex>

        <div>
          <Flex justify='space-between' style={{maxWidth: 170}}>
            <div>
              начало:
            </div>
            <div>
              <span style={{marginRight: 10}}>15:00</span>
              <FontAwesomeIcon icon={faCaretDown} />
            </div>
          </Flex>
          <Flex justify='space-between' style={{maxWidth: 170}}>
            <div>
              конец:
            </div>
            <div>
              <span style={{marginRight: 10}}>19:00</span>
              <FontAwesomeIcon icon={faCaretDown} />
            </div>
          </Flex>
        </div>
      </SidePanel>
      <img style={{position: 'absolute', top: 100, left: 450}} src="mock-map.png" alt="map"/>
    </StyledPage>
  );
};
