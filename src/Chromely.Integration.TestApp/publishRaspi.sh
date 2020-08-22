#!/usr/bin/env bash
dotnet publish -c Release -f netcoreapp3.1 -r linux-arm -p:PublishSingleFile=true -p:PublishTrimmed=true
