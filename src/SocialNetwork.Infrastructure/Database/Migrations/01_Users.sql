START TRANSACTION;

CREATE DATABASE IF NOT EXISTS SocialNetwork;

USE SocialNetwork;

CREATE TABLE Users (
       Id bigint NOT NULL AUTO_INCREMENT,
       UserName varchar(256) NOT NULL,
       NormalizedUserName varchar(256) NOT NULL,
       PasswordHash text NOT NULL,
       Name varchar(256) NOT NULL,
       NormalizedName varchar(256) NOT NULL,
       Surname varchar(256) NOT NULL,
       NormalizedSurname varchar(256) NOT NULL,
       Age tinyint NULL,
       Interests varchar(1024) NULL,
       City varchar(256) NOT NULL,
       CONSTRAINT Id_PK PRIMARY KEY (Id)
);

COMMIT;