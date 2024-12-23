@echo off
rmdir /s /q patches\
mkdir patches\
copy patch.base patches\
copy ..\ClientBin\Patcher.exe patches\NewPatcher.dat
..\WvsBeta.PatchCreator\bin\Release\WvsBeta.PatchCreator.exe make-patches
