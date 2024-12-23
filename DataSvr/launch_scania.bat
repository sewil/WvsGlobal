@echo off
cd ..\BinSvr
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Center.exe' -ArgumentList 'ScaniaCenter'"
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Game.exe' -ArgumentList 'ScaniaGame0'"
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Shop.exe' -ArgumentList 'ScaniaShop'"
