# Client configuration
## Building ijl15.dll
1. Open OpenMGClient\MapleClientCollection.sln
2. Set CTemplate as startup project, set configuration to Release mode.
3. Build. ijl15.dll will be generated in ClientBin.

## Signing and packing
Signing:
1. Install the Windows SDK which contains signtool: https://developer.microsoft.com/sv-se/windows/downloads/windows-sdk/ (Don't forget to add signtool to PATH!)
3. Install OpenSSL: https://slproweb.com/products/Win32OpenSSL.html
4. In ClientBin, run `MapleStoryGenerateCertificate.bat`, followed by `MapleStoryGeneratePFX.bat`, and then `MapleStorySign.bat`.

Packing:
1. Install mpress: https://github.com/gerardbalaoro/MpressGUI
2. Run `MapleStoryPack.bat`.

You may verify the client's signature and packing status with Detect-It-Easy.

## Installer
### Sign tool configuration
1. Install InnoSetup: https://jrsoftware.org/isinfo.php
2. In ClientBin, copy Setup.iss.example to Setup.iss, and open the file with InnoSetup.
3. Go to Tools->Configure Sign Tools, click on add and enter a name. As the command, enter:
```
signtool sign /f <path to your .pfx file> /fd sha512 /p <password> $f
```
4. Make any remaining changes to the installer file as you see fit, and press run to generate it.

## Patching Data.wz
With the original v22 installed in `ClientBin\`, run `python DataWZ_ApplyPatches.py`, and it will apply the patches located in `ClientBin\Patches\` using [Courgette](https://www.chromium.org/developers/design-documents/software-updates-courgette/).
