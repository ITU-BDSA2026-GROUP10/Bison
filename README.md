# Bison

# This project is an app for Ornithology posts

# How to run the application
'cd src/Bison.CSVDBService'
'dotnet run'
then in a new terminal window:
'cd src/Bison.CLI'
'dotnet run -- read' 

'dotnet run -- observe "your observation" "your location"'

'dotnet run -- comment "your comment" "the observation id integer"'

'dotnet run -- location "the location"'

'dotnet run -- proposal "taxon id" "observation id"'

'dotnet run -- discussion "observation id"'

'dotnet run -- proposals "observation id"'

# How to run tests
'cd src/Bison.CSVDBService'
'dotnet run'
then in a new terminal window:
'cd test/Bison.CLI.tests'
'dotnet test'
'cd test/SimpleDB.tests'
'dotnet test'