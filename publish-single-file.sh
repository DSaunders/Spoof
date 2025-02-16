cd src/Runner/
dotnet publish -c Release 
cd ../../_publish
find . -type f ! -name '*.exe' ! -name '*.route.json' -delete
rm -rf _scaffold
cd ..