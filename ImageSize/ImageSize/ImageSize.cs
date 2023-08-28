namespace ImageSize;

public class ImageSize
{
    public static (int width, int height)? GetImageSize(string filePath)
    {
        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (var binaryReader = new BinaryReader(fileStream))
            {
                byte[] header = binaryReader.ReadBytes(8);

                // Check for JPEG magic numbers
                if (header[0] == 0xFF && header[1] == 0xD8)
                {
                    while (fileStream.Position < fileStream.Length)
                    {
                        byte marker = binaryReader.ReadByte();
                        byte markerType = binaryReader.ReadByte();
                        short length = binaryReader.ReadInt16BigEndian();

                        if (marker == 0xFF && (markerType >= 0xC0 && markerType <= 0xCF) && markerType != 0xC4 && markerType != 0xC8 && markerType != 0xCC)
                        {
                            binaryReader.ReadByte(); // Skip 1 byte
                            int height = binaryReader.ReadInt16BigEndian();
                            int width = binaryReader.ReadInt16BigEndian();
                            return (width, height);
                        }
                        else
                        {
                            fileStream.Seek(length - 2, SeekOrigin.Current);
                        }
                    }
                }

                // Check for PNG magic numbers
                else if (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47)
                {
                    fileStream.Seek(16, SeekOrigin.Begin);
                    int width = binaryReader.ReadInt32BigEndian();
                    int height = binaryReader.ReadInt32BigEndian();
                    return (width, height);
                }
                
                // Check for GIF magic numbers
                else if (header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46)
                {
                    fileStream.Seek(6, SeekOrigin.Begin);
                    int width = binaryReader.ReadInt16LittleEndian();
                    int height = binaryReader.ReadInt16LittleEndian();
                    return (width, height);
                }

                // Check for BMP magic numbers
                else if (header[0] == 0x42 && header[1] == 0x4D)
                {
                    fileStream.Seek(18, SeekOrigin.Begin);
                    int width = binaryReader.ReadInt32LittleEndian();
                    int height = binaryReader.ReadInt32LittleEndian();
                    return (width, height);
                }
            }
        }

        return null;
    }
}