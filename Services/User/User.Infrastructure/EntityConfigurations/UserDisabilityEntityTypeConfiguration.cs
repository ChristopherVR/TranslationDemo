using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Infrastructure.Constants;

namespace User.Infrastructure.EntityConfigurations;
internal sealed class UserDisabilityEntityTypeConfiguration : IEntityTypeConfiguration<User.Domain.AggregatesModel.UserAggregate.UserDisability>
{
    public void Configure(EntityTypeBuilder<User.Domain.AggregatesModel.UserAggregate.UserDisability> builder)
    {
        builder.ToTable(TableNames.UserDisabilities, UserContext.DefaultSchema);

        builder.HasKey(u => u.Id);

        const string UserId = nameof(UserId);
        builder.Property(UserId).IsRequired(true);
    }
}
