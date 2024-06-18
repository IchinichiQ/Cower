import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faPenToSquare,
  faCheck,
  faTrash,
  faXmark,
} from "@fortawesome/free-solid-svg-icons";
import { Flex } from "antd";
import { FC } from "react";

interface Props {
  onEdit(): void;
  onCreate(): void;
  onCancel(): void;
  onDelete?(): void;
}

export const SeatButtons: FC<Props> = ({
  onCreate,
  onCancel,
  onDelete,
  onEdit,
}) => {
  return (
    <Flex justify="space-between" style={{ marginTop: 8 }} gap={10}>
      <Flex gap={10}>
        <FontAwesomeIcon
          onClick={() => onEdit()}
          style={{ cursor: "pointer" }}
          icon={faPenToSquare}
        />
        {onDelete && (
          <FontAwesomeIcon
            onClick={() => onDelete()}
            style={{ cursor: "pointer" }}
            icon={faTrash}
          />
        )}
      </Flex>
      <Flex gap={10}>
        <FontAwesomeIcon
          onClick={() => onCancel()}
          icon={faXmark}
          style={{ cursor: "pointer" }}
        />

        <FontAwesomeIcon
          onClick={() => onCreate()}
          icon={faCheck}
          style={{ cursor: "pointer" }}
        />
      </Flex>
    </Flex>
  );
};
