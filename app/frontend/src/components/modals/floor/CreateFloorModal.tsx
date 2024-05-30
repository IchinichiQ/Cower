import { Flex, Input, Modal, Upload } from "antd";
import { FC, useState } from "react";
import axios from "axios";
import { baseUrl } from "@/api";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUpload } from "@fortawesome/free-solid-svg-icons";
import { UploadRequestOption } from "rc-upload/es/interface";
import {colors} from "@/styles/constants";

interface Props {
  open: boolean;
  coworkingId: number;

  onSubmit(): void;

  close(): void;
}

export const CreateFloorModal: FC<Props> = ({
  coworkingId,
  close,
  onSubmit,
  open,
}) => {
  const [floorNumber, setFloorNumber] = useState(1);
  const [loading, setLoading] = useState(false);

  const handleSubmit = () => {
    setLoading(true);
    const formData = new FormData();
    formData.append("image", uploadedFile!);
    formData.append("type", "floor");
    axios.post(`${baseUrl}/v1/images`, formData).then((res) => {
      axios
        .post(`${baseUrl}/v1/floors`, {
          number: floorNumber,
          coworkingId,
          imageId: res.data.id,
        })
        .then(() => {
          setLoading(false);
          onSubmit();
          close();
        });
    });
  };

  const [uploadedFile, setUploadedFile] = useState<File | undefined>();
  const handleUpload = (options: UploadRequestOption<number[]>) => {
    const { file } = options;
    setUploadedFile(file as File);
  };

  return (
    <Modal
      title="Добавить этаж"
      open={open}
      onOk={handleSubmit}
      onCancel={close}
      okButtonProps={{ loading }}
      okText="Создать"
      okType="default"
      cancelText="Отмена"
      cancelButtonProps={{
        style: { background: "transparent", color: colors.dark },
      }}
    >
      <Input
        type="number"
        min={1}
        value={String(floorNumber)}
        onChange={(e) => setFloorNumber(+e.target.value)}
      />

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
