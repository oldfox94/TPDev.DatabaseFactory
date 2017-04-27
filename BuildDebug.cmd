echo Copy files ...
copy DbNotifyer\bin\Release\Hardcodet.Wpf.TaskbarNotification DbFactory\Bin\Debug
copy SQLiteLibrary\bin\Release\System.Data.SQLite.dll DbFactory\bin\Debug
copy SQLiteLibrary\SQLite.Interop.dll DbFactory\bin\Debug
copy MySQLLibrary\bin\Release\MySql.Data.dll DbFactory\bin\Debug
copy MySQLLibrary\bin\Release\MySql.Data.Entity.EF5.dll DbFactory\bin\Debug
copy MySQLLibrary\bin\Release\MySql.Data.Entity.EF6.dll DbFactory\bin\Debug
copy MySQLLibrary\bin\Release\MySql.Fabric.Plugin.dll DbFactory\bin\Debug


echo Building Project ...
"C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe" DatabaseFactory.sln /p:Configuration=Debug;OutputPath=..\Binaries /t:Rebuild


echo Cleanup Debug folder ...
del _Debug\*.*  /s /q

echo Copy new dll files ...
copy DbFactory\bin\Debug\*.dll Binaries
copy DbFactory\bin\Debug\*.pdb Binaries

copy Binaries\Hardcodet.Wpf.TaskbarNotification.dll _Debug
copy Binaries\System.Data.SQLite.dll _Debug
copy SQLiteLibrary\SQLite.Interop.dll _Debug
copy Binaries\MySql.Data.dll _Debug


echo Merging Binaries ...
"C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe" /ndebug /copyattrs /targetplatform:4.0,"C:\Windows\Microsoft.NET\Framework64\v4.0.30319" /out:_Debug\TPDev.DatabaseFactory.dll Binaries\DatabaseFactory.dll Binaries\DbInterface.dll Binaries\DbLogger.dll Binaries\DbNotifyer.dll Binaries\MySQLLibrary.dll Binaries\SQLiteLibrary.dll Binaries\SQLLibrary.dll Binaries\OracleLibrary.dll

echo finished!
pause