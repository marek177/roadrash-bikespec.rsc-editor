# Road Rash BIKESPEC.RSC Editor

Offline editor for the motorcycle specification file used by **Road Rash**.

## Current reverse-engineered format

The editor currently uses the binary layout confirmed from `BIKESPEC.RSC` analysis:

- File size expected: **6,276 bytes** (`0x1884`)
- Header: **576 bytes** (`0x240`)
- Physical bike slots: **15**
- Physical slot stride: **380 bytes** (`0x17C`)
- Editable payload per slot: **356 bytes** (`0x164`)
- Payload interpretation: **89 little-endian signed 32-bit integers**
- Padding after each payload: **24 bytes** (`0x18`)

The editor preserves the header and padding bytes and only replaces the 89 editable `int32` values in each selected bike slot.

## Usage

1. Open `index.html` in Edge/Chrome on Windows 11.
2. Click **Open BIKESPEC.RSC**.
3. Select bike slot 1–15.
4. Edit the values.
5. Click **Apply values to memory**.
6. Click **Save As...** to download the modified `BIKESPEC.RSC`.

The original file is not overwritten by the browser.

## Notes

Field names are initially shown as `Field 00` … `Field 88` together with their absolute and record-relative offsets. As individual meanings are confirmed from `ROADRASH.EXE` / `RASHME.EXE` reverse engineering, they can be renamed in `fields.js` without changing the binary reader/writer.

## Status

This repository is a reconstruction/reverse-engineering tool. It does not include original Road Rash game data.
