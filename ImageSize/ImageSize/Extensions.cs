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
        return (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
    }
    
    public static int ReadInt32LittleEndian(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(4);
        return bytes[0] | (bytes[1] << 8) | (bytes[2] << 16) | (bytes[3] << 24);
    }
    
    public static short ReadInt16LittleEndian(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(2);
        return (short)(bytes[0] | (bytes[1] << 8));
    }
    
    public static ushort ReadUInt16BigEndian(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(2);
        if (bytes.Length < 2)
            throw new EndOfStreamException();

        return (ushort)((bytes[0] << 8) | bytes[1]);
    }
    
    public static uint ReadUInt32BigEndian(this BinaryReader reader)
    {
        var bytes = reader.ReadBytes(4);
        if (bytes.Length < 4)
            throw new EndOfStreamException();

        return ((uint)bytes[0] << 24) | ((uint)bytes[1] << 16) | ((uint)bytes[2] << 8) | bytes[3];
    }
}