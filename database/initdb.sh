#!/bin/sh

psql -U $POSTGRES_USER -d $POSTGRES_DB \
        -f /src/schema.sql && \
psql -U $POSTGRES_USER -d $POSTGRES_DB \
        -f /src/area_public_EFMigrationsHistory.sql \
        -f /src/area_public_Services.sql \
        -f /src/area_public_Widgets.sql \
        -f /src/area_public_Enums.sql \
        -f /src/area_public_Params.sql && \
psql -U $POSTGRES_USER -d $POSTGRES_DB \
        -f /src/area_public_ParamsToEnums.sql \
        -f /src/area_public_EnumHasValues.sql
