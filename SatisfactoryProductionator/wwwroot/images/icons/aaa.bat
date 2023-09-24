@echo off
for %%f in (IconDesc_*_256.png) do (
    set "filename=%%~nf"
    setlocal enabledelayedexpansion
    ren "%%f" "!filename:IconDesc_=!.png"
    endlocal
)
for %%f in (*_256.png) do (
    set "filename=%%~nf"
    setlocal enabledelayedexpansion
    ren "%%f" "!filename:_256=!.png"
    endlocal
)
pause