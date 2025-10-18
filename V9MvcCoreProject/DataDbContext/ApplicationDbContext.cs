using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using V9MvcCoreProject.Entities.Models;

namespace V9MvcCoreProject.DataDbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    private readonly bool _isDesignTime;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, bool isDesignTime = false)
        : base(options)
    {
        _isDesignTime = isDesignTime;

        if (!_isDesignTime)
        {
            TryInitializeDatabase();
        }
    }

    private void TryInitializeDatabase()
    {
        try
        {
            var databaseCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            if (databaseCreator is not null)
            {
                if (!databaseCreator.CanConnect())
                {
                    databaseCreator.Create();
                }
                if (!databaseCreator.HasTables())
                {
                    databaseCreator.CreateTables();
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while initializing the database context.", ex);
        }
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public virtual DbSet<ListItems> ListItems { get; set; }
    public virtual DbSet<UserAccess> UserAccess { get; set; }
    public virtual DbSet<PermissionTemplate> PermissionTemplate { get; set; }
    public virtual DbSet<PermissionTemplateDetail> PermissionTemplateDetail { get; set; }
    public virtual DbSet<UserLoginLogs> UserLoginLogs { get; set; }
    public virtual DbSet<UserActivityLogs> UserActivityLogs { get; set; }
    public virtual DbSet<UserHistoryLogs> UserHistoryLogs { get; set; }
    public virtual DbSet<ApplicationFunctionalities> ApplicationFunctionalities { get; set; }
    public virtual DbSet<FormDetail> FormDetail { get; set; }
    public virtual DbSet<AppLogs> Logs { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.HasIndex(u => u.CNIC).IsUnique().HasDatabaseName("IDX_IcNumber");
            entity.HasIndex(u => u.Email).IsUnique().HasDatabaseName("IDX_Email");
            entity.HasIndex(u => u.UserName).IsUnique().HasDatabaseName("IDX_UserName");

            entity.Property(e => e.CNIC)
                  .IsRequired()
                  .HasMaxLength(13);

            entity.Property(e => e.DateCreated)
                  .HasDefaultValueSql("GETUTCDATE()") // Ensures DB defaulting
                  .IsRequired();

            entity.Property(e => e.UpdatedDate)
                  .HasDefaultValueSql("NULL"); // Ensures first-time NULL
        });

        //builder.Entity<AppUser>()
        //   .HasQueryFilter(user => user.IsActive);

        builder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)      // Client-side EF default
                .IsRequired();

            entity.HasQueryFilter(e => e.IsActive);
        });

        builder.Entity<Employee>()
           .HasIndex(e => e.Salary)
           .HasDatabaseName("IX_Employee_Salary"); // Optional: custom index name


        builder.Entity<ListItems>(entity =>
        {
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

            entity.Property(e => e.Text)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        builder.Entity<UserHistoryLogs>(entity =>
        {
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ActionMethod)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.NewValueJson).IsUnicode(false);

            entity.Property(e => e.OldValueJson).IsUnicode(false);
        });

        builder.Entity<UserActivityLogs>(entity =>
        {
            entity.Property(e => e.Action)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Controller)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Datetime).HasColumnType("datetime");

            entity.Property(e => e.Exception).HasColumnType("text");

            entity.Property(e => e.LogId).HasMaxLength(450);

            entity.Property(e => e.Method)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.Property(e => e.Path)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.QueryString)
                .IsRequired()
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.RequestBody)
                .IsRequired()
                .HasColumnType("text");

            entity.Property(e => e.ResponseBody)
                .IsRequired()
                .HasColumnType("text");
        });


        builder.Entity<UserLoginLogs>(entity =>
        {
            entity.Property(e => e.LoginTime).HasColumnType("datetime");

            entity.Property(e => e.StatusMessage)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        builder.Entity<PermissionTemplate>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.Property(e => e.TemplateName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        builder.Entity<PermissionTemplateDetail>(entity =>
        {
            entity.Property(e => e.FormName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Template)
                .WithMany(p => p.PermissionTemplateDetail)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PermissionTemplateDetail_PermissionTemplate");
        });

        builder.Entity<UserAccess>(entity =>
        {
            entity.Property(e => e.FormName).HasMaxLength(50);

            entity.Property(e => e.UserId).HasMaxLength(50);
        });

        builder.Entity<FormDetail>(entity =>
        {
            entity.Property(e => e.ActionName).HasMaxLength(50);

            entity.Property(e => e.ControllerName).HasMaxLength(50);

            entity.Property(e => e.DisplayName).HasMaxLength(50);

            entity.Property(e => e.FormName).HasMaxLength(50);

            entity.Property(e => e.IconCode).HasMaxLength(40).IsRequired(false); ;
        });

        builder.Entity<ApplicationFunctionalities>(entity =>
        {
            entity.Property(e => e.ActionMethodName).HasMaxLength(50);
            entity.Property(e => e.MenuReferenceName).HasMaxLength(150);

            entity.Property(e => e.FunctionalityName).HasMaxLength(50);
        });

        builder.Entity<AppLogs>(entity =>
        {
            entity.Property(e => e.Application)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Callsite)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Channel).IsUnicode(false);

            entity.Property(e => e.ControllerName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.DeviceId)
                .HasColumnName("DeviceID")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.EndTime).HasColumnType("datetime");

            entity.Property(e => e.ErrorCode).HasMaxLength(100);

            entity.Property(e => e.FunctionName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Ip)
                .HasColumnName("IP")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Level)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.LogId).HasMaxLength(450);

            entity.Property(e => e.Logged).HasColumnType("datetime");

            entity.Property(e => e.Logger)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Message).IsUnicode(false);

            entity.Property(e => e.RequestDateTime).HasColumnType("datetime");

            entity.Property(e => e.RequestId).HasMaxLength(450);

            entity.Property(e => e.RequestParameters).IsUnicode(false);

            entity.Property(e => e.RequestResponse).IsUnicode(false);

            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });


        //Auto Migration Seed Data

        var fixedDate = new DateTime(2025, 01, 01);
        var fixedSecurityStamp = "SEC-STATIC-001";
        var fixedConcurrencyStamp = "CONC-STATIC-001";

        //Seed for UserRole
        builder.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = 1,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = fixedConcurrencyStamp
            }
        );

        //Seed for ApplicationUser
        var hasher = new PasswordHasher<ApplicationUser>();
        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = 1,
                UserName = "abdulmussawir",
                CNIC = "4210148778829",
                NormalizedUserName = "ABDULMUSSAWIR",
                Email = "abdul_mussawir@hotmail.com",
                NormalizedEmail = "ABDUL_MUSSAWIR@HOTMAIL.com",
                EmailConfirmed = true,
                SecurityStamp = fixedSecurityStamp,
                ConcurrencyStamp = fixedConcurrencyStamp,
                RoleTemplateId = 1,
                PasswordHash = hasher.HashPassword(null, "123456"),
                DateCreated = fixedDate
            }
        );

        // Seed FormDetail
        builder.Entity<FormDetail>().HasData(
            new FormDetail { Id = 1, ControllerName = "Home", ActionName = "ViewsHome", FormName = "ViewsHome", IsActive = true, DisplayName = "Home", IconCode = null, DisplayOrder = 1 },
            new FormDetail { Id = 2, ControllerName = "Permission", ActionName = "Index", FormName = "Permission", IsActive = true, DisplayName = "User Permission", IconCode = null, DisplayOrder = 2 }
        );

        // Seed ApplicationFunctionality
        builder.Entity<ApplicationFunctionalities>().HasData(
            new ApplicationFunctionalities { Id = 1, FunctionalityName = "Index", FormId = 1, IsActive = true, ActionMethodName = "Index", IsMenuItem = true, MenuReferenceName = "Home" },
            new ApplicationFunctionalities { Id = 2, FunctionalityName = "Privacy", FormId = 1, IsActive = true, ActionMethodName = "Privacy", IsMenuItem = true, MenuReferenceName = "Home" },
            new ApplicationFunctionalities { Id = 3, FunctionalityName = "Add Role", FormId = 2, IsActive = true, ActionMethodName = "PermissionTemplate", IsMenuItem = true, MenuReferenceName = "User Permission" },
            new ApplicationFunctionalities { Id = 4, FunctionalityName = "Change Role", FormId = 2, IsActive = true, ActionMethodName = "ChangePermissionTemplate", IsMenuItem = true, MenuReferenceName = "User Permission" }
        );

        // Seed PermissionTemplate
        builder.Entity<PermissionTemplate>().HasData(
           new PermissionTemplate
           {
               Id = 1,
               TemplateName = "Super Admin",
               IsActive = true,
               CreatedBy = 1,
               CreatedDate = fixedDate
           }
       );

        // Seed PermissionTemplateDetail
        builder.Entity<PermissionTemplateDetail>().HasData(
            new PermissionTemplateDetail { Id = 1, TemplateId = 1, FormName = "ViewsHome", FunctionalityId = 1, IsAllow = true },
            new PermissionTemplateDetail { Id = 2, TemplateId = 1, FormName = "ViewsHome", FunctionalityId = 2, IsAllow = true },
            new PermissionTemplateDetail { Id = 3, TemplateId = 1, FormName = "Permission", FunctionalityId = 3, IsAllow = true },
            new PermissionTemplateDetail { Id = 4, TemplateId = 1, FormName = "Permission", FunctionalityId = 4, IsAllow = true }
        );
    }
}
