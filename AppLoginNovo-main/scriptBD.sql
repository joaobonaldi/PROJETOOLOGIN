drop database if exists LoginCore;

create database LoginCore;
use LoginCore;

create table Cliente
(
Id int auto_increment primary key,
Nome Varchar(50) not null,
Nascimento DateTime not null,
Sexo char(1),
CPF Varchar(15) not null,
Telefone Varchar(14) not null,
Email Varchar(50) not null,
Senha Varchar(10) not null,
Situacao char(1) not null
);

create table Colaborador
(
Id int auto_increment primary key,
Nome Varchar(50) not null,
Email Varchar(50) not null,
Senha Varchar(10) not null,
Tipo Varchar(8) not null,
CPF Varchar(11) not null,
Telefone Varchar(11) not null
);

insert into Cliente values (default, "Isabela", "2008-07-09", "M", "12345678910", "11999999999999", "fratabela@gmail.com", "12345678", "A");

insert into Colaborador values (default, "Marina", "marinanogali@gmail.com", "12345678", "G", "12345678910", "11999999999");

select * from Colaborador;

select * from Cliente;
