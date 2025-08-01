# MinimalApiExample.PostgreSQL
Проект в стадии работы, ReadMe будет позже...

для работы нужно, чтобы в PgAdmin были созданы таблицы (user 1-N blogs)
```
CREATE TABLE IF NOT EXISTS public.users
(
    id integer NOT NULL DEFAULT nextval('users_id_seq'::regclass),
    "firstName" character varying(30) COLLATE pg_catalog."default" NOT NULL,
    "lastName" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "dateOfBirth" date NOT NULL,
    CONSTRAINT users_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.users
    OWNER to postgres;
```
```
CREATE TABLE public.blogs
(
    id serial NOT NULL,
    title character varying(30) NOT NULL,
    created date NOT NULL,
    context text NOT NULL,
    user_id bigint NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (user_id)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
);

ALTER TABLE IF EXISTS public.blogs
    OWNER to postgres;
```
имя базы TestDb, пароль 251187 (или любой другой, тогда в файле Program надо поправить строку подключения)
