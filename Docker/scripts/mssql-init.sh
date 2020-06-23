mssql_ready="nc -z localhost 1433"

while ! $mssql_ready
do
  echo 'Waiting for SQL Server. Retrying in 5 seconds.'
  sleep 5
done

echo 'Running SQL Server initialization script.'

## Run inital sql script
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i mssql-setup.sql

echo 'SQL Server initialization finished.'
