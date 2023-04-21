dotnet publish -c Release --self-contained -r linux-x64 -o publish-linux-x64
dotnet publish -c Release --self-contained -r linux-musl-x64 -o publish-linux-musl-x64
dotnet publish -c Release --self-contained -r win-x64 -o publish-win-x64
dotnet publish -c Release --self-contained -r win-x86 -o publish-win-x86

mkdir -p publish
cd publish

tar --lzma -cvf linux-x64.tar.gz      ../publish-linux-x64
tar --lzma -cvf linux-musl-x64.tar.gz ../publish-linux-musl-x64
tar --lzma -cvf win-x64.tar.gz        ../publish-win-x64
tar --lzma -cvf win-x86.tar.gz        ../publish-win-x86

cd ..

rm -rf publish-linux-x64
rm -rf publish-linux-musl-x64
rm -rf publish-win-x64
rm -rf publish-win-x86