#!/bin/bash

# bash function for running sed on a file
# $1 = file to run sed on
function includeTxt() {
    sed "/<\/PropertyGroup>/a  <ItemGroup><Content Include=\"*.txt\" CopyToOutputDirectory=\"PreserveNewest\" /></ItemGroup>" -i $1
}

function createProject() {
    project_name=$1

    dotnet new console -n $project_name
    includeTxt $project_name/$project_name.csproj

    dotnet new xunit -n $project_name.test
    includeTxt $project_name.test/$project_name.test.csproj

    dotnet add $project_name.test/$project_name.test.csproj reference $project_name/$project_name.csproj

    dotnet sln add $project_name/$project_name.csproj
    dotnet sln add $project_name.test/$project_name.test.csproj
}

mkdir $1
cd $1
dotnet new sln -n $1

createProject part1
createProject part2

dotnet add part2/part2.csproj reference part1/part1.csproj
