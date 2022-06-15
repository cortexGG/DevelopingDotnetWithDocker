#!/bin/bash

docker stop devenv_MyDevSqlServer_1
docker container rm -f devenv_MyDevSqlServer_1
docker image rm -f mydevsqlserver:1.0.1
docker volume rm devenv_mssqldata

docker stop devenv_Scheduler_1
docker container rm -f devenv_Scheduler_1
docker image rm -f scheduler:1.0.1
