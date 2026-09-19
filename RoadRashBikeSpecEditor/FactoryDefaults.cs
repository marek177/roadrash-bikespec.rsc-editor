namespace RoadRashBikeSpecEditor;

public static class FactoryDefaults
{
    static FactoryDefaults()
    {
        if (Fields.Length != BikeSpecFile.FieldCount || ByField.Length != BikeSpecFile.FieldCount)
            throw new InvalidOperationException("Factory field table must contain exactly 89 fields.");

        if (Bikes.Length != BikeSpecFile.BikeCount)
            throw new InvalidOperationException("Factory bike table must contain exactly 15 bikes.");

        foreach (var row in ByField)
        {
            if (row.Length != BikeSpecFile.BikeCount)
                throw new InvalidOperationException("Every factory field row must contain 15 bike values.");
        }
    }

    // Reconstructed factory/reference profiles from the reverse-engineered bikespecs data.
    public static readonly BikeDefinition[] Bikes =
    {
        new(1, "BANZAI", 60, 450, "Rat"),
        new(2, "CORSAIR", 45, 400, "Rat"),
        new(3, "KAMIKAZE", 50, 250, "Rat"),
        new(4, "KILLER / KILLERRAT", 35, 125, "Rat"),
        new(5, "PERRO", 65, 500, "Rat"),
        new(6, "BANZAI", 140, 1100, "Super"),
        new(7, "CORSAIR", 100, 600, "Super"),
        new(8, "DIABLO", 160, 1000, "Super"),
        new(9, "KAMIKAZE", 120, 750, "Super"),
        new(10, "STILETTO", 140, 900, "Super"),
        new(11, "DIABLO", 90, 750, "Sport"),
        new(12, "DMG", 120, 1000, "Sport"),
        new(13, "KAMIKAZE", 100, 750, "Sport"),
        new(14, "PERRO", 75, 250, "Sport"),
        new(15, "STILETTO", 75, 600, "Sport")
    };

