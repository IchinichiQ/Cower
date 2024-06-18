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
  seat: Seat;
  onSubmit(seat: Seat): void;
  close(): void;
}

export const EditSeatModal: FC<Props> = ({ close, seat, onSubmit, open }) => {
  const [number, setNumber] = useState(seat.number);
  const [description, setDescription] = useState(seat.description);
  const [price, setPrice] = useState(seat.price);
  const [uploadedFile, setUploadedFile] = useState<File | undefined>();

  const handleUpload = (options: UploadRequestOption<number[]>) => {
    const { file } = options;
    setUploadedFile(file as File);
  };

  const handleSubmit = async () => {
    let newImage;
    if (uploadedFile) {
      const formData = new FormData();
      formData.append("image", uploadedFile!);
      formData.append("type", "seat");
      const res = await axios.post(`${baseUrl}/v1/images`, formData);
      newImage = res.data;
    }
    onSubmit({
      ...seat,
      number,
      description,
      price,
      image: newImage || seat.image,
    });
    close();
  };

  return (
    <Modal
      title="Изменить рабочее место"
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

      <div>
        <img
          style={{ maxWidth: "100%" }}
          src={
            uploadedFile ? URL.createObjectURL(uploadedFile) : seat.image.url
          }
          alt=""
        />
      </div>
    </Modal>
  );
};
