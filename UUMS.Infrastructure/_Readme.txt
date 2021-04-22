Cmdlet                      Description
--------------------------  ---------------------------------------------------
Add-Migration               Adds a new migration.
Drop-Database               Drops the database.
Get-DbContext               Gets information about a DbContext type.
Remove-Migration            Removes the last migration.
Scaffold-DbContext          Scaffolds a DbContext and entity types for a database.
Script-Migration            Generates a SQL script from migrations.
Update-Database             Updates the database to a specified migration.


存在多个DbContext：add-migration 迁移名称 -c ConfigurationDbContext