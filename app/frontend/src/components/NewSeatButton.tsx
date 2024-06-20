import { Flex } from "antd";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";

export const NewSeatButton = (props: any) => {
  return (
    <Flex
      {...props}
      align="center"
      gap={10}
      style={{ paddingBlock: 10, cursor: "pointer" }}
    >
      <FontAwesomeIcon icon={faPlus} />
      <span>Добавить место</span>
    </Flex>
  );
};
