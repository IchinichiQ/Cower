import {css} from "styled-components";

export const overrides = css`
  .ant-popover-inner-content {
    white-space: nowrap;
  }

  .time-dropdown > ul {
    max-height: 200px;
    overflow-y: auto;
    scrollbar-width: none;
  }

  .ant-select.styled-select {

    & > .ant-select-selector {
      padding-inline: 0 !important;

      .ant-select-selection-item {
        font-weight: 600 !important;
      }
    }

    .ant-select-arrow {
      color: unset !important;
      right: 0;
    }
  }

  .ant-picker {
    flex-grow: 1;

    .ant-picker-input input {
      font-weight: 600;
    }

    .ant-picker-suffix {
      color: unset !important;
    }
  }
`;
