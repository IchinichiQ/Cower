import { css } from "styled-components";
import { colors } from "@/styles/constants";

export const overrides = css`
  .ant-checkbox-checked .ant-checkbox-inner {
    background-color: ${colors.dark};
    border-color: ${colors.dark};
  }

  .ant-popover-inner-content {
    white-space: nowrap;
  }

  .time-dropdown > ul {
    max-height: 200px;
    overflow-y: auto;
    scrollbar-width: none;
  }

  .ant-select.styled-select {
    width: 100%;

    & > .ant-select-selector {
      padding-inline: 0 !important;

      .ant-select-selection-item {
        font-weight: 600 !important;
        font-size: 18px;
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
