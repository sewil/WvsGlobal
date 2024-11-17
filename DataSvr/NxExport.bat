@echo off
rmdir /S /Q NxExports\%~nx1
..\WvsBeta.NXWriter\bin\Release\WvsBeta.NXWriter.exe %~nx1 NxExports true
