## OMSBlazor
Real-time Order Management System
------------------------
### How start your application
In order to start application you should start `HttpApi.Host` project first, then start `.Blazor` project
In order to log in to the system use this default credentials: Login - **admin**, password - **1q2w3E***

------------------------
<b>1. Switch to EF Core SQLite Provider:</b>  
https://docs.abp.io/en/abp/latest/Entity-Framework-Core-SQLite
> For SQLite connection string use this pattern: "Filename=./MyDatabaseName.db",
this pattern will create databases in `bin` folder of the project where migrations are executed

<b>2. Separate host and identity data bases-</b>

- Create db context and design time db factory for new database using [this](https://docs.abp.io/en/abp/latest/Entity-Framework-Core-Migrations#create-a-second-dbcontext) article
> Don't forget to set attribute `ConnectionString` for your new db context. Make your db context look
like in [here](https://github.com/SergeyDavidovich/OMSBlazor/blob/development/src/OMSBlazor.EntityFrameworkCore/EntityFrameworkCore/OMSBlazorIdentityDbContext.cs)
- Automate migration for second db context using [this](https://docs.abp.io/en/abp/latest/Entity-Framework-Core-Migrations#automate-the-second-database-schema-migration) article
- Edit `OnModelCreating` method of your new db context so it looks like in [this](https://docs.abp.io/en/abp/latest/Entity-Framework-Core-Migrations#separating-host-tenant-database-schemas) article
- You can run `.DbMigrator` project now
- Check `bin->Debug->net7.0` folder. Here you will find your new database

<b>3. Make manipulations with host database</b>

In previous step we separated host and identity databases. So let make a test migration to host database.

> First of all put your database in this folder `.DbMigrator->bin->Debug->net7.0`

Follow this steps:
- Create simple class, `CustomerDemographics` you can find it [here](https://github.com/SergeyDavidovich/OMSBlazor/blob/development/src/OMSBlazor.EntityFrameworkCore/HostModels/CustomerDemographic.cs).

- Inside your `DbContext` for host database add `DbSet`:
```
public DbSet<CustomerDemographics> CustomerDemographics { get; set; }
```

- Add this code in the `OnModelCreating` method of `DbContext` for host database:
```
builder.Entity<CustomerDemographics>(b =>
{
    b.ToTable("CustomerDemographics");

    b.HasKey(x => x.CustomerTypeId);

    b.HasData(new CustomerDemographics() { CustomerTypeId = 1, CustomerDescription = "Lorem ipsum" });
});
```

- Add migration (I attach sample that works for my project, for your project use your db context name): 
> Please remember that you should run this command in directory where your db context is
```
dotnet ef migrations add AddCustomerDemographics --context OMSBlazorDbContext
```

- Run db migrator

- Now you can check changes in your host database

<b>4. Add configuration for SQLite</b>

In the `OMSBlazorEntityFrameworkCoreModule` class of the `.EntityFrameworkCore` project add this lines of code:
```
//This configuration is needed for sqlite
//https://github.com/abpframework/abp/issues/5661?ysclid=lhfcwt9oqb875559031
Configure<AbpUnitOfWorkDefaultOptions>(options =>
{
    options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
});
```

<b>5. Add required connection strings</b>

In `appsettings.json` file of the `.HttpApi.Host` project add this connection strings:
```
"Default": "Filename=./NorthwindSQLite.db",
"AbpIdentity": "Filename=./NorthwindIdentitySQLite.db",
"AbpFeatureManagement": "Filename=./NorthwindIdentitySQLite.db",
"AbpAuditLogging": "Filename=./NorthwindIdentitySQLite.db"
```

`Default` - connection string is needed for connecting to the database where you store your Northwind
Why do we need other three connection strings(`AbpIdentity`, `AbpFeatureManagement`, `AbpAuditLogging`)? 
So this three connection strings are pointing to the database where tables for related module stored. 
So for example in this current case tables for Identity, Feature, and Audit logging modules are stored in
`NorthwindIdentitySQLite` database. If you don't write this connection strings ABP will try to find this tables
in the `Default` connection string(this is how Abp works)

### Migration to .net 8 and ABP V8 notes

- Add migration that add `AbpSettingDefinitions` table to `NorthwindSQLite`
- Add migration that add `LastPasswordChangeTime` column to `AbpUsers` table in `NorthwindIdentitySQLite`
