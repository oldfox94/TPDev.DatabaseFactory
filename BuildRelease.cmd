echo Building Project ...
"C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe" DbInterface\DbInterface.csproj /p:Configuration=Release;TargetFrameworkVersion=v4.5;TargetFrameworkProfile="";OutputPath=..\Binaries /t:Rebuild

"C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe" DbFactory\DbFactory.csproj /p:Configuration=Release;TargetFrameworkVersion=v4.5;TargetFrameworkProfile="";OutputPath=..\Binaries /t:Rebuild


echo Merging Binaries ...
"C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe" /ndebug /copyattrs /targetplatform:4.0,"C:\Windows\Microsoft.NET\Framework64\v4.0.30319" /out:Release\TPDev.DatabaseFactory.dll Binaries\DatabaseFactory.dll Binaries\DbInterface.dll Binaries\DbLogger.dll Binaries\MySQLLibrary.dll Binaries\SQLiteLibrary.dll Binaries\SQLLibrary.dll

echo Cleanup Release folder ...
del Release\*.*  /s /q
mkdir  Release

echo Copy new dll files ...
copy SQLiteLibrary\SQLite.Interop.dll Release
copy Binaries\MySql.Data.dll Release

echo finished!
pause