@echo off
rmdir /s /q patches\
mkdir patches\
copy patch.base patches\
..\WvsBeta.PatchCreator\bin\Release\WvsBeta.PatchCreator.exe make-patches
