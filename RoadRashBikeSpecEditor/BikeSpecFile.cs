using System.Buffers.Binary;

namespace RoadRashBikeSpecEditor;

public static class BikeSpecFile
{
    public const int HeaderSize = 0x240;
    public const int SlotStride = 0x17C;
    public const int PayloadOffsetWithinSlot = 0x04;
    public const int PayloadSize = 0x164;
    public const int FieldCount = 89;
    public const int BikeCount = 15;
    public const int ExpectedFileSize = HeaderSize + BikeCount * SlotStride; // 6276 bytes

    public static byte[] LoadBytes(string path)
    {
        var bytes = File.ReadAllBytes(path);
        Validate(bytes);
        return bytes;
    }

    public static void Validate(ReadOnlySpan<byte> bytes)
    {
        if (bytes.Length != ExpectedFileSize)
        {
            throw new InvalidDataException(
                $"Unexpected BIKESPEC.RSC size: {bytes.Length} bytes. " +
                $"Expected exactly {ExpectedFileSize} bytes (0x{ExpectedFileSize:X}).");
        }
    }

    public static int[][] ReadValues(ReadOnlySpan<byte> bytes)
    {
        Validate(bytes);
        var values = new int[BikeCount][];

        for (var bike = 0; bike < BikeCount; bike++)
        {
            values[bike] = new int[FieldCount];
            var payloadBase = GetPayloadBase(bike);

            for (var field = 0; field < FieldCount; field++)
            {
                var offset = payloadBase + field * 4;
                values[bike][field] = BinaryPrimitives.ReadInt32LittleEndian(bytes.Slice(offset, 4));
            }
        }

        return values;
    }

    public static byte[] WriteValues(ReadOnlySpan<byte> originalBytes, int[][] values)
    {
        Validate(originalBytes);
        ValidateValues(values);

        var output = originalBytes.ToArray();

        for (var bike = 0; bike < BikeCount; bike++)
        {
            var payloadBase = GetPayloadBase(bike);

            for (var field = 0; field < FieldCount; field++)
            {
                var offset = payloadBase + field * 4;
                BinaryPrimitives.WriteInt32LittleEndian(output.AsSpan(offset, 4), values[bike][field]);
            }
        }

        return output;
    }

    public static int GetSlotBase(int bikeIndex)
    {
        ValidateBikeIndex(bikeIndex);
        return HeaderSize + bikeIndex * SlotStride;
    }

    public static int GetPayloadBase(int bikeIndex)
    {
        return GetSlotBase(bikeIndex) + PayloadOffsetWithinSlot;
    }

    public static string GetOriginalBackupPath(string sourcePath) => sourcePath + ".original.bak";
    public static string GetLastSaveBackupPath(string sourcePath) => sourcePath + ".lastsave.bak";

    public static string EnsureOriginalBackup(string sourcePath)
    {
        var backupPath = GetOriginalBackupPath(sourcePath);

        if (!File.Exists(backupPath))
            File.Copy(sourcePath, backupPath, overwrite: false);

        return backupPath;
    }

    public static string CreateLastSaveBackup(string sourcePath)
    {
        var backupPath = GetLastSaveBackupPath(sourcePath);
        File.Copy(sourcePath, backupPath, overwrite: true);
        return backupPath;
    }

    public static int[][] CloneValues(int[][] source)
    {
        var clone = new int[source.Length][];
        for (var i = 0; i < source.Length; i++)
            clone[i] = (int[])source[i].Clone();
        return clone;
    }

    private static void ValidateValues(int[][] values)
    {
        if (values.Length != BikeCount)
            throw new InvalidDataException($"Expected {BikeCount} bike value arrays.");

        for (var bike = 0; bike < BikeCount; bike++)
        {
            if (values[bike].Length != FieldCount)
                throw new InvalidDataException($"Bike #{bike + 1} must contain {FieldCount} fields.");
        }
    }

    private static void ValidateBikeIndex(int bikeIndex)
    {
        if ((uint)bikeIndex >= BikeCount)
            throw new ArgumentOutOfRangeException(nameof(bikeIndex));
    }
}
