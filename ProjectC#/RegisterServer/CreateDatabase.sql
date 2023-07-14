create  database users
go
use users
create table Users(
id int Primary key Identity(1,1),
[UserName] varchar(max) NOT NULL,
[Password] varchar(max)
)