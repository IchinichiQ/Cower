import { Seat } from "@/types/Coworking";
import { FC, useState } from "react";
import axios from "axios";
import { baseUrl } from "@/api";
import { Flex, Input, Modal, Upload } from "antd";
import { UploadRequestOption } from "rc-upload/es/interface";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUpload } from "@fortawesome/free-solid-svg-icons";
import { colors } from "@/styles/constants";

interface Props {
  open: boolean;

  onSubmit(seat: Seat): void;

  close(): void;
}

export const CreateSeatModal: FC<Props> = ({ close, onSubmit, open }) => {
  const [number, setNumber] = useState(1);
  const [description, setDescription] = useState("");
  const [price, setPrice] = useState(50);
  const [uploadedFile, setUploadedFile] = useState<File | undefined>();

  const handleUpload = (options: UploadRequestOption<number[]>) => {
    const { file } = options;
    setUploadedFile(file as File);
  };

  const handleSubmit = () => {
    const formData = new FormData();
    formData.append("image", uploadedFile!);
    formData.append("type", "seat");
    axios.post(`${baseUrl}/v1/images`, formData).then((res) => {
      onSubmit({
        coworkingId: -1,
        id: -1,
        description,
        price,
        number,
        position: { angle: 0, height: 75, width: 50, x: 200, y: 200 },
        image: res.data,
      });
      close();
    });
  };

  return (
    <Modal
      title="Добавить рабочее место"
      open={open}
      onOk={handleSubmit}
      onCancel={close}
      okText="Продолжить"
      cancelText="Отмена"
      okType="default"
      cancelButtonProps={{
        style: { background: "transparent", color: colors.dark },
      }}
    >
      <div>
        <span>Номер места:</span>
        <Input
          type="number"
          min={1}
          value={String(number)}
          onChange={(e) => setNumber(+e.target.value)}
        />
      </div>

      <div>
        <span>Описание места:</span>
        <Input
          value={description}
          onChange={(e) => setDescription(e.target.value)}
        />
      </div>

      <div>
        <span>Стоимость места (руб/ч):</span>
        <Input
          type="number"
          min={1}
          value={String(price)}
          onChange={(e) => setPrice(+e.target.value)}
        />
      </div>

      <Upload
        multiple={false}
        customRequest={handleUpload}
        showUploadList={false}
      >
        <Flex
          align="center"
          style={{ paddingBlock: 15, cursor: "pointer" }}
          gap={15}
        >
          <FontAwesomeIcon icon={faUpload} />
          <span>Загрузить файл</span>
        </Flex>
      </Upload>

      {uploadedFile && (
        <div>
          <img
            style={{ maxWidth: "100%" }}
            src={URL.createObjectURL(uploadedFile)}
            alt=""
          />
        </div>
      )}
    </Modal>
  );
};
