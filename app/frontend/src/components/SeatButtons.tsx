import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faPenToSquare,
  faCheck,
  faTrash,
} from "@fortawesome/free-solid-svg-icons";
import { Flex } from "antd";
import { FC } from "react";

interface Props {
  onDelete(): void;

  onEdit(): void;

  onCreate(): void;
}

export const SeatButtons: FC<Props> = ({ onCreate, onDelete, onEdit }) => {
  return (
    <Flex style={{ marginTop: 8 }} gap={10}>
      <FontAwesomeIcon
        onClick={() => onEdit()}
        style={{ cursor: "pointer" }}
        icon={faPenToSquare}
      />
      <FontAwesomeIcon
        onClick={() => onDelete()}
        style={{ cursor: "pointer" }}
        icon={faTrash}
      />
      <FontAwesomeIcon
        onClick={() => onCreate()}
        icon={faCheck}
        style={{ marginLeft: "auto", cursor: "pointer" }}
      />
    </Flex>
  );
};
