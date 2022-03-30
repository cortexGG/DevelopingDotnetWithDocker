#!/bin/bash
set -e

if [ "$1" = '/opt/mssql/bin/sqlservr' ]; then

  # If this is the container's first run, initialise the application database
  if [ ! -f /tmp/app-initialised ]; then

    # Initialise the application database asynchronously in a background process. 
    # This allows 
    #     a) the SQL Server process to be the main process in the container, which allows graceful shutdown and other goodies, and 
    #     b) us to only start the SQL Server process once, as opposed to starting, stopping, then starting it again.
    function initialise_app_database() {

      # Wait a bit for SQL Server to start. SQL Server's process doesn't provide a clever way to check if it's up or not,
      # and it needs to be up before we can import the application database
      sleep 15s

      #run the setup script to create the DB and the schema in the DB
      /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P !SomethingSecret123 -d master -i 01_create-src-db.sql
      /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P !SomethingSecret123 -d bike-store-db -i ./ddl/02_create-bike-store-db.sql
      /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P !SomethingSecret123 -d bike-store-db -i ./data/03_data-bike-store-db.sql

      echo "Tell that aardvark it's a wrap."
      # Note that the container has been initialised so future starts won't wipe changes to the data
      touch /tmp/app-initialised
    }

    initialise_app_database &

  fi

fi

exec "$@"