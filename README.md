

# ChromeStatus - Rainmeter Plugin

**ChromeStatus** is a **Rainmeter** plugin that checks whether **Google Chrome** is installed on your system. It looks for Chrome's installation by checking both the Windows registry and common installation file paths. This plugin provides an easy way to display the installation status of Google Chrome in your Rainmeter skins.

## Features

- **Checks if Google Chrome is installed** using both the Windows registry and common file system paths.
- Returns `1` if Chrome is installed and `0` if it is not installed.
- Easy integration into Rainmeter skins with a simple measure.

## Installation

1. **Download the latest release** of **ChromeStatus** from the [Releases page](https://github.com/NSTechBytes/ChromeStatus/releases).
2. **Install the plugin** by copying `ChromeStatus.dll` into your Rainmeter `Plugins` directory:
   - The default path is:  
     `C:\Users\<YourUsername>\Documents\Rainmeter\Plugins\`

3. After installation, you can use the plugin in your Rainmeter skins.

## Usage

### 1. Create a Measure in your Rainmeter skin

In your `.ini` skin file, define a measure that uses the **ChromeStatus** plugin.

```ini
[Rainmeter]
Update=1000
BackgroundMode=2
SolidColor=FFFFFF

[mChromeCheck]
Measure=Plugin
Plugin=ChromeStatus.dll
Type=String

[Text1]
Meter=STRING
MeasureName=mChromeCheck
X=10
Y=35
W=200
H=70
FontColor=000000
Antialias=1
FontSize=12
FontFace=Arial
stringAlign = LeftCenter
Text="Chrome Installed: %1#CRLF#"

```

In this example:
- `%1` will display `1` if Chrome is installed, or `0` if it is not.


## Parameters

- **None**: This plugin doesn't require any additional parameters in the `.ini` skin file.

## Troubleshooting

- **Chrome Not Found**: If the plugin returns `0` and you are sure Chrome is installed, ensure the installation paths and registry entries are correct.
- **File Not Found**: The plugin checks the default installation paths for Chrome. If Chrome is installed in a custom directory, the plugin might not find it.

## Contributing

If you'd like to contribute to this project, feel free to fork the repository and submit a pull request. Please follow the existing style and include tests where applicable.

### Bug Reports & Feature Requests

If you encounter bugs or want to suggest features, please use the [Issues tab](https://github.com/NSTechBytes/ChromeStatus/issues).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

