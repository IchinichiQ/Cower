import { Flex, Input, Modal, Upload } from "antd";
import { FC, useState } from "react";
import axios from "axios";
import { baseUrl } from "@/api";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUpload } from "@fortawesome/free-solid-svg-icons";
import { UploadRequestOption } from "rc-upload/es/interface";
import { colors } from "@/styles/constants";
import { Floor } from "@/types/Coworking";

interface Props {
  open: boolean;
  floor: Floor;

  onSubmit(): void;

  close(): void;
}

export const EditFloorModal: FC<Props> = ({ floor, close, onSubmit, open }) => {
  const [floorNumber, setFloorNumber] = useState(floor.number);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async () => {
    setLoading(true);
    let newImageId;
    if (uploadedFile) {
      const formData = new FormData();
      formData.append("image", uploadedFile!);
      formData.append("type", "floor");
      const res = await axios.post(`${baseUrl}/v1/images`, formData);
      newImageId = res.data.id;
    }
    await axios
      .patch(`${baseUrl}/v1/floors/${floor.id}`, {
        number: floorNumber,
        coworkingId: floor.coworkingId,
        imageId: newImageId ?? floor.image.id,
      })
      .then(() => {
        setLoading(false);
        onSubmit();
        close();
      });
  };

  const [uploadedFile, setUploadedFile] = useState<File | undefined>();
  const handleUpload = (options: UploadRequestOption<number[]>) => {
    const { file } = options;
    setUploadedFile(file as File);
  };

  return (
    <Modal
      title="Редактировать этаж"
      open={open}
      onOk={handleSubmit}
      onCancel={close}
      okButtonProps={{ loading }}
      okText="Сохранить"
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

      <div>
        <img
          style={{ maxWidth: "100%" }}
          src={
            uploadedFile ? URL.createObjectURL(uploadedFile) : floor.image.url
          }
          alt=""
        />
      </div>
    </Modal>
  );
};
