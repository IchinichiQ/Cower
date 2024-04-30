export const formatOrderTime = (timeFrom: number, timeTo: number, date: string) => {
  const timeString = `${String(timeFrom).padStart(2, '0')}:00 - ${String(timeTo).padStart(2, '0')}:00`
  return `${new Date(date).toLocaleDateString()}, ${timeString}`;
}
