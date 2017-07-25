for /r . %%a in (*.nupkg) do (
	del "%%a"
)
dotnet pack -c Release -o .\
for /r . %%a in (*.nupkg) do (
	nuget push "%%a" -source https://www.nuget.org/api/v2/package -apikey %1%
)
