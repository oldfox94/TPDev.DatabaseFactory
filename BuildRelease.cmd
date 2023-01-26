echo Cleanup Binaries folder ...
del Binaries\*.*  /s /q

echo Building Project ...
"C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe" DatabaseFactory.sln /p:Configuration=Release /t:Rebuild


echo Cleanup Release folder ...
del _Release\*.*  /s /q

echo Copy new dll files ...
copy _External\*.dll _Release
copy DbFactory\bin\Release\*.dll Binaries


echo Merging Binaries ...
"C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe" /ndebug /copyattrs /targetplatform:4.0,"C:\Windows\Microsoft.NET\Framework64\v4.0.30319" /out:_Release\TPDev.DatabaseFactory.dll Binaries\DatabaseFactory.dll Binaries\DbInterface.dll Binaries\DbLogger.dll Binaries\DbNotifyer.dll Binaries\MySQLLibrary.dll Binaries\SQLiteLibrary.dll Binaries\SQLLibrary.dll Binaries\OracleLibrary.dll

echo finished!
pause