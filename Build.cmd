"C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe" DbInterface\DbInterface.csproj /p:Configuration=Release;TargetFrameworkVersion=v4.5;TargetFrameworkProfile="";OutputPath=..\Binaries /t:Rebuild

"C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe" DbFactory\DbFactory.csproj /p:Configuration=Release;TargetFrameworkVersion=v4.5;TargetFrameworkProfile="";OutputPath=..\Binaries /t:Rebuild

pause

start Merge.cmd