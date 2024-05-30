import { Select } from "antd";

export const StyledSelect = (props: any) => {
  return <Select className="styled-select" bordered={false} {...props} />;
};
