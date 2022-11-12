using System.Diagnostics;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Translation.Common;
using Translation.Domain.Engine.SeedWork;
using User.Domain.AggregatesModel.UserAggregate;
using User.Infrastructure.Constants;

namespace User.Infrastructure.EntityConfigurations;
internal sealed class DisabilityEntityTypeConfiguration : IEntityTypeConfiguration<Disability>
{



    public void Configure(EntityTypeBuilder<Disability> builder)
    {
        builder.ToTable(TableNames.Disabilities, UserContext.DefaultSchema);
        var seed = Disability.InitialSeedData();

        builder.HasData(seed.Select(y => new
        {
            y.Id,
            y.UpdatedDate,
            y.UpdatedUser,
           // Description = System.Text.Json.JsonSerializer.Serialize(y.Description),
           // Name = System.Text.Json.JsonSerializer.Serialize(y.Name),
        }));

        Debugger.Launch();
        builder.HasKey(u => u.Id);
        // See - https://www.youtube.com/watch?v=sYvoIpsNGHM
        builder.OwnsMany(y => y.Name, a =>
        {
            a.ToJson();
            a.HasData(new
            {
                Id = 2,
                DisabilityId = 1,
                Culture = "test",
                Value = "test",
            });
            //a.HasData(seed.SelectMany(y => y.Name.Select((z, i) => new
            //{
            //    Id = i + 1,
            //    DisabilityId = y.Id,
            //    z.Culture,
            //    z.Value,
            //})));

        });

        // builder.OwnsOne(y => y.Name).HasData()

        builder.Ignore(y => y.Description);
        // builder.OwnsMany(y => y.Description, a =>
        // {
        //     a.HasData(seed.Select(y => new
        //     {
        //         y.Id,
        //         DisabilityId = y.Id,
        //         y.Description,
        //     }));
        //     a.ToJson();
        // });



        // builder.HasData(Disability.InitialSeedData());
        // builder.HasLocalizedField(y => y.Name);

        // builder.HasLocalizedField(y => y.Description);

        // TODO: See if passing the localizedProperties can be avoided by using the EntityTypeBuilder.
       // builder.HasLocalizedData(Disability.InitialSeedData(), x => x.Name, y => y.Description);
    }
}


public static class AnnotationExtension
{
    public static EntityTypeBuilder HasLocalizedField<TEntity, TProperty>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, TProperty>> localizedProperty) where TEntity : LocalizedEntity
    {
        var expression = (MemberExpression)localizedProperty.Body;
        string name = expression.Member.Name;

        string localizedName = $"Localized{name}";
        Console.Write(localizedName);
        // The actual property should be not mapped, while a shadow property will be responsible for containing all the data we need.
        // builder.Ignore(name);

        // builder.Ignore(nameof(LocalizedEntity.UserCulture));

        // builder.Ignore(nameof(LocalizedEntity.SiteCulture));

        // builder.Ignore(nameof(LocalizedEntity.FallbackCulture));


        // builder.HasMany(localizedName);
        // builder.OwnsMany(typeof(LocalizedString), localizedName, c =>
        // {
        //     c.Property(typeof(List<LocalizedString>), localizedName);
        //     c.ToJson();
        // });
        // 
        // builder
        //     .Property(typeof(List<LocalizedString>), localizedName)
        //     .HasJsonPropertyName(name)
        //     .HasAnnotation(nameof(LocalizedAttribute), new LocalizedAttribute() { PropertyName = name });


        return builder;
    }

    public static EntityTypeBuilder HasLocalizedData<TEntity, TProperty>(this EntityTypeBuilder<TEntity> builder, IList<TEntity> data, params Expression<Func<TEntity, TProperty>>[] localizedProperties) where TEntity : LocalizedEntity
    {
        List<IDictionary<string, object?>> mappedData = new();

        Type type = typeof(TEntity);
        System.Diagnostics.Debugger.Launch();
        foreach (TEntity record in data)
        {
            IDictionary<string, object?> currentEntityData = new ExpandoObject();

            IEnumerable<string> localizedNames = localizedProperties.Select(y => ((MemberExpression)y.Body).Member.Name);

            foreach (string property in localizedNames)
            {
                object? value = type.GetProperty(property)?.GetValue(record);
                currentEntityData.Add(property, value);
            }

            foreach (PropertyInfo prop in type.GetProperties().Where(p => !localizedNames.Contains(p.Name)))
            {
                object? value = prop.GetValue(record);
                currentEntityData.Add(prop.Name, value);
            }

            mappedData.Add(currentEntityData);
        }

        builder.HasData(mappedData);
        return builder;
    }
}
