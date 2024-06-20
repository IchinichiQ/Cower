import styled from "styled-components";
import { colors } from "@/styles/constants";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faCaretDown,
  faCircleQuestion,
} from "@fortawesome/free-solid-svg-icons";
import { Flex, Popover } from "antd";
import { MouseEvent, useEffect, useRef, useState } from "react";
import dayjs from "dayjs";
import "dayjs/locale/ru.js";
import updateLocale from "dayjs/plugin/updateLocale";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { Coworking, Floor, Seat } from "@/types/Coworking";
import { StyledSelect } from "@/components/ui/StyledSelect";
import { baseUrl } from "@/api";
import { useAuthorizedUser } from "@/redux/userSlice";
import { EditButtons } from "@/components/EditButtons";
import { CreateCoworkingModal } from "@/components/modals/coworking/CreateCoworkingModal";
import { EditCoworkingModal } from "@/components/modals/coworking/EditCoworkingModal";
import { CreateFloorModal } from "@/components/modals/floor/CreateFloorModal";
import { NewSeatButton } from "@/components/NewSeatButton";
import { CreateSeatModal } from "@/components/modals/seat/CreateSeatModal";
import { SeatButtons } from "@/components/SeatButtons";
import useModal from "antd/es/modal/useModal";
import { EditFloorModal } from "@/components/modals/floor/EditFloorModal";
import { EditSeatModal } from "@/components/modals/seat/EditSeatModal";
import { MapLoader } from "@/components/ui/MapLoader";
import { minmax } from "@/utils/minmax";

const SEAT_MIN_WIDTH = 60;
const SEAT_MAX_WIDTH = 130;

dayjs.extend(updateLocale);
dayjs.updateLocale("ru-ru", {
  weekStart: 0,
});

const StyledPage = styled("div")`
  flex-grow: 1;
  margin-top: -30px;
  position: relative;
`;

const Wrapper = styled("div")`
  display: flex;
  height: 100%;
`;

const SidePanel = styled(Flex)`
  width: 400px;
  border-right: 1px solid ${colors.grid};
  padding: 20px;
  display: flex;
  flex-direction: column;
`;

const Map = styled("div")`
  position: relative;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;

  -webkit-user-select: none;
  -khtml-user-select: none;
  -moz-user-select: none;
  -o-user-select: none;
  user-select: none;
`;

const MapWrapper = styled("div")`
  position: relative;
`;

const SeatComponent = styled("div")<{ $active: boolean }>`
  cursor: pointer;
  position: absolute;
  opacity: ${(p) => (p.$active ? 1 : 0.5)};
  pointer-events: ${(p) => (p.$active ? "all" : "none")};
  background-repeat: no-repeat;
  background-size: 100% 100%;
  background-position: center;
`;

