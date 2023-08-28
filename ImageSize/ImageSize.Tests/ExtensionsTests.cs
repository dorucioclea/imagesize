using Xunit;

namespace ImageSize.Tests;

public class ExtensionsTests
{
    [Fact]
    public void TestReadInt16BigEndian()
    {
        var data = new byte[] { 0x12, 0x34 };
        using (var stream = new MemoryStream(data))
        using (var reader = new BinaryReader(stream))
        {
            short result = reader.ReadInt16BigEndian();
            Assert.Equal(0x1234, result);
        }
    }

    [Fact]
    public void TestReadInt32BigEndian()
    {
        var data = new byte[] { 0x12, 0x34, 0x56, 0x78 };
        using (var stream = new MemoryStream(data))
        using (var reader = new BinaryReader(stream))
        {
            int result = reader.ReadInt32BigEndian();
            Assert.Equal(0x12345678, result);
        }
    }

    [Fact]
    public void TestReadInt16LittleEndian()
    {
        var data = new byte[] { 0x34, 0x12 };
        using (var stream = new MemoryStream(data))
        using (var reader = new BinaryReader(stream))
        {
            short result = reader.ReadInt16LittleEndian();
            Assert.Equal(0x1234, result);
        }
    }

    [Fact]
    public void TestReadInt32LittleEndian()
    {
        var data = new byte[] { 0x78, 0x56, 0x34, 0x12 };
        using (var stream = new MemoryStream(data))
        using (var reader = new BinaryReader(stream))
        {
            int result = reader.ReadInt32LittleEndian();
            Assert.Equal(0x12345678, result);
        }
    }
}