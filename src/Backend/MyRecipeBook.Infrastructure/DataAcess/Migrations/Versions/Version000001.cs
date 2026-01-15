using System;
using FluentMigrator;

namespace MyRecipeBook.Infrastructure.DataAcess.Migrations.Versions;

[Migration(1, "Create table to save user's informations")]
public class Version000001 : VersionBase
{
    public override void Up()
    {
        CreateTable("Users")
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable();
    }

}
