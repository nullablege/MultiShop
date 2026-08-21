using Microsoft.EntityFrameworkCore;
using MultiShop.Message.Entities;

namespace MultiShop.Message.Context;

public sealed class MessageContext : DbContext
{
    public MessageContext(DbContextOptions<MessageContext> options)
        : base(options)
    {
    }

    public DbSet<UserMessage> UserMessages => Set<UserMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var message = modelBuilder.Entity<UserMessage>();

        message.ToTable("UserMessages");
        message.HasKey(x => x.UserMessageId);

        message.Property(x => x.SenderId)
            .HasMaxLength(450)
            .IsRequired();

        message.Property(x => x.ReceiverId)
            .HasMaxLength(450)
            .IsRequired();

        message.Property(x => x.Subject)
            .HasMaxLength(200)
            .IsRequired();

        message.Property(x => x.MessageDetail)
            .HasMaxLength(4000)
            .IsRequired();

        message.Property(x => x.CreatedAtUtc)
            .IsRequired();

        message.HasIndex(x => new { x.ReceiverId, x.CreatedAtUtc });
        message.HasIndex(x => new { x.SenderId, x.CreatedAtUtc });
    }
}
