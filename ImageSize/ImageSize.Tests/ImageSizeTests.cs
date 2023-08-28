using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace ImageSize.Tests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class ImageSizeTests
{
    [Fact]
    public void TestJPEGSize()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "200x300.jpg");
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        var size = ImageSize.GetImageSize(fileStream);
        
        Assert.NotNull(size);
        Assert.Equal(200, size.Value.width);
        Assert.Equal(300, size.Value.height);
    }
    
    [Fact]
    public void TestBMPSize()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "200x300.bmp");
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        var size = ImageSize.GetImageSize(fileStream);
        
        Assert.NotNull(size);
        Assert.Equal(200, size.Value.width);
        Assert.Equal(300, size.Value.height);
    }
    
    [Fact]
    public void TestPNGSize()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "200x300.png");
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        var size = ImageSize.GetImageSize(fileStream);
        
        Assert.NotNull(size);
        Assert.Equal(200, size.Value.width);
        Assert.Equal(300, size.Value.height);
    }
    
    [Fact]
    public void TestGIFSize()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "200x300.gif");
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        var size = ImageSize.GetImageSize(fileStream);
        
        Assert.NotNull(size);
        Assert.Equal(200, size.Value.width);
        Assert.Equal(300, size.Value.height);
    }
    
    [Fact]
    public void TestTIFFSize()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Free_Test_Data_1.1MB_TIFF.tif");
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        var size = ImageSize.GetImageSize(fileStream);
        
        Assert.NotNull(size);
        Assert.Equal(200, size.Value.width);
        Assert.Equal(300, size.Value.height);
    }
}