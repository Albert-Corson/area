create table if not exists users
(
    user_id       serial  not null
        constraint user_pk
            primary key,
    name          varchar not null,
    password      text    not null,
    email         varchar,
    access_token  text,
    refresh_token text,
    expire_date   timestamp default now()
);

comment on table users is 'users information';

alter table users
    owner to postgres;

create unique index if not exists user_name_uindex
    on users (name);

create table if not exists services
(
    service_id    serial not null
        constraint service_pk
            primary key,
    name          varchar,
    client_id     text,
    client_secret text
);

comment on table services is 'services information';

alter table services
    owner to postgres;

create table if not exists widgets
(
    widget_id  serial  not null
        constraint widget_pk
            primary key,
    service_id integer
        constraint fk_service
            references services
            on delete cascade,
    name       varchar not null
);

comment on table widgets is 'widgets information';

alter table widgets
    owner to postgres;

create table if not exists user_has_widget
(
    user_id   integer
        constraint fk_user
            references users
            on delete cascade,
    widget_id integer
        constraint fk_widget
            references widgets
            on delete cascade
);

alter table user_has_widget
    owner to postgres;

create table if not exists user_has_service
(
    user_id    integer
        constraint fk_user
            references users
            on delete cascade,
    service_id integer
        constraint fk_service
            references services
            on delete cascade
);

alter table user_has_service
    owner to postgres;

create table if not exists user_service_credentials
(
    user_id       integer
        constraint fk_user
            references users
            on delete cascade,
    service_id    integer
        constraint fk_service
            references services
            on delete cascade,
    access_token  text,
    refresh_token text
);

comment on table user_service_credentials is 'user credentials (access_token and refresh_token) for a specific service';

alter table user_service_credentials
    owner to postgres;