    public static readonly FieldDefinition[] Fields =
    {
        new(0, 0x000, "type_byte_marker", "Identification"),
        new(1, 0x004, "base_scale_A", "Base"),
        new(2, 0x008, "base_scale_B", "Base"),
        new(3, 0x00C, "base_scale_C", "Base"),
        new(4, 0x010, "variant_or_flags", "Base"),
        new(5, 0x014, "performance_scale", "Base"),
        new(6, 0x018, "load_tuning_adjustment", "Base"),
        new(7, 0x01C, "runtime_state_initial", "Base"),
        new(8, 0x020, "engine_base_threshold", "Engine / gears"),
        new(9, 0x024, "engine_base_coefficient", "Engine / gears"),
        new(10, 0x028, "gear_0_upper_threshold", "Engine / gears"),
        new(11, 0x02C, "gear_0_lower_threshold", "Engine / gears"),
        new(12, 0x030, "gear_0_drive_coefficient", "Engine / gears"),
        new(13, 0x034, "gear_0_secondary_coefficient", "Engine / gears"),
        new(14, 0x038, "gear_1_upper_threshold", "Engine / gears"),
        new(15, 0x03C, "gear_1_lower_threshold", "Engine / gears"),
        new(16, 0x040, "gear_1_drive_coefficient", "Engine / gears"),
        new(17, 0x044, "gear_1_secondary_coefficient", "Engine / gears"),
        new(18, 0x048, "gear_2_upper_threshold", "Engine / gears"),
        new(19, 0x04C, "gear_2_lower_threshold", "Engine / gears"),
        new(20, 0x050, "gear_2_drive_coefficient", "Engine / gears"),
        new(21, 0x054, "gear_2_secondary_coefficient", "Engine / gears"),
        new(22, 0x058, "gear_3_upper_threshold", "Engine / gears"),
        new(23, 0x05C, "gear_3_lower_threshold", "Engine / gears"),
        new(24, 0x060, "gear_3_drive_coefficient", "Engine / gears"),
        new(25, 0x064, "gear_3_secondary_coefficient", "Engine / gears"),
        new(26, 0x068, "gear_4_upper_threshold", "Engine / gears"),
        new(27, 0x06C, "gear_4_lower_threshold", "Engine / gears"),
        new(28, 0x070, "gear_4_drive_coefficient", "Engine / gears"),
        new(29, 0x074, "gear_4_secondary_coefficient", "Engine / gears"),
        new(30, 0x078, "gear_5_upper_threshold", "Engine / gears"),
        new(31, 0x07C, "gear_5_lower_threshold", "Engine / gears"),
        new(32, 0x080, "gear_5_drive_coefficient", "Engine / gears"),
        new(33, 0x084, "gear_5_secondary_coefficient", "Engine / gears"),
        new(34, 0x088, "runtime_or_reserved_088", "Runtime / reserved"),
        new(35, 0x08C, "runtime_or_reserved_08C", "Runtime / reserved"),
        new(36, 0x090, "runtime_or_reserved_090", "Runtime / reserved"),
        new(37, 0x094, "runtime_or_reserved_094", "Runtime / reserved"),
        new(38, 0x098, "runtime_or_reserved_098", "Runtime / reserved"),
        new(39, 0x09C, "physics_lower_clamp", "Physics"),
        new(40, 0x0A0, "physics_upper_clamp", "Physics"),
        new(41, 0x0A4, "display_acceleration_rating", "Displayed ratings"),
        new(42, 0x0A8, "rating_scale_divisor", "Displayed ratings"),
        new(43, 0x0AC, "derived_rating_divisor_A", "Displayed ratings"),
        new(44, 0x0B0, "derived_rating_divisor_B", "Displayed ratings"),
        new(45, 0x0B4, "runtime_or_reserved_0B4", "Runtime / reserved"),
        new(46, 0x0B8, "runtime_or_reserved_0B8", "Runtime / reserved"),
        new(47, 0x0BC, "runtime_or_reserved_0BC", "Runtime / reserved"),
        new(48, 0x0C0, "legacy_overwritten_value", "Initialization"),
        new(49, 0x0C4, "derived_scale_A", "Initialization"),
        new(50, 0x0C8, "derived_scale_B", "Initialization"),
        new(51, 0x0CC, "runtime_or_reserved_0CC", "Runtime / reserved"),
        new(52, 0x0D0, "runtime_or_reserved_0D0", "Runtime / reserved"),
        new(53, 0x0D4, "runtime_or_reserved_0D4", "Runtime / reserved"),
        new(54, 0x0D8, "runtime_or_reserved_0D8", "Runtime / reserved"),
        new(55, 0x0DC, "runtime_or_reserved_0DC", "Runtime / reserved"),
        new(56, 0x0E0, "runtime_or_reserved_0E0", "Runtime / reserved"),
        new(57, 0x0E4, "handling_coefficient_A", "Handling"),
        new(58, 0x0E8, "handling_coefficient_B", "Handling"),
        new(59, 0x0EC, "runtime_or_reserved_0EC", "Runtime / reserved"),
        new(60, 0x0F0, "physics_fixed_scale", "Physics"),
        new(61, 0x0F4, "response_coefficient_A", "Handling"),
        new(62, 0x0F8, "response_coefficient_B", "Handling"),
        new(63, 0x0FC, "response_coefficient_C", "Handling"),
        new(64, 0x100, "response_coefficient_D", "Handling"),
        new(65, 0x104, "response_limit", "Handling"),
        new(66, 0x108, "fixed_point_unity", "Physics"),
        new(67, 0x10C, "fixed_point_coefficient_A", "Physics"),
        new(68, 0x110, "fixed_point_coefficient_B", "Physics"),
        new(69, 0x114, "fixed_point_coefficient_C", "Physics"),
        new(70, 0x118, "physics_step_or_limit", "Physics"),
        new(71, 0x11C, "runtime_or_reserved_11C", "Runtime / reserved"),
        new(72, 0x120, "runtime_or_reserved_120", "Runtime / reserved"),
        new(73, 0x124, "geometry_physics_coefficient", "Physics"),
        new(74, 0x128, "steering_response_left", "Handling"),
        new(75, 0x12C, "steering_response_right", "Handling"),
        new(76, 0x130, "steering_physics_clamp", "Handling"),
        new(77, 0x134, "runtime_or_reserved_134", "Runtime / reserved"),
        new(78, 0x138, "byte_scale_max", "Physics"),
        new(79, 0x13C, "runtime_or_reserved_13C", "Runtime / reserved"),
        new(80, 0x140, "durability_display_reference", "Damage"),
        new(81, 0x144, "max_durability", "Damage"),
        new(82, 0x148, "nitro_marker_overwritten", "Nitro"),
        new(83, 0x14C, "nitro_recharge_rate", "Nitro"),
        new(84, 0x150, "nitro_boost_multiplier", "Nitro"),
        new(85, 0x154, "nitro_capacity", "Nitro"),
        new(86, 0x158, "nitro_duration_counter", "Nitro"),
        new(87, 0x15C, "nitro_activation_threshold", "Nitro"),
        new(88, 0x160, "nitro_reserved", "Nitro")
    };

