--- Venue
insert into dbo.Venue
values ('First venue', 'First venue address', '123 45 678 90 12')

--- Layout
insert into dbo.Layout
values (1, 'First layout'),
(1, 'Second layout'),
(1, 'Test layout')


--- Area
insert into dbo.Area
values (1, 'First area of first layout', 1, 1),
(1, 'Second area of first layout', 1, 1),
(2, 'First area of second layout', 4, 4)

--- Roles
insert into dbo.AspNetRoles
values (
'09dae078-8c4d-4c10-9c20-7e143f6cb9ba',	'Event',	'EVENT',	'0384d25e-31a8-41b0-bba8-a7c369e47629'),
('316401a4-8ffb-49f8-9388-64761ed1813c',	'User',	'USER',	'c3230bc0-3f58-4ac1-8402-a31e76c09d53'),
('fb9ec25a-3c45-4d33-b68e-e121d1d15a74',	'Admin',	'ADMIN',	'21c51e2e-e7c5-436b-876e-84ffd8bd98d1'),
('fda7440c-88a9-4abc-ae18-f66ded3274ad',	'Venue',	'VENUE',	'd3bc6679-5a57-43ae-aa67-c50884081992')

--- Seat
insert into dbo.Seat
values (1, 1, 1),
(1, 1, 2),
(1, 1, 3),
(1, 2, 2),
(2, 1, 1),
(1, 2, 1),
(2, 1, 2),
(2, 1, 3),
(2, 2, 2),
(2, 2, 1),
(3, 1, 1),
(3, 1, 2),
(3, 1, 3),
(3, 2, 2),
(3, 2, 1)