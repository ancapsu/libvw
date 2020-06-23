FROM mcr.microsoft.com/mssql/server:2017-CU20-ubuntu-16.04

ARG backup_filename=a_v001_Lksj88s7jwjshanx
ARG backup_dataname=a_a001_visao

ENV BACKUP_FILENAME ${backup_filename}
ENV BACKUP_DATANAME ${backup_dataname}

## Change image initial command
CMD /bin/bash ./entrypoint.sh

## Copy scripts folder to container root folder
COPY Docker/scripts /

## Copy database backup file
COPY Resources/${backup_filename}.zip /

## Ensure that script file is executable
RUN chmod +x /mssql-init.sh

## Install extra packages
RUN apt-get update; \
    apt-get install netcat unzip -y

## Unzip backup file
RUN unzip /${backup_filename}.zip
