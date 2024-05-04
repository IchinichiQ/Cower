import {createGlobalStyle} from 'styled-components';
import {colors} from "@/styles/constants";
import {overrides} from "@/styles/overrides";

export const GlobalStyle = createGlobalStyle`
  @import url('https://fonts.googleapis.com/css2?family=Montserrat:ital,wght@0,100..900;1,100..900&display=swap');
  @import url('https://fonts.googleapis.com/css2?family=Saira+Condensed:wght@100;200;300;400;500;600;700;800;900&display=swap');

  *, *:before, *:after {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
  }

  html, body, #root {
    height: 100vh;
  }
  
  #root {
    display: flex;
    flex-direction: column;
  }
  
  body {
    font-family: 'Montserrat', sans-serif;
    font-weight: 600;
    color: ${colors.dark};
    background-color: ${colors.light};
  }

  li {
    list-style: none;
  }

  ${overrides}
`;
