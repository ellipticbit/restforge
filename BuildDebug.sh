#Update NETPath
git pull origin master

#Update Submodules
git submodule init
git submodule update

#Build System.Utilities
cd System.Utilities
"/C/Program Files (x86)/MSBuild/14.0/Bin/MSBuild.exe" ./WinRT8/WinRT8.csproj -nologo -p:Configuration=Debug -p:Platform=AnyCPU -t:Build
"/C/Program Files (x86)/MSBuild/14.0/Bin/MSBuild.exe" ./NET45/NET45.csproj -nologo -p:Configuration=Debug -p:Platform=AnyCPU -t:Build
cd ..

cd EllipticBit.Controls
sh BuildVS14.sh
cd ..

#Build NETPath
#"/C/Program Files (x86)/MSBuild/14.0/Bin/MSBuild.exe" ./RestForge.sln -nologo -t:Build