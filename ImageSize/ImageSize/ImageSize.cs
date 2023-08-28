namespace ImageSize;

public static class ImageSize
{
    public static (int width, int height)? GetImageSize(Stream imageStream)
    {
        using var binaryReader = new BinaryReader(imageStream);
        
        var header = binaryReader.ReadBytes(8);

        // Check for JPEG magic numbers
        if (header[0] == 0xFF && header[1] == 0xD8)
        {
            return GetJpegSize(binaryReader);
        }

        // Check for PNG magic numbers
        else if (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47)
        {
            binaryReader.BaseStream.Seek(16, SeekOrigin.Begin);
            var width = binaryReader.ReadInt32BigEndian();
            var height = binaryReader.ReadInt32BigEndian();
            return (width, height);
        }
                
        // Check for GIF magic numbers
        else if (header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46)
        {
            binaryReader.BaseStream.Seek(6, SeekOrigin.Begin);
            int width = binaryReader.ReadInt16LittleEndian();
            int height = binaryReader.ReadInt16LittleEndian();
            return (width, height);
        }

        // Check for BMP magic numbers
        else if (header[0] == 0x42 && header[1] == 0x4D)
        {
            binaryReader.BaseStream.Seek(18, SeekOrigin.Begin);
            var width = binaryReader.ReadInt32LittleEndian();
            var height = binaryReader.ReadInt32LittleEndian();
            return (width, height);
        }

        return null;
    }
    
    private static (int width, int height)? GetJpegSize(BinaryReader binaryReader)
    {
        while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
        {
            byte marker = binaryReader.ReadByte();

            if (marker != 0xFF)
                continue;

            byte segmentType = binaryReader.ReadByte();

            if (segmentType >= 0xC0 && segmentType <= 0xCF && segmentType != 0xC4 && segmentType != 0xC8 && segmentType != 0xCC)
            {
                binaryReader.ReadByte(); // Skip length high byte
                binaryReader.ReadByte(); // Skip length low byte
                binaryReader.ReadByte(); // Skip bits/sample
                int height = binaryReader.ReadInt16BigEndian();
                int width = binaryReader.ReadInt16BigEndian();
                return (width, height);
            }
            else
            {
                int segmentLength = binaryReader.ReadInt16BigEndian();
                binaryReader.BaseStream.Seek(segmentLength - 2, SeekOrigin.Current);
            }
        }

        return null;
    }
}