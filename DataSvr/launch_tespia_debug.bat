@echo off
cd ..\BinSvr
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Center.exe' -ArgumentList 'DebugTespiaCenter'"
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Game.exe' -ArgumentList 'DebugTespiaGame0'"
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Shop.exe' -ArgumentList 'DebugTespiaShop'"
