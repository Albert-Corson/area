#!/bin/sh

psql -U $POSTGRES_USER -d $POSTGRES_DB -f /src/schema.sql && \
psql -U $POSTGRES_USER -d $POSTGRES_DB \
        -f /src/dashboard_public_EFMigrationsHistory.sql \
        -f /src/dashboard_public_Services.sql \
        -f /src/dashboard_public_WidgetHasParams.sql \
        -f /src/dashboard_public_Widgets.sql
