echo Copy files ...
copy DbNotifyer\bin\Release\Hardcodet.Wpf.TaskbarNotification DbFactory\Bin\Release
copy SQLiteLibrary\bin\Release\System.Data.SQLite.dll DbFactory\bin\Release
copy SQLiteLibrary\SQLite.Interop.dll DbFactory\bin\Release
copy MySQLLibrary\bin\Release\MySql.Data.dll DbFactory\bin\Release
copy MySQLLibrary\bin\Release\MySql.Data.Entity.EF5.dll DbFactory\bin\Release
copy MySQLLibrary\bin\Release\MySql.Data.Entity.EF6.dll DbFactory\bin\Release
copy MySQLLibrary\bin\Release\MySql.Fabric.Plugin.dll DbFactory\bin\Release


echo Building Project ...
"C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe" DbInterface\DbInterface.csproj /p:Configuration=Debug;OutputPath=..\Binaries /t:Rebuild
"C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe" DbFactory\DbFactory.csproj /p:Configuration=Debug;OutputPath=..\Binaries /t:Rebuild


echo Cleanup Release folder ...
del Release\*.*  /s /q

echo Copy new dll files ...
copy DbFactory\bin\Release\*.dll Binaries
copy DbFactory\bin\Release\*.pdb Binaries

copy Binaries\Hardcodet.Wpf.TaskbarNotification.dll Release
copy Binaries\System.Data.SQLite.dll Release
copy SQLiteLibrary\SQLite.Interop.dll Release
copy Binaries\MySql.Data.dll Release


echo Merging Binaries ...
"C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe" /ndebug /copyattrs /targetplatform:4.0,"C:\Windows\Microsoft.NET\Framework64\v4.0.30319" /out:Release\TPDev.DatabaseFactory.dll Binaries\DatabaseFactory.dll Binaries\DbInterface.dll Binaries\DbLogger.dll Binaries\DbNotifyer.dll Binaries\MySQLLibrary.dll Binaries\SQLiteLibrary.dll Binaries\SQLLibrary.dll Binaries\OracleLibrary.dll

echo finished!
pause