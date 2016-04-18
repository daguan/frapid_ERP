set directory=%~1%~2
set file=%~3
set path=%directory%%file%



if exist %path% (
	cd %directory%
	%file%
) else (
	echo Warning: %path% was not found
	echo.
)