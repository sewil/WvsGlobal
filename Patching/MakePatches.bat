@echo off
copy ..\ClientBin\Patcher.exe patches\NewPatcher.dat
..\WvsBeta.PatchCreator\bin\Debug\WvsBeta.PatchCreator.exe make-patches
