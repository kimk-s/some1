--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1 (Debian 16.1-1.pgdg120+1)
-- Dumped by pg_dump version 16.0

-- Started on 2024-04-20 16:11:49

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 238 (class 1255 OID 65541)
-- Name: buy_wait_product(character varying, character varying, character varying, timestamp without time zone, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.buy_wait_product(p_user_id character varying, p_product_id character varying, p_order_id character varying, p_purchase_data timestamp without time zone, p_premium_days integer) RETURNS integer
    LANGUAGE plpgsql
    AS $_$
DECLARE
  v_cnt integer;
  v_premium_accumulated_days integer;
  v_premium_expiration_date timestamp;
  v_utc_now timestamp DEFAULT timezone('utc'::text, now());
BEGIN
	
	INSERT INTO purchases(order_id, product_id, user_id)
	VALUES ($3, $2, $1)
	ON CONFLICT (order_id) DO NOTHING;

	GET DIAGNOSTICS v_cnt = ROW_COUNT;
	IF (v_cnt = 0) THEN
		RETURN 1;
	END IF;
	
	UPDATE users
	SET premium_accumulated_days = premium_accumulated_days + $5,
		premium_expiration_date = CASE WHEN premium_expiration_date IS NULL OR premium_expiration_date < v_utc_now THEN v_utc_now ELSE premium_expiration_date END + make_interval(days => $5)
	WHERE id = $1
	RETURNING premium_accumulated_days,
		premium_expiration_date
	INTO v_premium_accumulated_days,
		v_premium_expiration_date;
	
	GET DIAGNOSTICS v_cnt = ROW_COUNT;
	IF (v_cnt = 0) THEN
		RETURN 2;
	END IF;
	
	INSERT INTO premium_logs(
	 	user_id,
		reason,
		premium_changed_days,
		premium_accumulated_days,
		premium_expiration_date,
		purchase_order_id,
		purchase_product_id,
		purchase_date)
	VALUES(
		$1,
		1,
		$5,
		v_premium_accumulated_days,
		v_premium_expiration_date,
		$3,
		$2,
		$4);
	
	RETURN 0;
	
END;
$_$;


ALTER FUNCTION public.buy_wait_product(p_user_id character varying, p_product_id character varying, p_order_id character varying, p_purchase_data timestamp without time zone, p_premium_days integer) OWNER TO postgres;

--
-- TOC entry 225 (class 1255 OID 32793)
-- Name: get_play_play(character varying); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_play_play(id character varying) RETURNS TABLE(maintenance boolean)
    LANGUAGE sql
    AS $_$
	SELECT maintenance FROM plays WHERE id = $1;
$_$;


ALTER FUNCTION public.get_play_play(id character varying) OWNER TO postgres;

--
-- TOC entry 240 (class 1255 OID 98316)
-- Name: get_play_users(text, character varying); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_play_users(p_values text, p_play_id character varying) RETURNS TABLE(id character varying, play_id character varying, is_playing boolean, manager boolean, premium_expiration_date timestamp without time zone, packed_data jsonb)
    LANGUAGE plpgsql
    AS $_$
		
BEGIN
	RETURN QUERY EXECUTE 'WITH rows AS(
		INSERT INTO users(id, play_id, is_playing) VALUES ' || $1 ||
		' ON CONFLICT (id) DO UPDATE SET
			play_id = (CASE WHEN users.is_playing = false THEN ''' || $2 || ''' ELSE users.play_id END),
			is_playing = (CASE WHEN users.play_id = ''' || $2 || ''' OR users.is_playing = false THEN true ELSE users.is_playing END)
		RETURNING id, play_id, is_playing, manager, premium_expiration_date, packed_data)
		SELECT id, play_id, is_playing, manager, premium_expiration_date, packed_data FROM rows';
END
$_$;


ALTER FUNCTION public.get_play_users(p_values text, p_play_id character varying) OWNER TO postgres;

