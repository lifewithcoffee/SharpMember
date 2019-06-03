# Notes for SharpMember

## Database

- Support SQL Server and SQLite

- Database selection is controlled in
  - GlobalConfigs.DatabaseType
  - E:\rp\git\SharpMember\SharpMember.Core\ServiceExt.cs

- Connection Info
  - SQLite file name is in "DbConsts.SqliteDbFileName"
  - SQL Server: 
	- Server: localhost\Express
	- Database: SharpMember
	- setting file: SharpMember/appsettings.json

- Database migration: