@echo off
cd ..\BinSvr
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Center.exe' -ArgumentList 'TespiaCenter'"
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Game.exe' -ArgumentList 'TespiaGame0'"
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Shop.exe' -ArgumentList 'TespiaShop'"