    // Indexed as [field][bike].
    private static readonly int[][] ByField =
    {
        new[] { -800, -800, -800, -800, -800, -800, -800, -800, -800, -800, -800, -800, -800, -800, -800 },
        new[] { 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024 },
        new[] { 160, 160, 160, 160, 160, 160, 160, 160, 160, 160, 160, 160, 160, 160, 160 },
        new[] { 160, 160, 160, 160, 160, 160, 160, 160, 160, 160, 160, 160, 160, 160, 160 },
        new[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        new[] { 196, 256, 164, 196, 128, 196, 256, 196, 320, 196, 256, 384, 320, 164, 196 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 5000, 4096, 3800, 5000, 3800, 4000, 4000, 4000, 4000, 4000, 4000, 3000, 4000, 3800, 4000 },
        new[] { 256, 248, 256, 256, 256, 256, 256, 256, 256, 256, 256, 240, 248, 256, 256 },
        new[] { 3000, 3000, 4000, 3000, 4000, 3500, 4000, 3584, 4000, 4000, 3000, 4000, 3000, 4000, 3000 },
        new[] { -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999, -999 },
        new[] { 3000, 2048, 2500, 3000, 2500, 2400, 2400, 2048, 2800, 2400, 2400, 2200, 2400, 2500, 2400 },
        new[] { 246, 240, 248, 248, 248, 256, 248, 248, 240, 250, 248, 240, 240, 248, 248 },
        new[] { 5000, 5000, 6000, 5000, 6000, 6000, 6000, 7168, 5500, 6000, 5000, 6000, 5000, 6000, 5000 },
        new[] { 2500, 2500, 3500, 2500, 3500, 3000, 2500, 2816, 3500, 3500, 2500, 3500, 2500, 3500, 2500 },
        new[] { 2000, 1280, 2000, 2000, 2000, 1500, 1800, 1280, 1900, 1700, 1800, 1700, 1800, 2000, 1800 },
        new[] { 238, 232, 240, 240, 240, 256, 240, 240, 232, 240, 240, 240, 232, 244, 240 },
        new[] { 7000, 7000, 7000, 7000, 7000, 9000, 8000, 10752, 8000, 8500, 7000, 8000, 7000, 7500, 7000 },
        new[] { 4500, 4500, 5500, 4500, 5500, 5500, 5500, 6400, 5000, 5500, 4500, 5500, 4500, 5500, 4500 },
        new[] { 1600, 1024, 1800, 1700, 1800, 1200, 1400, 1024, 1500, 1300, 1400, 1400, 1400, 1600, 1400 },
        new[] { 226, 220, 232, 236, 232, 248, 232, 232, 224, 230, 236, 232, 224, 240, 236 },
        new[] { 8500, 8500, 8000, 8500, 8000, 12000, 10000, 14336, 10000, 11000, 9000, 10000, 9000, 9000, 8500 },
        new[] { 6500, 6500, 6500, 6500, 6500, 8500, 7500, 9984, 7500, 7500, 6500, 7500, 6500, 7000, 6500 },
        new[] { 1200, 768, 1600, 1400, 1600, 1000, 1200, 768, 1200, 1050, 1200, 1200, 1200, 1400, 1100 },
        new[] { 216, 214, 224, 224, 224, 244, 224, 224, 218, 224, 224, 224, 216, 232, 224 },
        new[] { 21504, 21504, 9000, 9500, 9000, 14000, 12000, 17920, 12000, 13500, 11000, 12000, 11000, 10500, 9500 },
        new[] { 8000, 8000, 7500, 8000, 7500, 11500, 9500, 13568, 9500, 10500, 8500, 9500, 8500, 8500, 8000 },
        new[] { 512, 512, 1400, 1200, 1400, 900, 1000, 512, 1000, 900, 1000, 1000, 1000, 1100, 800 },
        new[] { 208, 204, 212, 214, 211, 240, 218, 214, 212, 220, 213, 212, 208, 209, 211 },
        new[] { 16777215, 16777215, 16777215, 16777215, 16777215, 16777215, 16777215, 16777215, 16777215, 16777215, 16777215, 16777215, 16777215, 16777215, 16777215 },
        new[] { 17152, 17152, 8500, 9000, 8500, 13500, 11500, 17152, 11500, 13000, 10500, 11500, 10500, 10000, 9000 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000 },
        new[] { 45000, 45000, 45000, 45000, 45000, 45000, 45000, 45000, 45000, 45000, 45000, 45000, 45000, 45000, 45000 },
        new[] { 38, 36, 37, 40, 35, 42, 44, 48, 45, 47, 43, 44, 42, 41, 40 },
        new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 },
        new[] { 60, 60, 256, 60, 256, 128, 60, 128, 60, 128, 60, 60, 60, 256, 60 },
        new[] { 64, 64, 128, 96, 128, 190, 90, 256, 96, 190, 160, 32, 96, 128, 128 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { -60, -60, -80, -60, -120, -120, -90, -120, -90, -100, -90, -40, -60, -100, -80 },
        new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 },
        new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 64, 96, 160, 128, 64, 64, 128, 196, 196, 160, 160, 96, 128, 32, 196 },
        new[] { 320, 212, 256, 196, 320, 400, 256, 320, 340, 340, 304, 304, 320, 400, 256 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 11776, 11776, 11776, 11776, 11776, 11776, 11776, 11776, 11776, 11776, 11776, 11776, 11776, 11776, 11776 },
        new[] { 50, 60, 45, 40, 25, 30, 45, 30, 50, 40, 50, 70, 65, 30, 40 },
        new[] { 30, 70, 90, 50, 30, 40, 45, 25, 90, 90, 60, 90, 90, 25, 120 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120, 120 },
        new[] { 65536, 65536, 65536, 65536, 65536, 65536, 65536, 65536, 65536, 65536, 65536, 65536, 65536, 65536, 65536 },
        new[] { 196, 196, 196, 196, 196, 196, 196, 196, 196, 196, 196, 196, 196, 196, 196 },
        new[] { 2560, 2560, 2560, 2560, 2560, 2560, 2560, 2560, 2560, 2560, 2560, 2560, 2560, 2560, 2560 },
        new[] { 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024, 1024 },
        new[] { 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 346, 346, 346, 346, 346, 346, 346, 346, 346, 346, 346, 346, 346, 346, 346 },
        new[] { 128, 120, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128, 128 },
        new[] { 196, 196, 256, 196, 256, 196, 196, 128, 128, 128, 128, 64, 128, 256, 196 },
        new[] { 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000, 20000 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127, 127 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new[] { 160, 142, 90, 160, 128, 320, 160, 256, 196, 196, 160, 396, 256, 160, 128 },
        new[] { 160, 142, 90, 160, 128, 320, 160, 256, 196, 196, 160, 396, 256, 160, 128 },
        new[] { 0, 0, 0, 0, 0, 0, 256, 256, 256, 256, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 288, 384, 320, 352, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 180, 240, 200, 220, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 10, 10, 10, 10, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 120, 120, 120, 120, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 15, 15, 15, 15, 0, 0, 0, 0, 0 },
        new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };

