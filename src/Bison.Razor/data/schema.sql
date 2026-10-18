drop table if exists authors;
create table authors (
  user_id integer primary key autoincrement,
  username string not null,
  email string not null,
  pw_hash string not null
);

drop table if exists observation;
create table observation (
  observation_id integer primary key autoincrement,
  author_id integer not null,
  text string not null,
  pub_date integer,
  foreign key (author_id, text, pub_date) references post(author_id, text,pub_date)
);

drop table if exists proposal;
create table proposal (
  proposal_id integer primary key autoincrement,
  author_id integer not null,
  taxon_id string not null,
  pub_date integer,
  observation_id integer references observation(observation_id),
  foreign key (author_id, pub_date) references post(author_id,pub_date)
);

drop table if exists comment;
create table comment (
  comment_id integer primary key autoincrement,
  author_id integer not null,
  text string not null,
  pub_date integer,
  observation_id integer references observation(observation_id),
  foreign key (author_id, text, pub_date) references post(author_id, text,pub_date)
);

drop table if exists post;
create table post (
  id integer primary key autoincrement,
  text string not null, 
  pub_date integer,
  author_id integer not null
);