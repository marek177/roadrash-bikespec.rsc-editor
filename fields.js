// Field dictionary for the 0x164-byte editable bike payload.
// Names can be replaced as individual meanings are confirmed by reverse engineering.

window.BIKE_FIELDS = Array.from({ length: 89 }, (_, index) => ({
  index,
  name: `Field ${String(index).padStart(2, '0')}`,
  relativeOffset: index * 4,
  type: 'int32le'
}));
