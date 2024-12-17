@echo off
rem Usage: DataWZ_GeneratePatch.bat old-in new-in patch-out
..\Patching\courgette64.exe -gen %1 %2 %3
