@echo off
dotnet build wz2nx\WZ2NX.sln -c Debug
dotnet build wz2nx\WZ2NX.sln -c Release
dotnet build WvsBeta.NXWriter\WvsBeta.NXWriter.csproj -c Release
if errorlevel 1 exit

cd ClientBin
echo:
echo Applying patches to Data.wz...
python DataWZ_ApplyPatches.py
if errorlevel 1 exit

cd ..\DataSvr

echo:
echo Building ClientData.nx...
..\wz2nx\bin\Release\WZ2NX /in:..\ClientBin\Data.wz /out:ClientData.nx /wzv:Classic /wzn
echo:
echo Removing exports directory...
rmdir /S /Q NxExports\ClientData.nx
echo Exporting...
..\WvsBeta.NXWriter\bin\Release\WvsBeta.NXWriter.exe ClientData.nx NxExports true
if errorlevel 1 exit

echo:
echo Building ServerData.nx...
..\wz2nx\bin\Release\WZ2NX /in:ServerData.wz /out:ServerData.nx /wzv:Classic /wzn /sortnodes
echo:
echo Removing exports directory...
rmdir /S /Q NxExports\ServerData.nx
echo Exporting...
..\WvsBeta.NXWriter\bin\Release\WvsBeta.NXWriter.exe ServerData.nx NxExports true
if errorlevel 1 exit

pause