export const EditPage = () => {
  const user = useAuthorizedUser();

  const [loading, setLoading] = useState(false);
  const [coworkingList, setCoworkingList] = useState<Coworking[]>([]);
  const [coworkingId, setCoworkingId] = useState<number | undefined>();

  const fetchCoworkings = (fetchCurrent?: boolean) => {
    setLoading(true);
    axios
      .get(`${baseUrl}/v1/coworkings`)
      .then((response) => {
        if ("data" in response) {
          setCoworkingList(response.data.coworkings);
          if (fetchCurrent) {
            fetchCoworking(coworkingId!);
          } else if (response.data.coworkings.length) {
            fetchCoworking(response.data.coworkings[0].id);
          } else {
            setCoworking(undefined);
            setSelectedFloor(-1);
            setFloor(undefined);
          }
        }
      })
      .finally(() => setLoading(false));
  };

  useEffect(() => {
    fetchCoworkings();
  }, []);

  const fetchCoworking = (
    coworkingId: number,
    preserveFloor = true,
    cb?: () => void
  ) => {
    setLoading(true);
    setCoworkingId(coworkingId);
    axios
      .get(`${baseUrl}/v1/coworkings/${coworkingId}`)
      .then((response) => {
        if ("data" in response) {
          const coworking = response.data;
          coworking.floors.sort((a: Floor, b: Floor) => a.number - b.number);
          setCoworking(coworking);
          if (coworking.floors.length) {
            handleFloorChange(
              (preserveFloor ? floor?.id : undefined) ?? coworking.floors[0].id,
              coworking.floors
            );
          } else {
            setSelectedFloor(-1);
            setFloor(undefined);
          }
        }
      })
      .finally(() => {
        setLoading(false);
        cb?.();
      });
  };

  const [coworking, setCoworking] = useState<Coworking>();
  const [floor, setFloor] = useState<Floor>();
  const [selectedFloor, setSelectedFloor] = useState(-1);

  const handleFloorChange = (id: number, floors?: Floor[]) => {
    setSelectedFloor(id);
    setFloor(
      (floors || coworking?.floors || []).find((floor) => floor.id === id)
    );
  };

  const [{ confirm }, modalContextHolder] = useModal();
  // coworking
  const [createCoworkingModalOpen, setCreateCoworkingModalOpen] =
    useState(false);
  const [editCoworkingModalOpen, setEditCoworkingModalOpen] = useState(false);

  const handleCoworkingCreate = () => {
    setCreateCoworkingModalOpen(true);
  };

  const handleCoworkingDelete = () => {
    confirm({
      title: "Подтвердите действие",
      content: `Удалить коворкинг: ${coworking?.address}`,
      okType: "default",
      okText: "Удалить",
      cancelText: "Отмена",
      onOk() {
        setLoading(true);
        axios
          .delete(`${baseUrl}/v1/coworkings/${coworkingId}`)
          .then(() => {
            fetchCoworkings();
          })
          .finally(() => setLoading(false));
      },
    });
  };

  const handleCoworkingEdit = () => {
    setEditCoworkingModalOpen(true);
  };

  // Floor
  const [createFloorModalOpen, setCreateFloorModalOpen] = useState(false);
  const [editFloorModalOpen, setEditFloorModalOpen] = useState(false);

  const handleFloorCreate = () => {
    setCreateFloorModalOpen(true);
  };

  const handleFloorDelete = () => {
    confirm({
      title: "Подтвердите действие",
      content: `Удалить этаж: ${floor?.number}`,
      okType: "default",
      okText: "Удалить",
      cancelText: "Отмена",
      onOk() {
        setLoading(true);
        axios
          .delete(`${baseUrl}/v1/floors/${floor?.id}`)
          .then(() => {
            fetchCoworking(coworkingId!);
          })
          .finally(() => setLoading(false));
      },
    });
  };

  const handleFloorEdit = () => {
    setEditFloorModalOpen(true);
  };

  // Seat
  const [seatStatus, setSeatStatus] = useState<"idle" | "new" | "edit">("idle");
  const [activeSeat, setActiveSeat] = useState<Seat | undefined>();
  const [createSeatModalOpen, setCreateSeatModalOpen] = useState(false);
  const [editSeatModalOpen, setEditSeatModalOpen] = useState(false);

  const handleCreateSeat = () => {
    setCreateSeatModalOpen(true);
  };

  const handleDeleteActiveSeat = () => {
    confirm({
      title: "Подтвердите действие",
      content: `Удалить рабочее место: ${activeSeat?.number}`,
      okType: "default",
      okText: "Удалить",
      cancelText: "Отмена",
      onOk() {
        setLoading(true);
        axios
          .delete(`${baseUrl}/v1/seats/${activeSeat?.id}`)
          .then(() => {
            setActiveSeat(undefined);
            setSeatStatus("idle");
            fetchCoworking(coworkingId!);
          })
          .finally(() => setLoading(false));
      },
    });
  };

  const handleEditActiveSeat = () => {
    setEditSeatModalOpen(true);
  };

  const handleCancelSeat = () => {
    setActiveSeat(undefined);
    setSeatStatus("idle");
  };

  const handleSaveActiveSeat = () => {
    setLoading(true);
    const { id, coworkingId: _, image, ...data } = activeSeat!;
    data.position.x = Math.round(data.position.x);
    data.position.y = Math.round(data.position.y);
    data.position.angle = Math.round(data.position.angle);
    if (seatStatus === "edit") {
      axios
        .patch(`${baseUrl}/v1/seats/${id}`, {
          ...data,
          ...data.position,
          imageId: image.id,
          floorId: floor!.id,
        })
        .then(() => {
          fetchCoworking(coworkingId!, true, () => {
            setActiveSeat(undefined);
            setSeatStatus("idle");
          });
        });
    } else {
      axios
        .post(`${baseUrl}/v1/seats`, {
          ...data,
          imageId: image.id,
          floorId: floor!.id,
        })
        .then(() => {
          fetchCoworking(coworkingId!, true, () => {
            setActiveSeat(undefined);
            setSeatStatus("idle");
          });
        });
    }
  };

  const mapRef = useRef<HTMLDivElement | null>(null);
  const rectRef = useRef<HTMLDivElement | null>(null);
  const [isDragging, setIsDragging] = useState(false);

  const handleSeatDrag = (e: MouseEvent<HTMLDivElement>) => {
    setIsDragging(true);
  };

  const handleMouseMove = (e: MouseEvent<HTMLDivElement>) => {
    if (!isDragging) {
      return;
    }
    const rect = mapRef.current!.getBoundingClientRect()!;
    const x = minmax(0, Math.round(e.clientX - Number(rect.left)), rect.width);
    const y = minmax(0, Math.round(e.clientY - Number(rect.top)), rect.height);
    setActiveSeat({
      ...activeSeat!,
      position: { ...activeSeat!.position, x, y },
    });
  };

  const handleMouseUp = (e: MouseEvent<HTMLDivElement>) => {
    setIsDragging(false);
  };

  useEffect(() => {
    const pd = (e: any) => e.preventDefault();
    if (mapRef.current) {
      mapRef.current?.addEventListener("wheel", pd);
    }
    if (rectRef.current) {
      rectRef.current?.addEventListener("wheel", pd);
    }
    return () => {
      if (mapRef.current) {
        mapRef.current?.removeEventListener("wheel", pd);
      }
      if (rectRef.current) {
        rectRef.current?.removeEventListener("wheel", pd);
      }
    };
  }, []);

  const navigate = useNavigate();
  return (
    <StyledPage>
      {/* Modals */}
      {modalContextHolder}

      {/* Coworking */}
      {createCoworkingModalOpen && (
        <CreateCoworkingModal
          open={createCoworkingModalOpen}
          close={() => setCreateCoworkingModalOpen(false)}
          onSubmit={fetchCoworkings}
        />
      )}
      {editCoworkingModalOpen && (
        <EditCoworkingModal
          coworking={coworking!}
          open={editCoworkingModalOpen}
          close={() => setEditCoworkingModalOpen(false)}
          onSubmit={() => fetchCoworkings(true)}
        />
      )}

      {/* Floor */}
      {createFloorModalOpen && (
        <CreateFloorModal
          open={createFloorModalOpen}
          coworkingId={coworkingId!}
          onSubmit={() => fetchCoworking(coworkingId!)}
          close={() => setCreateFloorModalOpen(false)}
        />
      )}
      {editFloorModalOpen && (
        <EditFloorModal
          open={editFloorModalOpen}
          floor={floor!}
          onSubmit={() => fetchCoworking(coworkingId!)}
          close={() => setEditFloorModalOpen(false)}
        />
      )}

      {/* Seat */}
      {createSeatModalOpen && (
        <CreateSeatModal
          open={createSeatModalOpen}
          onSubmit={(seat) => {
            setActiveSeat(seat);
            setSeatStatus("new");
          }}
          close={() => setCreateSeatModalOpen(false)}
        />
      )}
      {editSeatModalOpen && (
        <EditSeatModal
          open={editSeatModalOpen}
          seat={activeSeat!}
          onSubmit={setActiveSeat}
          close={() => setEditSeatModalOpen(false)}
        />
      )}

      <Wrapper>
        <SidePanel vertical gap={25}>
          <span
            onClick={() => navigate("/home")}
            style={{
              fontSize: 14,
              fontWeight: 700,
              textDecoration: "underline",
              cursor: "pointer",
            }}
          >
            Вернуться к бронированию
          </span>

          <div>
            {coworkingList.length > 0 ? (
              <StyledSelect
                style={{ fontSize: 20 }}
                suffixIcon={
                  <FontAwesomeIcon fill={"blue"} icon={faCaretDown} />
                }
                value={coworkingId}
                options={coworkingList.map((coworking) => ({
                  label: coworking.address,
                  value: coworking.id,
                }))}
                onChange={(id: number) => {
                  fetchCoworking(id, false);
                }}
              />
            ) : (
              <span style={{ fontSize: 15 }}>Нет коворкингов</span>
            )}
            <EditButtons
              leftVisible={coworkingList.length > 0}
              onDelete={handleCoworkingDelete}
              onEdit={handleCoworkingEdit}
              onCreate={handleCoworkingCreate}
            />
          </div>

          <div>
            {coworking?.floors && Number(coworking?.floors?.length) > 0 ? (
              <StyledSelect
                suffixIcon={
                  <FontAwesomeIcon fill={"blue"} icon={faCaretDown} />
                }
                value={selectedFloor}
                options={coworking.floors.map((floor) => ({
                  label: `${floor.number} этаж`,
                  value: floor.id,
                }))}
                onChange={(v: number) => handleFloorChange(v)}
              />
            ) : (
              <span style={{ fontSize: 15 }}>Нет этажей</span>
            )}
            <EditButtons
              leftVisible={Number(coworking?.floors?.length) > 0}
              onDelete={handleFloorDelete}
              onEdit={handleFloorEdit}
              onCreate={handleFloorCreate}
            />
          </div>

          <div>
            {seatStatus === "idle" ? (
              <NewSeatButton onClick={handleCreateSeat} />
            ) : (
              <div>
                <div style={{ marginBottom: 7 }}>
                  {seatStatus === "new" ? "Новое место" : "Редактировать место"}
                  :
                </div>
                <div style={{ fontSize: 15 }}>
                  <div>
                    <b>Номер</b>: {activeSeat!.number}
                  </div>
                  <div>
                    <b>Описание</b>: {activeSeat!.description}
                  </div>
                  <div>
                    <b>Стоимость</b> (руб/ч): {activeSeat!.price}
                  </div>
                </div>

                <SeatButtons
                  onDelete={
                    seatStatus === "edit" ? handleDeleteActiveSeat : undefined
                  }
                  onEdit={handleEditActiveSeat}
                  onCreate={handleSaveActiveSeat}
                  onCancel={handleCancelSeat}
                />
              </div>
            )}
          </div>

          <Flex justify="center">
            <Popover
              content={
                <Flex
                  vertical
                  gap={10}
                  style={{ maxWidth: 400, whiteSpace: "pre-wrap" }}
                >
                  <p>
                    Чтобы изменить рабочее место, нажмите на него один раз.
                    После этого вам станут доступны опции редактирования.
                  </p>
                  <p>
                    Вы можете менять положение места, перетаскивая его мышкой.
                  </p>
                  <p>
                    Чтобы изменить угол поворота места, прокрутире колесо мыши,
                    держа курсор над выбранным местом.
                  </p>
                  <p>
                    Чтобы изменить размер места, также воспользуйтесь колесом
                    мыши, зажав при это клавишу <b>Shift</b>.
                  </p>
                </Flex>
              }
            >
              <FontAwesomeIcon
                cursor="pointer"
                fontSize={30}
                icon={faCircleQuestion}
              />
            </Popover>
          </Flex>
        </SidePanel>

        {floor?.seats && (
          <Map onMouseMove={handleMouseMove} onMouseUp={handleMouseUp}>
            {loading && <MapLoader />}
            <MapWrapper ref={mapRef}>
              {floor.seats.map((seat) => (
                <div
                  onClick={() => {
                    if (seatStatus === "idle") {
                      setActiveSeat(seat);
                      setSeatStatus("edit");
                    }
                  }}
                  key={seat.id}
                  style={{
                    display: activeSeat?.id === seat.id ? "none" : "grid",
                    placeContent: "center",
                    position: "absolute",
                    width: 100,
                    height: 100,
                    top: seat.position.y - 50,
                    left: seat.position.x - 50,
                  }}
                >
                  <SeatComponent
                    $active={seatStatus === "idle"}
                    style={{
                      width: seat.position.width,
                      height: seat.position.height,
                      backgroundImage: `url(${seat.image.url})`,
                      transform: `rotate(${seat.position.angle}deg)`,
                      transformOrigin: "center",
                      position: "relative",
                      pointerEvents: seatStatus === "idle" ? "all" : "none",
                    }}
                  />
                </div>
              ))}

              {activeSeat && (
                <div
                  ref={rectRef}
                  style={{
                    display: "grid",
                    placeContent: "center",
                    position: "absolute",
                    width: 100,
                    height: 100,
                    top: activeSeat.position.y - 50,
                    left: activeSeat.position.x - 50,
                  }}
                >
                  <SeatComponent
                    $active
                    draggable="false"
                    style={{
                      width: activeSeat.position.width,
                      height: activeSeat.position.height,
                      backgroundImage: `url(${activeSeat.image.url})`,
                      transform: `rotate(${activeSeat.position.angle}deg)`,
                      transformOrigin: "center",
                      cursor: "grab",
                      position: "relative",
                    }}
                    onMouseDown={handleSeatDrag}
                    onWheel={(e) => {
                      if (e.shiftKey) {
                        const s = e.deltaY < 0 ? -1 : 1;
                        const f =
                          Math.max(
                            activeSeat?.position.width,
                            activeSeat?.position.height
                          ) /
                          Math.min(
                            activeSeat?.position.width,
                            activeSeat?.position.height
                          );
                        let width = activeSeat?.position.width;
                        let height = activeSeat?.position.height;
                        if (width < height) {
                          const mn = width + s;
                          const mx = Math.round(mn * f);
                          if (SEAT_MIN_WIDTH <= mx && mx <= SEAT_MAX_WIDTH) {
                            width = mn;
                            height = mx;
                          }
                        } else {
                          const mn = height + s;
                          const mx = Math.round(mn * f);
                          if (SEAT_MIN_WIDTH <= mx && mx <= SEAT_MAX_WIDTH) {
                            width = mx;
                            height = mn;
                          }
                        }
                        setActiveSeat({
                          ...activeSeat,
                          position: {
                            ...activeSeat?.position,
                            height,
                            width,
                          },
                        });
                        return;
                      }
                      let r = activeSeat?.position.angle + e.deltaY / 50;
                      if (r > 360) {
                        r -= 360;
                      }
                      if (r < 0) {
                        r += 360;
                      }
                      setActiveSeat({
                        ...activeSeat,
                        position: { ...activeSeat?.position, angle: r },
                      });
                    }}
                  />
                </div>
              )}

              <img
                draggable={false}
                style={{ maxWidth: 800 }}
                src={floor.image.url}
                alt="map"
              />
            </MapWrapper>
          </Map>
        )}
      </Wrapper>
    </StyledPage>
  );
};
