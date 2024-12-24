@echo off
rem Usage: DataWZ_GeneratePatch.bat old-in new-in patch-out
set /p oldIn=Path in to old Data.wz: 
set /p newIn=Path in to new Data.wz: 
set /p patchOut=Path out to .patch file: 
..\Patching\courgette64.exe -gen %oldIn% %newIn% %patchOut%
pause
