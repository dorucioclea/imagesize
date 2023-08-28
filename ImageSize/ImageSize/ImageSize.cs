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

        if (header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47)
        {
            binaryReader.BaseStream.Seek(16, SeekOrigin.Begin);
            var width = binaryReader.ReadInt32BigEndian();
            var height = binaryReader.ReadInt32BigEndian();
            return (width, height);
        }

        // Check for GIF magic numbers
        if (header[0] == 0x47 && header[1] == 0x49 && header[2] == 0x46)
        {
            binaryReader.BaseStream.Seek(6, SeekOrigin.Begin);
            int width = binaryReader.ReadInt16LittleEndian();
            int height = binaryReader.ReadInt16LittleEndian();
            return (width, height);
        }

        // Check for BMP magic numbers
        if (header[0] == 0x42 && header[1] == 0x4D)
        {
            binaryReader.BaseStream.Seek(18, SeekOrigin.Begin);
            var width = binaryReader.ReadInt32LittleEndian();
            var height = binaryReader.ReadInt32LittleEndian();
            return (width, height);
        }

        // Check for TIFF magic numbers
        if (header.Take(2).SequenceEqual("II"u8.ToArray()) ||
            header.Take(2).SequenceEqual("MM"u8.ToArray())) // "II" or "MM"
        {
            binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
            return GetTiffSize(binaryReader);
        }

        return null;
    }

   private static (int width, int height)? GetTiffSize(BinaryReader reader)
   {
       // Read byte order marks
       var byteOrderBytes = reader.ReadBytes(2);

       bool isLittleEndian;
       if (byteOrderBytes[0] == 0x49 && byteOrderBytes[1] == 0x49) // II for Intel order (Little-Endian)
       {
           isLittleEndian = true;
       }
       else if (byteOrderBytes[0] == 0x4D && byteOrderBytes[1] == 0x4D) // MM for Motorola order (Big-Endian)
       {
           isLittleEndian = false;
       }
       else
       {
           throw new ImageFormatException("Not a valid TIFF file"); // Not a valid TIFF file
       }

       // Read the TIFF magic number (should be 42)
       var magicNumber = isLittleEndian ? reader.ReadUInt16() : reader.ReadUInt16BigEndian();
       if (magicNumber != 42)
       {
           return null; // Not a valid TIFF file
       }

       // Read the offset to the first IFD
       var ifdOffset = isLittleEndian ? reader.ReadUInt32() : reader.ReadUInt32BigEndian();

       if (ifdOffset > reader.BaseStream.Length - 2)
       {
           return null; // Not a valid IFD offset
       }

       reader.BaseStream.Seek(ifdOffset, SeekOrigin.Begin);

       var entryCount = isLittleEndian ? reader.ReadUInt16() : reader.ReadUInt16BigEndian();

       int width = 0, height = 0;

       for (int i = 0; i < entryCount; i++)
       {
           var tag = isLittleEndian ? reader.ReadUInt16() : reader.ReadUInt16BigEndian();
           reader.BaseStream.Seek(2, SeekOrigin.Current); // Skip the type, we assume short/int for width/height
           var valueOrOffset = isLittleEndian ? reader.ReadUInt32() : reader.ReadUInt32BigEndian();

           if (tag == 256) // ImageWidth tag
           {
               width = (int)valueOrOffset;
           }
           else if (tag == 257) // ImageHeight tag
           {
               height = (int)valueOrOffset;
           }

           if (width != 0 && height != 0)
               return (width, height);
       }

       return (width, height); // Returns width and height even if one of them is zero
   }

    private static (int width, int height)? GetJpegSize(BinaryReader binaryReader)
    {
        while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
        {
            byte marker = binaryReader.ReadByte();

            if (marker != 0xFF)
                continue;

            byte segmentType = binaryReader.ReadByte();

            if (segmentType is >= 0xC0 and <= 0xCF && segmentType != 0xC4 && segmentType != 0xC8 &&
                segmentType != 0xCC)
            {
                binaryReader.ReadByte(); // Skip length high byte
                binaryReader.ReadByte(); // Skip length low byte
                binaryReader.ReadByte(); // Skip bits/sample
                int height = binaryReader.ReadInt16BigEndian();
                int width = binaryReader.ReadInt16BigEndian();
                return (width, height);
            }

            int segmentLength = binaryReader.ReadInt16BigEndian();
            binaryReader.BaseStream.Seek(segmentLength - 2, SeekOrigin.Current);
        }

        return null;
    }
}