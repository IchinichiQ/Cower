import styled, { keyframes } from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleNotch } from "@fortawesome/free-solid-svg-icons";

const kf = keyframes`
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
`;

const MapLoaderWrapper = styled("div")`
  position: absolute;
  top: 0;
  left: 0;
  background-color: rgba(193, 193, 193, 0.7);
  width: 100%;
  height: 100%;
  z-index: 10;
  display: flex;
  justify-content: center;
  align-items: center;

  & > * {
    animation: ${kf} 1s linear infinite;
  }
`;

export const MapLoader = () => (
  <MapLoaderWrapper>
    <FontAwesomeIcon fontSize={50} icon={faCircleNotch} />
  </MapLoaderWrapper>
);
