export const hourOptions = Array(24)
  .fill(null)
  .map((_, index) => ({ value: index, label: String(index).padStart(2, "0") }));
export const minuteOptions = Array(60)
  .fill(null)
  .map((_, index) => ({
    value: index,
    label: String(index).padStart(2, "0"),
  }));
