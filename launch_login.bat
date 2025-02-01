@echo off
cd BinSvr\Release
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Login.exe' -ArgumentList 'Login'"
