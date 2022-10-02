using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Infrastructure.EntityConfigurations;
internal sealed class UserEntityTypeConfiguration : IEntityTypeConfiguration<User.Domain.AggregatesModel.UserAggregate.User>
{
    public void Configure(EntityTypeBuilder<User.Domain.AggregatesModel.UserAggregate.User> builder)
    {
        builder.ToTable("Users", UserContext.DefaultSchema);

        builder.HasKey(u => u.Id);

        builder.HasIndex(r => r.Username).IsUnique();

        //builder.Metadata
        //    ?.FindNavigation(nameof(User.))
        //    ?.SetPropertyAccessMode(PropertyAccessMode.Field);

        //builder.OwnsOne(p => p., ps =>
        //{
        //    ps.WithOwner();

        //    ps.HasData(
        //        new
        //        {

        //        });
        //});

        builder.HasData(Domain.AggregatesModel.UserAggregate.User.InitialSeedData());
    }
}
