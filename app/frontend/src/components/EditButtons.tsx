import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faPenToSquare,
  faPlus,
  faTrash,
} from "@fortawesome/free-solid-svg-icons";
import { Flex } from "antd";
import { FC } from "react";

interface Props {
  leftVisible: boolean;

  onDelete(): void;

  onEdit(): void;

  onCreate(): void;
}

export const EditButtons: FC<Props> = ({
  leftVisible,
  onCreate,
  onDelete,
  onEdit,
}) => {
  return (
    <Flex style={{ marginTop: 8 }} gap={10}>
      {leftVisible && (
        <>
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
        </>
      )}
      <FontAwesomeIcon
        onClick={() => onCreate()}
        icon={faPlus}
        style={{ marginLeft: "auto", cursor: "pointer" }}
      />
    </Flex>
  );
};
