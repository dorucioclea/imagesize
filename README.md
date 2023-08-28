# Image Dimension Detector
An easy-to-use C# library that determines the dimensions (width and height) of images in various formats, without relying on external libraries.

## Supported Formats
* PNG
* JPEG
* BMP
* GIF
* TIFF


## Installation

Via NuGet
Copy code

```bash
Install-Package ImageSize
```

Or search for ImageDimensionDetector in the NuGet Package Manager.

## Manual Installation

Download the latest release and add the .dll file to your C# project.

## Usage

```csharp
using ImageDimensionDetector;

...

Stream imageStream = File.OpenRead("path_to_your_image_file");
var dimensions = ImageDimensionDetector.GetImageSize(imageStream);

if(dimensions.HasValue)
{
    Console.WriteLine($"Width: {dimensions.Value.width}, Height: {dimensions.Value.height}");
}
else
{
    Console.WriteLine("Unsupported image format or invalid image file.");
}
```

## Contributing
Fork the repository.
Create your feature branch (git checkout -b feature/YourFeatureName).
Commit your changes (git commit -am 'Add some feature').
Push to the branch (git push origin feature/YourFeatureName).

Open a pull request.

## Issues
If you discover a bug or have a suggestion, please open an issue.

## License
This project is licensed under the MIT License. See the LICENSE file for details.

