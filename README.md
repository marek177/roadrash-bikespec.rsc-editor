# Road Rash BIKESPEC.RSC Editor (C# / WinForms)

A small offline Windows editor for the Road Rash `BIKESPEC.RSC` motorcycle data file.

## Features

- C# WinForms application for Windows 10/11.
- Opens the 6,276-byte (`0x1884`) `BIKESPEC.RSC` format used by this reverse-engineering project.
- Select any of the 15 motorcycle slots from a drop-down list.
- Edit all 89 known 32-bit values for one bike, switch to another bike, and keep all unsaved edits in memory.
- Shows each field's relative offset, working name/group, per-bike factory/reference default, and the range used by all 15 reconstructed factory profiles.
- `Save` writes all edited bikes back to the file while preserving the non-payload bytes.
- `Save As...` creates a separate edited RSC.
- Automatic first-open backup: `BIKESPEC.RSC.original.bak` (created only once and never overwritten).
- Automatic pre-save backup: `BIKESPEC.RSC.lastsave.bak` (updated before every in-place save).
- `Restore selected bike` restores one motorcycle to its embedded reconstructed factory/reference profile.
- `Restore ALL factory defaults` restores all 15 embedded profiles in memory.
- `Restore original backup` directly loads the automatic first-open backup.
- `Restore backup...` can load any compatible `.bak` / `.rsc` backup.
- `Revert unsaved` returns to the last opened/saved state.
- Values outside the known factory range are highlighted; very large values trigger a warning before Save.

## Binary layout used

- File size: `0x1884` = 6,276 bytes
- Header / first physical slot: `0x240`
- Physical slot stride: `0x17C` = 380 bytes
- 15 physical slots
- Editable payload starts at `slot + 0x04`
- Editable payload size: `0x164` = 356 bytes
- 89 values × signed 32-bit little-endian integers

Only the 356-byte payload is rewritten for each motorcycle. The rest of the loaded file is preserved.

## Factory defaults

The embedded defaults are the 15 reconstructed reference profiles recovered during the `BIKESPEC.RSC` / `ROADRASH.EXE` reverse-engineering work. The analyzed RSC sample itself contained 15 copies of the CORSAIR Super #7 payload, so the distinct 15-bike reference values came from the recovered reference data rather than from that duplicated sample file.

The application does **not** contain or distribute the original Road Rash game data file.

## Build

Open `RoadRashBikeSpecEditor/RoadRashBikeSpecEditor.csproj` in Visual Studio 2022 with the .NET 8 SDK, or run:

```powershell
dotnet build .\RoadRashBikeSpecEditor\RoadRashBikeSpecEditor.csproj -c Release
```

To create a standalone Windows x64 single-file EXE:

```powershell
dotnet publish .\RoadRashBikeSpecEditor\RoadRashBikeSpecEditor.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:DebugType=None -p:DebugSymbols=false -o .\publish
```

A GitHub Actions workflow also builds the standalone Windows x64 version automatically.

## Safety / rollback workflow

1. Open the game's `BIKESPEC.RSC`.
2. The editor automatically creates `BIKESPEC.RSC.original.bak` if it does not already exist.
3. Edit one or more bikes and test in-game.
4. Before each normal Save, the current file is copied to `BIKESPEC.RSC.lastsave.bak`.
5. If the tuning becomes unplayable, use `Restore selected bike`, `Restore ALL factory defaults`, `Restore original backup`, or `Restore backup...` and then Save.
