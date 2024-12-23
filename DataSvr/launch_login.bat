@echo off
cd ..\BinSvr
powershell -WindowStyle Hidden -Command "Start-Process 'WvsBeta.Login.exe' -ArgumentList 'Login'"