--
-- TOC entry 237 (class 1255 OID 73730)
-- Name: get_wait_plays(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_wait_plays() RETURNS TABLE(id character varying, region character varying, city character varying, number integer, address character varying, opening_soon boolean, maintenance boolean, busy real)
    LANGUAGE sql
    AS $$
	SELECT 
		id,
		region,
		city,
		number,
		address,
		opening_soon,
		maintenance,
		busy
	FROM plays;
$$;


ALTER FUNCTION public.get_wait_plays() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 218 (class 1259 OID 49156)
-- Name: premium_logs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.premium_logs (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    user_id character varying NOT NULL,
    reason integer NOT NULL,
    created_date timestamp without time zone DEFAULT timezone('utc'::text, now()) NOT NULL,
    premium_changed_days integer NOT NULL,
    premium_expiration_date timestamp without time zone NOT NULL,
    purchase_order_id character varying,
    purchase_product_id character varying,
    purchase_date timestamp without time zone,
    note character varying,
    premium_accumulated_days integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.premium_logs OWNER TO postgres;

--
-- TOC entry 220 (class 1255 OID 49172)
-- Name: get_wait_premium_logs(character varying); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_wait_premium_logs(user_id character varying) RETURNS SETOF public.premium_logs
    LANGUAGE sql
    AS $_$
	SELECT * FROM premium_logs WHERE user_id = $1;
$_$;


ALTER FUNCTION public.get_wait_premium_logs(user_id character varying) OWNER TO postgres;

--
-- TOC entry 222 (class 1255 OID 40985)
-- Name: get_wait_user(character varying); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_wait_user(id character varying) RETURNS TABLE(id character varying, play_id character varying, is_playing boolean, manager boolean)
    LANGUAGE sql
    AS $_$
	INSERT INTO users(id) VALUES($1)
	ON CONFLICT (id) DO NOTHING;
	
	SELECT id, play_id, is_playing, manager
	FROM users WHERE id = $1;	
$_$;


ALTER FUNCTION public.get_wait_user(id character varying) OWNER TO postgres;

--
-- TOC entry 224 (class 1255 OID 32790)
-- Name: get_wait_wait(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.get_wait_wait() RETURNS TABLE(maintenance boolean)
    LANGUAGE sql
    AS $$
	SELECT maintenance FROM waits WHERE id = 1;
$$;


ALTER FUNCTION public.get_wait_wait() OWNER TO postgres;

--
-- TOC entry 223 (class 1255 OID 32794)
-- Name: set_play_play(character varying, real); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.set_play_play(id character varying, busy real) RETURNS void
    LANGUAGE sql
    AS $_$
	UPDATE plays
	SET busy = $2,
		set_time = timezone('utc', now())
	WHERE id = $1;
$_$;


ALTER FUNCTION public.set_play_play(id character varying, busy real) OWNER TO postgres;

--
-- TOC entry 239 (class 1255 OID 98308)
-- Name: set_play_users(text[]); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.set_play_users(insert_values text[]) RETURNS TABLE(id character varying, premium_expiration_date timestamp without time zone)
    LANGUAGE plpgsql
    AS $_$
DECLARE
	v_utc_now timestamp DEFAULT timezone('utc'::text, now());
BEGIN
	RETURN QUERY EXECUTE 'WITH rows AS(
		INSERT INTO users(id, play_id, is_playing, packed_data)
		VALUES ' || array_to_string($1, ',') ||
		' ON CONFLICT (id) DO UPDATE SET
			play_id = (CASE WHEN users.is_playing = false THEN EXCLUDED.play_id ELSE users.play_id END),
			is_playing = (CASE WHEN users.play_id = EXCLUDED.play_id OR users.is_playing = false THEN EXCLUDED.is_playing ELSE users.is_playing END),
			packed_data = (CASE WHEN users.play_id = EXCLUDED.play_id OR users.is_playing = false THEN EXCLUDED.packed_data ELSE users.packed_data END),
			save_time = $2
		RETURNING *)
		SELECT id, premium_expiration_date FROM rows'
	USING insert_values, v_utc_now;
END
$_$;


ALTER FUNCTION public.set_play_users(insert_values text[]) OWNER TO postgres;

--
-- TOC entry 221 (class 1255 OID 90135)
-- Name: test_func(text[]); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.test_func(p_values text[]) RETURNS TABLE(id text)
    LANGUAGE plpgsql
    AS $_$
		
BEGIN
	SELECT ARRAY_CAT($1) as id;
END
$_$;


ALTER FUNCTION public.test_func(p_values text[]) OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 32773)
-- Name: plays; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.plays (
    id character varying NOT NULL,
    region character varying NOT NULL,
    city character varying NOT NULL,
    number integer NOT NULL,
    address character varying NOT NULL,
    maintenance boolean DEFAULT false NOT NULL,
    busy real DEFAULT 0 NOT NULL,
    set_time timestamp without time zone,
    opening_soon boolean DEFAULT false NOT NULL
);


ALTER TABLE public.plays OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 57348)
-- Name: purchases; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.purchases (
    order_id character varying NOT NULL,
    product_id character varying NOT NULL,
    date timestamp without time zone DEFAULT timezone('utc'::text, now()) NOT NULL,
    user_id character varying NOT NULL
);


