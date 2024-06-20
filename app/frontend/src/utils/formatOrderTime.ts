export const formatOrderTime = (
  timeFrom: string,
  timeTo: string,
  date: string,
) => {
  const timeString = `${timeFrom} - ${timeTo}`;
  return `${new Date(date).toLocaleDateString()}, ${timeString}`;
};
