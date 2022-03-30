FROM mcr.microsoft.com/mssql/server:2019-CU15-ubuntu-20.04

# Mandatory environment variables needed by the SqlServer image above
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=!SomethingSecret123

# Copy all of the Database creation scripts into the image and then run the script that makes it happen
COPY ./01_Create/ /
ENTRYPOINT [ "/bin/bash", "InitialiseIfNotCreated.sh" ]

CMD [ "/opt/mssql/bin/sqlservr" ] 