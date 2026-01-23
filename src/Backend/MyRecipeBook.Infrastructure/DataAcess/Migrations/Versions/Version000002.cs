using System;
using FluentMigrator;

namespace MyRecipeBook.Infrastructure.DataAcess.Migrations.Versions;

[Migration(2, "Create table to save recipes")]
public class Version000002 : VersionBase
{
    public override void Up()
    {
        CreateTable("Recipes")
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("CookingTime").AsInt32().Nullable()
            .WithColumn("Difficulty").AsInt32().Nullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Recipe_User_Id", "Users", "Id");

        CreateTable("Ingredients")
            .WithColumn("Item").AsString().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredients_Recipe_Id", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);

        CreateTable("Instructions")
            .WithColumn("Steep").AsInt32().NotNullable()
            .WithColumn("Text").AsString(2000).NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Instructions_Recipe_Id", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);

        CreateTable("DishType")
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_DishType_Recipe_Id", "Recipes", "Id")
            .OnDelete(System.Data.Rule.Cascade);
    }
}
