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
}