    public static int[] GetBikeDefaults(int bikeIndex)
    {
        ValidateBikeIndex(bikeIndex);
        var values = new int[Fields.Length];
        for (var field = 0; field < Fields.Length; field++)
            values[field] = ByField[field][bikeIndex];
        return values;
    }

    public static int GetDefault(int bikeIndex, int fieldIndex)
    {
        ValidateBikeIndex(bikeIndex);
        if ((uint)fieldIndex >= Fields.Length)
            throw new ArgumentOutOfRangeException(nameof(fieldIndex));
        return ByField[fieldIndex][bikeIndex];
    }

    public static (int Min, int Max) GetFactoryRange(int fieldIndex)
    {
        if ((uint)fieldIndex >= Fields.Length)
            throw new ArgumentOutOfRangeException(nameof(fieldIndex));

        var row = ByField[fieldIndex];
        var min = row[0];
        var max = row[0];
        for (var i = 1; i < row.Length; i++)
        {
            if (row[i] < min) min = row[i];
            if (row[i] > max) max = row[i];
        }
        return (min, max);
    }

    public static int[][] CreateAllDefaults()
    {
        var result = new int[Bikes.Length][];
        for (var bike = 0; bike < Bikes.Length; bike++)
            result[bike] = GetBikeDefaults(bike);
        return result;
    }

    private static void ValidateBikeIndex(int bikeIndex)
    {
        if ((uint)bikeIndex >= Bikes.Length)
            throw new ArgumentOutOfRangeException(nameof(bikeIndex));
    }
}
