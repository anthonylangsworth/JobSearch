if exists
	(select *
	from INFORMATION_SCHEMA.TABLES
	where TABLE_NAME = 'Activities'
	and TABLE_TYPE = 'BASE TABLE')
begin
	drop table dbo.Activities
end

create table dbo.Activities
(
	Id int identity(1,1) not null primary key,
	Start datetime not null,
	Description nvarchar(max) NULL,
	Duration time(7) not null,
	Completed bit not null,
	Contact_Id int null,
	JobOpening_Id int null,
)

if exists
	(select *
	from INFORMATION_SCHEMA.TABLES
	where TABLE_NAME = 'Contacts'
	and TABLE_TYPE = 'BASE TABLE')
begin
	drop table dbo.Contacts
end

create table dbo.Contacts
(
	Id int identity(1,1) not null primary key,
	Name nvarchar(max) null,
	Phone nvarchar(max) null,
	Email nvarchar(max) null,
	Notes nvarchar(max) null,
	Organization nvarchar(max) null,
	Role int not null,
	JobOpening_Id int null,
)

if exists
	(select *
	from INFORMATION_SCHEMA.TABLES
	where TABLE_NAME = 'JobOpenings'
	and TABLE_TYPE = 'BASE TABLE')
begin
	drop table dbo.JobOpenings
end

create table dbo.JobOpenings
(
	Id int identity(1,1) not null primary key,
	Url nvarchar(max) null,
	Title nvarchar(max) not null,
	Organization nvarchar(max) not null,
	Notes nvarchar(max) not null,
	AdvertisedDate datetime not null,
)

create nonclustered index IX_Contact_Id on dbo.Activities
(
	Contact_Id ASC
)

create nonclustered index IX_JobOpening_Id on dbo.Activities
(
	JobOpening_Id ASC
)

create nonclustered index IX_JobOpening_Id on dbo.Contacts
(
	JobOpening_Id ASC
)

alter table dbo.Activities with check
add constraint [FK_dbo.Activities_dbo.Contacts_Contact_Id]
foreign key(Contact_Id) references dbo.Contacts(Id)

alter table dbo.Activities with check
add constraint [FK_dbo.Activities_dbo.JobOpenings_JobOpening_Id] 
foreign key(JobOpening_Id)
references dbo.JobOpenings(Id)

alter table dbo.Contacts with check
add constraint[FK_dbo.Contacts_dbo.JobOpenings_JobOpening_Id] 
foreign key (JobOpening_Id)
references dbo.JobOpenings(Id)
