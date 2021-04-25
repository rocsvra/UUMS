参考文档：
https://docs.microsoft.com/zh-cn/aspnet/core/data/ef-rp/migrations?view=aspnetcore-5.0&tabs=visual-studio
https://docs.microsoft.com/zh-cn/ef/core/cli/powershell

//Get-Help about_EntityFrameworkCore

Cmdlet                      Description
--------------------------  ---------------------------------------------------
Add-Migration               Adds a new migration.

Drop-Database               Drops the database.

Get-DbContext               Lists and gets information about available DbContext types.

Get-Migration               Lists available migrations.

Remove-Migration            Removes the last migration.

Scaffold-DbContext          Scaffolds a DbContext and entity types for a database.

Script-DbContext            Generates a SQL script from the DbContext. Bypasses any migrations.

Script-Migration            Generates a SQL script from migrations.

Update-Database             Updates the database to a specified migration.

存在多个DbContext：add-migration 迁移名称 -c ConfigurationDbContext

注意：
1.IEnumerable 集合上调用 Contains，则使用 .NET Core 实现。 
  如果在 IQueryable 对象上调用 Contains，则使用数据库实现。
  出于性能考虑，通常首选对 IQueryable 调用 Contains。 数据库服务器利用 IQueryable 完成筛选。 
  如果先创建 IEnumerable，则必须从数据库服务器返回所有行。
2.AsNoTracking(),不跟踪返回实体，可以提升性能。