ALTER TABLE public.purchases OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 40973)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id character varying NOT NULL,
    play_id character varying DEFAULT ''::character varying NOT NULL,
    is_playing boolean DEFAULT false NOT NULL,
    manager boolean DEFAULT false NOT NULL,
    packed_data jsonb DEFAULT '{}'::json NOT NULL,
    premium_expiration_date timestamp without time zone,
    created_time timestamp without time zone DEFAULT timezone('utc'::text, now()) NOT NULL,
    save_time timestamp without time zone,
    premium_accumulated_days integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 32783)
-- Name: waits; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.waits (
    id integer NOT NULL,
    maintenance boolean DEFAULT false NOT NULL
);


ALTER TABLE public.waits OWNER TO postgres;

--
-- TOC entry 3399 (class 0 OID 32773)
-- Dependencies: 215
-- Data for Name: plays; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.plays VALUES ('ap-seoul-1', 'AsiaPacific', 'Seoul', 1, '127.0.0.1:8000', false, 0.125, '2024-04-20 07:10:34.989834', false);


--
-- TOC entry 3402 (class 0 OID 49156)
-- Dependencies: 218
-- Data for Name: premium_logs; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3403 (class 0 OID 57348)
-- Dependencies: 219
-- Data for Name: purchases; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 3401 (class 0 OID 40973)
-- Dependencies: 217
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.users VALUES ('kFxwpFtNUPREg7yvmuxdcdtRpup1', 'ap-seoul-1', true, false, '{"Exp": {"Value": 1000, "ChargeTime": "0001-01-01T00:00:00"}, "Emoji": {"LikeDelay": 0}, "Title": {"LikePoint": 0}, "Welcome": {"Value": true}, "Characters": {"Items": [{"Id": 0, "Exp": 0, "Skins": [{"Id": 0, "IsRandomSelected": true}, {"Id": 1, "IsRandomSelected": false}, {"Id": 2, "IsRandomSelected": false}, {"Id": 3, "IsRandomSelected": false}, {"Id": 4, "IsRandomSelected": false}, {"Id": 5, "IsRandomSelected": false}], "IsPicked": true, "IsRandomSkin": false, "SelectedSkinId": 0}, {"Id": 1, "Exp": 0, "Skins": [{"Id": 0, "IsRandomSelected": true}, {"Id": 1, "IsRandomSelected": false}, {"Id": 2, "IsRandomSelected": false}, {"Id": 3, "IsRandomSelected": false}, {"Id": 4, "IsRandomSelected": false}, {"Id": 5, "IsRandomSelected": false}], "IsPicked": false, "IsRandomSkin": false, "SelectedSkinId": 0}, {"Id": 2, "Exp": 0, "Skins": [{"Id": 0, "IsRandomSelected": true}, {"Id": 1, "IsRandomSelected": false}, {"Id": 2, "IsRandomSelected": false}, {"Id": 3, "IsRandomSelected": false}, {"Id": 4, "IsRandomSelected": false}, {"Id": 5, "IsRandomSelected": false}], "IsPicked": false, "IsRandomSkin": false, "SelectedSkinId": 0}, {"Id": 3, "Exp": 0, "Skins": [{"Id": 0, "IsRandomSelected": true}, {"Id": 1, "IsRandomSelected": false}, {"Id": 2, "IsRandomSelected": false}, {"Id": 3, "IsRandomSelected": false}, {"Id": 4, "IsRandomSelected": false}, {"Id": 5, "IsRandomSelected": false}], "IsPicked": true, "IsRandomSkin": false, "SelectedSkinId": 0}, {"Id": 4, "Exp": 0, "Skins": [{"Id": 0, "IsRandomSelected": true}, {"Id": 1, "IsRandomSelected": false}, {"Id": 2, "IsRandomSelected": false}, {"Id": 3, "IsRandomSelected": false}, {"Id": 4, "IsRandomSelected": false}, {"Id": 5, "IsRandomSelected": false}], "IsPicked": false, "IsRandomSkin": false, "SelectedSkinId": 0}, {"Id": 5, "Exp": 0, "Skins": [{"Id": 0, "IsRandomSelected": true}, {"Id": 1, "IsRandomSelected": false}, {"Id": 2, "IsRandomSelected": false}, {"Id": 3, "IsRandomSelected": false}, {"Id": 4, "IsRandomSelected": false}, {"Id": 5, "IsRandomSelected": false}], "IsPicked": false, "IsRandomSkin": false, "SelectedSkinId": 0}], "PickTime": "2024-04-21T00:00:00", "SelectedItemId": 0}, "GameArgses": {"Items": [{"Id": 0, "Score": 0}, {"Id": 1, "Score": 0}], "SelectedItemId": 0}, "GameResults": {"Items": [{"Game": null, "Cycles": 0, "EndTime": "0001-01-01T00:00:00", "Success": false, "Character": {"Id": 0, "Exp": 0}, "Specialties": [{"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}]}, {"Game": null, "Cycles": 0, "EndTime": "0001-01-01T00:00:00", "Success": false, "Character": {"Id": 0, "Exp": 0}, "Specialties": [{"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}]}, {"Game": null, "Cycles": 0, "EndTime": "0001-01-01T00:00:00", "Success": false, "Character": {"Id": 0, "Exp": 0}, "Specialties": [{"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}]}, {"Game": null, "Cycles": 0, "EndTime": "0001-01-01T00:00:00", "Success": false, "Character": {"Id": 0, "Exp": 0}, "Specialties": [{"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}]}, {"Game": null, "Cycles": 0, "EndTime": "0001-01-01T00:00:00", "Success": false, "Character": {"Id": 0, "Exp": 0}, "Specialties": [{"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}]}, {"Game": null, "Cycles": 0, "EndTime": "0001-01-01T00:00:00", "Success": false, "Character": {"Id": 0, "Exp": 0}, "Specialties": [{"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}]}, {"Game": null, "Cycles": 0, "EndTime": "0001-01-01T00:00:00", "Success": false, "Character": {"Id": 0, "Exp": 0}, "Specialties": [{"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}]}, {"Game": null, "Cycles": 0, "EndTime": "0001-01-01T00:00:00", "Success": false, "Character": {"Id": 0, "Exp": 0}, "Specialties": [{"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}]}, {"Game": null, "Cycles": 0, "EndTime": "0001-01-01T00:00:00", "Success": false, "Character": {"Id": 0, "Exp": 0}, "Specialties": [{"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}]}, {"Game": null, "Cycles": 0, "EndTime": "0001-01-01T00:00:00", "Success": false, "Character": {"Id": 0, "Exp": 0}, "Specialties": [{"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}]}]}, "Specialties": {"Items": [{"Id": 29, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}, {"Id": 0, "Number": 0}]}}', NULL, '2024-04-20 07:09:33.708851', '2024-04-20 07:10:34.989831', 0);


