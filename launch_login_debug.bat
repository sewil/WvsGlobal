@echo off
cd BinSvr\Debug
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Login.exe' -ArgumentList 'DebugLogin'"
