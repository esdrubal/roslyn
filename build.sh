find . -name "obj" | xargs rm -fdr
mkdir -p Src/packages/Microsoft.Net.ToolsetCompilers.0.7.4032713-beta/build
echo "<Project DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">
  <PropertyGroup>
    <DisableRoslyn>true</DisableRoslyn>
    <CscToolPath Condition=\" '$(OS)' == 'Windows_NT'\">$(MSBuildThisFileDirectory)..\tools</CscToolPath>
    <CscToolExe Condition=\" '$(OS)' == 'Windows_NT'\">csc2.exe</CscToolExe>
    <VbcToolPath>$(MSBuildThisFileDirectory)..\tools</VbcToolPath>
    <VbcToolExe>vbc2.exe</VbcToolExe>
  </PropertyGroup>
</Project>" >packages/Microsoft.Net.ToolsetCompilers.0.7.4100302-beta/build/Microsoft.Net.ToolsetCompilers.props
xbuild /p:Configuration=Debug Src/Compilers/CSharp/csc/csc.csproj
xbuild /p:Configuration=Debug ./Src/Workspaces/CSharp/Desktop/CSharpWorkspace.Desktop.csproj