--
-- TOC entry 3400 (class 0 OID 32783)
-- Dependencies: 216
-- Data for Name: waits; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.waits VALUES (1, false);


--
-- TOC entry 3243 (class 2606 OID 32781)
-- Name: plays plays_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.plays
    ADD CONSTRAINT plays_pkey PRIMARY KEY (id);


--
-- TOC entry 3249 (class 2606 OID 49163)
-- Name: premium_logs premium_log_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.premium_logs
    ADD CONSTRAINT premium_log_pkey PRIMARY KEY (id);


--
-- TOC entry 3252 (class 2606 OID 98305)
-- Name: premium_logs premium_logs_reason_purchase_order_id_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.premium_logs
    ADD CONSTRAINT premium_logs_reason_purchase_order_id_key UNIQUE (reason, purchase_order_id);


--
-- TOC entry 3254 (class 2606 OID 57355)
-- Name: purchases purchases_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.purchases
    ADD CONSTRAINT purchases_pkey PRIMARY KEY (order_id);


--
-- TOC entry 3247 (class 2606 OID 40983)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- TOC entry 3245 (class 2606 OID 32788)
-- Name: waits waits_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.waits
    ADD CONSTRAINT waits_pkey PRIMARY KEY (id);


--
-- TOC entry 3250 (class 1259 OID 49171)
-- Name: premium_log_user_id_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX premium_log_user_id_idx ON public.premium_logs USING btree (user_id) WITH (deduplicate_items='true');


--
-- TOC entry 3255 (class 2606 OID 49166)
-- Name: premium_logs premium_log_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.premium_logs
    ADD CONSTRAINT premium_log_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id) NOT VALID;


-- Completed on 2024-04-20 16:11:50

--
-- PostgreSQL database dump complete
--

