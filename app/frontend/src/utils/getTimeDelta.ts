export const getTimeDelta = (timeFrom: string, timeTo: string) => {
  const spFrom = timeFrom.split(":");
  const spTo = timeTo.split(":");
  const hourFrom = +spFrom[0];
  const minuteFrom = +spFrom[1];
  const hourTo = +spTo[0];
  const minuteTo = +spTo[1];
  const tFrom = hourFrom * 60 + minuteFrom;
  const tTo = hourTo * 60 + minuteTo;
  return (tTo - tFrom) / 60;
};
