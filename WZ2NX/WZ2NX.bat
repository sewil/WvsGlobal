@echo off
for %%x in (%*) do (
  WZ2NX /in:%%x /out:%%~dpnx.nx /wzv:Classic /wzn
)
pause
