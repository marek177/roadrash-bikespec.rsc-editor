namespace RoadRashBikeSpecEditor;

public sealed record BikeDefinition(int Id, string Name, int Hp, int Cc, string Series)
{
    public override string ToString() => $"{Id:00}  {Name}  [{Series}]  {Hp} hp / {Cc} cc";
}

public sealed record FieldDefinition(int Index, int Offset, string Name, string Group);
