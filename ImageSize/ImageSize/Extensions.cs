namespace ImageSize;

public static class Extensions
{
    public static short ReadInt16BigEndian(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(2);
        return (short)((bytes[0] << 8) | bytes[1]);
    }

    public static int ReadInt32BigEndian(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(4);
        return (int)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]);
    }
}