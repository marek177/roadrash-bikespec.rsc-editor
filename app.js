(() => {
  'use strict';

  const HEADER_SIZE = 0x240;
  const BIKE_COUNT = 15;
  const SLOT_STRIDE = 0x17C;
  const PAYLOAD_SIZE = 0x164;
  const FIELD_COUNT = 89;
  const EXPECTED_SIZE = HEADER_SIZE + BIKE_COUNT * SLOT_STRIDE;

  const fileInput = document.getElementById('fileInput');
  const bikeSelect = document.getElementById('bikeSelect');
  const tableBody = document.getElementById('tableBody');
  const status = document.getElementById('status');
  const applyBtn = document.getElementById('applyBtn');
  const saveBtn = document.getElementById('saveBtn');
  const resetBtn = document.getElementById('resetBtn');
  const fileInfo = document.getElementById('fileInfo');

  let originalBytes = null;
  let workingBytes = null;
  let loadedFileName = 'BIKESPEC.RSC';

  function hex(value, width = 4) {
    return '0x' + value.toString(16).toUpperCase().padStart(width, '0');
  }

  function slotBase(slotIndex) {
    return HEADER_SIZE + slotIndex * SLOT_STRIDE;
  }

  function setStatus(message, kind = '') {
    status.textContent = message;
    status.className = kind ? `status ${kind}` : 'status';
  }

  function setEnabled(enabled) {
    bikeSelect.disabled = !enabled;
    applyBtn.disabled = !enabled;
    saveBtn.disabled = !enabled;
    resetBtn.disabled = !enabled;
  }

  function populateBikeSelect() {
    bikeSelect.innerHTML = '';
    for (let i = 0; i < BIKE_COUNT; i++) {
      const option = document.createElement('option');
      option.value = i;
      option.textContent = `Bike slot ${i + 1}`;
      bikeSelect.appendChild(option);
    }
  }

  function validateFile(bytes) {
    if (bytes.byteLength !== EXPECTED_SIZE) {
      throw new Error(
        `Unexpected file size: ${bytes.byteLength} bytes. Expected ${EXPECTED_SIZE} bytes (${hex(EXPECTED_SIZE)}).`
      );
    }
  }

  function renderSlot(slotIndex) {
    if (!workingBytes) return;

    const view = new DataView(workingBytes.buffer, workingBytes.byteOffset, workingBytes.byteLength);
    const base = slotBase(slotIndex);
    tableBody.innerHTML = '';

    for (const field of window.BIKE_FIELDS) {
      const absoluteOffset = base + field.relativeOffset;
      const value = view.getInt32(absoluteOffset, true);

      const tr = document.createElement('tr');

      const tdIndex = document.createElement('td');
      tdIndex.textContent = field.index;

      const tdName = document.createElement('td');
      tdName.textContent = field.name;

      const tdRel = document.createElement('td');
      tdRel.textContent = hex(field.relativeOffset, 3);

      const tdAbs = document.createElement('td');
      tdAbs.textContent = hex(absoluteOffset, 4);

      const tdValue = document.createElement('td');
      const input = document.createElement('input');
      input.type = 'number';
      input.step = '1';
      input.min = '-2147483648';
      input.max = '2147483647';
      input.value = String(value);
      input.dataset.fieldIndex = String(field.index);
      input.className = 'valueInput';
      tdValue.appendChild(input);

      tr.append(tdIndex, tdName, tdRel, tdAbs, tdValue);
      tableBody.appendChild(tr);
    }

    fileInfo.textContent =
      `Selected slot ${slotIndex + 1}: base ${hex(base, 4)}, payload ${hex(PAYLOAD_SIZE, 3)}, ` +
      `padding ${hex(SLOT_STRIDE - PAYLOAD_SIZE, 2)}`;
  }

  function applyCurrentSlot() {
    if (!workingBytes) return false;

    const slotIndex = Number(bikeSelect.value);
    const base = slotBase(slotIndex);
    const view = new DataView(workingBytes.buffer, workingBytes.byteOffset, workingBytes.byteLength);
    const inputs = tableBody.querySelectorAll('.valueInput');

    for (const input of inputs) {
      const fieldIndex = Number(input.dataset.fieldIndex);
      const parsed = Number(input.value);

      if (!Number.isInteger(parsed) || parsed < -2147483648 || parsed > 2147483647) {
        input.focus();
        setStatus(`Invalid int32 value in field ${fieldIndex}.`, 'error');
        return false;
      }

      view.setInt32(base + fieldIndex * 4, parsed, true);
    }

    setStatus(`Changes applied to bike slot ${slotIndex + 1}.`, 'ok');
    return true;
  }

  function resetCurrentSlot() {
    if (!workingBytes || !originalBytes) return;

    const slotIndex = Number(bikeSelect.value);
    const base = slotBase(slotIndex);
    const end = base + PAYLOAD_SIZE;
    workingBytes.set(originalBytes.slice(base, end), base);
    renderSlot(slotIndex);
    setStatus(`Bike slot ${slotIndex + 1} restored from the originally loaded file.`, 'ok');
  }

  function saveAs() {
    if (!workingBytes) return;
    if (!applyCurrentSlot()) return;

    const blob = new Blob([workingBytes], { type: 'application/octet-stream' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = loadedFileName || 'BIKESPEC.RSC';
    document.body.appendChild(a);
    a.click();
    a.remove();
    setTimeout(() => URL.revokeObjectURL(url), 1000);
    setStatus('Modified BIKESPEC.RSC exported. Header and padding bytes were preserved.', 'ok');
  }

  async function loadFile(file) {
    const buffer = await file.arrayBuffer();
    const bytes = new Uint8Array(buffer);
    validateFile(bytes);

    originalBytes = new Uint8Array(bytes);
    workingBytes = new Uint8Array(bytes);
    loadedFileName = file.name || 'BIKESPEC.RSC';

    setEnabled(true);
    bikeSelect.value = '0';
    renderSlot(0);
    setStatus(
      `Loaded ${loadedFileName}: ${bytes.byteLength} bytes, ${BIKE_COUNT} bike slots, ${FIELD_COUNT} int32 fields per slot.`,
      'ok'
    );
  }

  fileInput.addEventListener('change', async (event) => {
    const file = event.target.files?.[0];
    if (!file) return;

    try {
      await loadFile(file);
    } catch (error) {
      originalBytes = null;
      workingBytes = null;
      setEnabled(false);
      tableBody.innerHTML = '';
      fileInfo.textContent = '';
      setStatus(error.message || String(error), 'error');
    }
  });

  bikeSelect.addEventListener('change', () => {
    renderSlot(Number(bikeSelect.value));
    setStatus(`Viewing bike slot ${Number(bikeSelect.value) + 1}.`);
  });

  applyBtn.addEventListener('click', applyCurrentSlot);
  saveBtn.addEventListener('click', saveAs);
  resetBtn.addEventListener('click', resetCurrentSlot);

  populateBikeSelect();
  setEnabled(false);
  setStatus('Open a BIKESPEC.RSC file to begin.');
})();
