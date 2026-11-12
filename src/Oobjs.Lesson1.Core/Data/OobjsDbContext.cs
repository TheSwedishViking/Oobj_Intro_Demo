using Microsoft.EntityFrameworkCore;
using Oobjs.Lesson1.Core.Entities;

namespace Oobjs.Lesson1.Core.Data;

public class OobjsDbContext : DbContext
{
	public OobjsDbContext(DbContextOptions<OobjsDbContext> options)
		: base(options)
	{
	}

	public DbSet<Oobj> Oobjs => Set<Oobj>();

	public DbSet<FieldValue> FieldValues => Set<FieldValue>();

	public DbSet<Relation> Relations => Set<Relation>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Oobj>()
			.HasKey(x => x.Guid);

		modelBuilder.Entity<FieldValue>()
			.HasKey(x => x.Id);

		modelBuilder.Entity<Relation>()
			.HasKey(x => x.Id);

		modelBuilder.Entity<FieldValue>()
			.HasOne<Oobj>()
			.WithMany()
			.HasForeignKey(x => x.OobjGuid)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Relation>()
			.HasOne<Oobj>()
			.WithMany()
			.HasForeignKey(x => x.ParentGuid)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Relation>()
			.HasOne<Oobj>()
			.WithMany()
			.HasForeignKey(x => x.ChildGuid)
			.OnDelete(DeleteBehavior.Restrict);
	}
}