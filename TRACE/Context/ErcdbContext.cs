using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TRACE.Models;

namespace TRACE.Context;

public partial class ErcdbContext : DbContext
{
    public ErcdbContext()
    {
    }

    public ErcdbContext(DbContextOptions<ErcdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountCategory> AccountCategories { get; set; }

    public virtual DbSet<AccountGroup> AccountGroups { get; set; }

    public virtual DbSet<AccountPayable> AccountPayables { get; set; }

    public virtual DbSet<ActionType> ActionTypes { get; set; }

    public virtual DbSet<Advice> Advices { get; set; }

    public virtual DbSet<AdviceStatus> AdviceStatuses { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Barcode> Barcodes { get; set; }

    public virtual DbSet<BarcodeBatch> BarcodeBatches { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<CaseApplicant> CaseApplicants { get; set; }

    public virtual DbSet<CaseAssignment> CaseAssignments { get; set; }

    public virtual DbSet<CaseCategory> CaseCategories { get; set; }

    public virtual DbSet<CaseDocument> CaseDocuments { get; set; }

    public virtual DbSet<CaseEvent> CaseEvents { get; set; }

    public virtual DbSet<CaseEventType> CaseEventTypes { get; set; }

    public virtual DbSet<CaseMilestone> CaseMilestones { get; set; }

    public virtual DbSet<CaseMilestoneTemplate> CaseMilestoneTemplates { get; set; }

    public virtual DbSet<CaseNature> CaseNatures { get; set; }

    public virtual DbSet<CaseNote> CaseNotes { get; set; }

    public virtual DbSet<CaseRespondent> CaseRespondents { get; set; }

    public virtual DbSet<CaseStatus> CaseStatuses { get; set; }

    public virtual DbSet<CaseTask> CaseTasks { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CivilStatus> CivilStatuses { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<ContactEntry> ContactEntries { get; set; }

    public virtual DbSet<ContactEntryValue> ContactEntryValues { get; set; }

    public virtual DbSet<Correspondent> Correspondents { get; set; }

    public virtual DbSet<CorrespondentCategory> CorrespondentCategories { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<DeliveryMethod> DeliveryMethods { get; set; }

    public virtual DbSet<DistributionList> DistributionLists { get; set; }

    public virtual DbSet<DistributionListMember> DistributionListMembers { get; set; }

    public virtual DbSet<Division> Divisions { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentAssignment> DocumentAssignments { get; set; }

    public virtual DbSet<DocumentCategory> DocumentCategories { get; set; }

    public virtual DbSet<DocumentCopy> DocumentCopies { get; set; }

    public virtual DbSet<DocumentCorrespondent> DocumentCorrespondents { get; set; }

    public virtual DbSet<DocumentFilingType> DocumentFilingTypes { get; set; }

    public virtual DbSet<DocumentForward> DocumentForwards { get; set; }

    public virtual DbSet<DocumentLocation> DocumentLocations { get; set; }

    public virtual DbSet<DocumentLog> DocumentLogs { get; set; }

    public virtual DbSet<DocumentReply> DocumentReplies { get; set; }

    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    public virtual DbSet<DocumentVersion> DocumentVersions { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<EntityCategory> EntityCategories { get; set; }

    public virtual DbSet<Erccase> Erccases { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<EventLog> EventLogs { get; set; }

    public virtual DbSet<ExternalCase> ExternalCases { get; set; }

    public virtual DbSet<ExternalCaseCategory> ExternalCaseCategories { get; set; }

    public virtual DbSet<ExternalCaseDocument> ExternalCaseDocuments { get; set; }

    public virtual DbSet<ExternalCaseStatus> ExternalCaseStatuses { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Grid> Grids { get; set; }

    public virtual DbSet<HandlingOfficerType> HandlingOfficerTypes { get; set; }

    public virtual DbSet<Hearing> Hearings { get; set; }

    public virtual DbSet<HearingCategory> HearingCategories { get; set; }

    public virtual DbSet<HearingOfficer> HearingOfficers { get; set; }

    public virtual DbSet<HearingOfficerType> HearingOfficerTypes { get; set; }

    public virtual DbSet<HearingType> HearingTypes { get; set; }

    public virtual DbSet<HearingVenue> HearingVenues { get; set; }

    public virtual DbSet<HearingsInHearingType> HearingsInHearingTypes { get; set; }

    public virtual DbSet<InternalDocument> InternalDocuments { get; set; }

    public virtual DbSet<LogType> LogTypes { get; set; }

    public virtual DbSet<MigrationHistory> MigrationHistories { get; set; }

    public virtual DbSet<MilestonesAchieved> MilestonesAchieveds { get; set; }

    public virtual DbSet<NeededAction> NeededActions { get; set; }

    public virtual DbSet<OcNextBarcodeNo> OcNextBarcodeNos { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<OutgoingRecipient> OutgoingRecipients { get; set; }

    public virtual DbSet<PaymentDetail> PaymentDetails { get; set; }

    public virtual DbSet<PreFilingAssessment> PreFilingAssessments { get; set; }

    public virtual DbSet<PreFilingCategory> PreFilingCategories { get; set; }

    public virtual DbSet<PreFilingCorrespondent> PreFilingCorrespondents { get; set; }

    public virtual DbSet<PreFilingLog> PreFilingLogs { get; set; }

    public virtual DbSet<PreFilingStatus> PreFilingStatuses { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<RelatedCase> RelatedCases { get; set; }

    public virtual DbSet<Sector> Sectors { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Setting> Settings { get; set; }

    public virtual DbSet<SharepointMapping> SharepointMappings { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAction> UserActions { get; set; }

    public virtual DbSet<UserComment> UserComments { get; set; }

    public virtual DbSet<UserLog> UserLogs { get; set; }

    public virtual DbSet<UserTokenCache> UserTokenCaches { get; set; }

    public virtual DbSet<VersionStatus> VersionStatuses { get; set; }
    public virtual DbSet<CaseDetails> CaseDetails { get; set; }
    public virtual DbSet<CaseLastMile> CaseLastMiles { get; set; }
    public virtual DbSet<CaseBlobDocument> CaseBlobDocument { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ErcDatabase");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Accounts", "DS");

            entity.HasIndex(e => new { e.AccountName, e.AccountNo }, "IX_Accounts").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AccountCategoryId).HasColumnName("AccountCategoryID");
            entity.Property(e => e.AccountName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.AccountNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AlternateEmailAddress)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Branch)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MiddleInitial)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.PrimaryMobileNo)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.SecondaryMobileNo)
                .HasMaxLength(11)
                .IsUnicode(false);

            entity.HasOne(d => d.AccountCategory).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.AccountCategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Accounts_AccountCategories");

            entity.HasMany(d => d.AccountGroups).WithMany(p => p.Accounts)
                .UsingEntity<Dictionary<string, object>>(
                    "AccountsInAccountGroup",
                    r => r.HasOne<AccountGroup>().WithMany()
                        .HasForeignKey("AccountGroupId")
                        .HasConstraintName("FK_AccountsInAccountGroup_AccountGroups"),
                    l => l.HasOne<Account>().WithMany()
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK_AccountsInAccountGroup_Accounts"),
                    j =>
                    {
                        j.HasKey("AccountId", "AccountGroupId");
                        j.ToTable("AccountsInAccountGroup", "DS");
                        j.IndexerProperty<long>("AccountId").HasColumnName("AccountID");
                        j.IndexerProperty<long>("AccountGroupId").HasColumnName("AccountGroupID");
                    });
        });
        modelBuilder.Entity<CaseBlobDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__CaseBlob__1ABEEF6F67F88418");

            entity.ToTable("CaseBlobDocuments", "cases");

            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.AttachmentName).HasMaxLength(255);
            entity.Property(e => e.Ercid).HasColumnName("ERCId");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AccountCategory>(entity =>
        {
            entity.ToTable("AccountCategories", "DS");

            entity.HasIndex(e => e.Category, "IX_AccountCategories").IsUnique();

            entity.Property(e => e.AccountCategoryId).HasColumnName("AccountCategoryID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AccountGroup>(entity =>
        {
            entity.ToTable("AccountGroups", "DS");

            entity.HasIndex(e => e.GroupName, "IX_AccountGroups").IsUnique();

            entity.Property(e => e.AccountGroupId).HasColumnName("AccountGroupID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.GroupName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AccountPayable>(entity =>
        {
            entity.ToTable("AccountPayables", "DS");

            entity.Property(e => e.AccountPayableId).HasColumnName("AccountPayableID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.AdviceId).HasColumnName("AdviceID");
            entity.Property(e => e.AllotmentClass)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ObligationRequestNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WithholdingTax).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.AccountPayables)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_AccountPayables_Accounts");

            entity.HasOne(d => d.Advice).WithMany(p => p.AccountPayables)
                .HasForeignKey(d => d.AdviceId)
                .HasConstraintName("FK_AccountPayables_Advices");
        });

        modelBuilder.Entity<ActionType>(entity =>
        {
            entity.ToTable("ActionTypes", "doc");

            entity.HasIndex(e => e.Type, "IX_ActionTypes").IsUnique();

            entity.Property(e => e.ActionTypeId).HasColumnName("ActionTypeID");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Advice>(entity =>
        {
            entity.HasKey(e => e.AdviceId).HasName("PK_LDDAPs");

            entity.ToTable("Advices", "DS");

            entity.HasIndex(e => e.AdviceNo, "IX_Advices").IsUnique();

            entity.Property(e => e.AdviceId).HasColumnName("AdviceID");
            entity.Property(e => e.AdviceNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("AdviceNO");
            entity.Property(e => e.AdviceStatusId).HasColumnName("AdviceStatusID");
            entity.Property(e => e.DateTimeCreated).HasColumnType("datetime");
            entity.Property(e => e.FundCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ncano)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NCANo");
            entity.Property(e => e.Remarks).IsUnicode(false);

            entity.HasOne(d => d.AdviceStatus).WithMany(p => p.Advices)
                .HasForeignKey(d => d.AdviceStatusId)
                .HasConstraintName("FK_Advices_AdviceStatuses");
        });

        modelBuilder.Entity<AdviceStatus>(entity =>
        {
            entity.HasKey(e => e.AdviceStatusId).HasName("PK_LDDAP_Statuses");

            entity.ToTable("AdviceStatuses", "DS");

            entity.HasIndex(e => e.Status, "IX_AdviceStatuses").IsUnique();

            entity.Property(e => e.AdviceStatusId).HasColumnName("AdviceStatusID");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId).HasName("PK_cities");

            entity.ToTable("Areas", "locations");

            entity.HasIndex(e => new { e.AreaName, e.ProvinceId }, "IX_Areas").IsUnique();

            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.AreaName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.HasOne(d => d.Province).WithMany(p => p.Areas)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK_areas_provinces");
        });

        modelBuilder.Entity<Barcode>(entity =>
        {
            entity.ToTable("Barcodes", "doc");

            entity.Property(e => e.BarcodeId).HasColumnName("BarcodeID");
            entity.Property(e => e.BarcodeBatchId).HasColumnName("BarcodeBatchID");
            entity.Property(e => e.BarcodeNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.BarcodeBatch).WithMany(p => p.Barcodes)
                .HasForeignKey(d => d.BarcodeBatchId)
                .HasConstraintName("FK_Barcodes_BarcodeBatches");
        });

        modelBuilder.Entity<BarcodeBatch>(entity =>
        {
            entity.ToTable("BarcodeBatches", "doc");

            entity.Property(e => e.BarcodeBatchId).HasColumnName("BarcodeBatchID");
            entity.Property(e => e.DocumentCategoryId).HasColumnName("DocumentCategoryID");

            entity.HasOne(d => d.DocumentCategory).WithMany(p => p.BarcodeBatches)
                .HasForeignKey(d => d.DocumentCategoryId)
                .HasConstraintName("FK_BarcodeBatches_DocumentCategories");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillingId);

            entity.ToTable("Bills", "billing");

            entity.HasIndex(e => e.BillNo, "IX_Bills").IsUnique();

            entity.Property(e => e.BillingId).HasColumnName("BillingID");
            entity.Property(e => e.AssignedStaff)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BillNo)
                .HasMaxLength(16)
                .IsUnicode(false);
            entity.Property(e => e.BillTypeId).HasColumnName("BillTypeID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DatetimeApproved).HasColumnType("datetime");
            entity.Property(e => e.DatetimeCreated).HasColumnType("datetime");
        });

        modelBuilder.Entity<CaseApplicant>(entity =>
        {
            entity.ToTable("CaseApplicants", "cases");

            entity.Property(e => e.CaseApplicantId).HasColumnName("CaseApplicantID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CorrespondentId).HasColumnName("CorrespondentID");
            entity.Property(e => e.ErccaseId).HasColumnName("ERCCaseID");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.CaseApplicants)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_CaseApplicants_Companies");

            entity.HasOne(d => d.Correspondent).WithMany(p => p.CaseApplicants)
                .HasForeignKey(d => d.CorrespondentId)
                .HasConstraintName("FK_CaseApplicants_Correspondents");

            entity.HasOne(d => d.Erccase).WithMany(p => p.CaseApplicants)
                .HasForeignKey(d => d.ErccaseId)
                .HasConstraintName("FK_CaseApplicants_ERCCases");
        });

        modelBuilder.Entity<CaseAssignment>(entity =>
        {
            entity.ToTable("CaseAssignments", "cases");

            entity.HasIndex(e => new { e.UserId, e.ErccaseId, e.HandlingOfficerTypeId }, "IX_CaseAssignments").IsUnique();

            entity.Property(e => e.CaseAssignmentId).HasColumnName("CaseAssignmentID");
            entity.Property(e => e.AssignedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ErccaseId).HasColumnName("ERCCaseID");
            entity.Property(e => e.HandlingOfficerTypeId).HasColumnName("HandlingOfficerTypeID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Erccase).WithMany(p => p.CaseAssignments)
                .HasForeignKey(d => d.ErccaseId)
                .HasConstraintName("FK_CaseAssignments_Cases");

            entity.HasOne(d => d.HandlingOfficerType).WithMany(p => p.CaseAssignments)
                .HasForeignKey(d => d.HandlingOfficerTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaseAssignments_HandlingOfficerTypes");
        });

        modelBuilder.Entity<CaseCategory>(entity =>
        {
            entity.ToTable("CaseCategories", "cases");

            entity.HasIndex(e => e.Category, "IX_CaseCategories").IsUnique();

            entity.Property(e => e.CaseCategoryId).HasColumnName("CaseCategoryID");
            entity.Property(e => e.Category)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CaseDocument>(entity =>
        {
            entity.HasKey(e => e.CaseDocumentId).HasName("PK_CaseOrders");

            entity.ToTable("CaseDocuments", "cases");

            entity.HasIndex(e => new { e.DocumentId, e.ErccaseId }, "IX_CaseDocuments").IsUnique();

            entity.Property(e => e.CaseDocumentId).HasColumnName("CaseDocumentID");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.ErccaseId).HasColumnName("ERCCaseID");

            entity.HasOne(d => d.Document).WithMany(p => p.CaseDocuments)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_CaseDocuments_Documents");

            entity.HasOne(d => d.Erccase).WithMany(p => p.CaseDocuments)
                .HasForeignKey(d => d.ErccaseId)
                .HasConstraintName("FK_CaseDocuments_ERCCases");
        });

        modelBuilder.Entity<CaseEvent>(entity =>
        {
            entity.ToTable("CaseEvents", "cases");

            entity.Property(e => e.CaseEventId).HasColumnName("CaseEventID");
            entity.Property(e => e.CaseEventTypeId).HasColumnName("CaseEventTypeID");
            entity.Property(e => e.ErccaseId).HasColumnName("ERCCaseID");
            entity.Property(e => e.EventDatetime).HasColumnType("datetime");
            entity.Property(e => e.EventDescription)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.IsUserAction)
                .HasDefaultValue(false)
                .HasColumnName("isUserAction");
            entity.Property(e => e.UserId)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.CaseEventType).WithMany(p => p.CaseEvents)
                .HasForeignKey(d => d.CaseEventTypeId)
                .HasConstraintName("FK_CaseEvents_CaseEventTypes");

            entity.HasOne(d => d.Erccase).WithMany(p => p.CaseEvents)
                .HasForeignKey(d => d.ErccaseId)
                .HasConstraintName("FK_CaseEvents_Cases");
        });

        modelBuilder.Entity<CaseEventType>(entity =>
        {
            entity.ToTable("CaseEventTypes", "cases");

            entity.Property(e => e.CaseEventTypeId).HasColumnName("CaseEventTypeID");
            entity.Property(e => e.EventType)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CaseMilestone>(entity =>
        {
            entity.ToTable("CaseMilestones", "cases");

            entity.HasIndex(e => e.Milestone, "IX_CaseMilestones").IsUnique();

            entity.Property(e => e.CaseMilestoneId).HasColumnName("CaseMilestoneID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Milestone)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CaseMilestoneTemplate>(entity =>
        {
            entity.ToTable("CaseMilestoneTemplates", "cases");

            entity.HasIndex(e => e.TemplateName, "IX_CaseMilestoneTemplates").IsUnique();

            entity.Property(e => e.CaseMilestoneTemplateId).HasColumnName("CaseMilestoneTemplateID");
            entity.Property(e => e.CaseCategoryId).HasColumnName("CaseCategoryID");
            entity.Property(e => e.TemplateName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.CaseCategory).WithMany(p => p.CaseMilestoneTemplates)
                .HasForeignKey(d => d.CaseCategoryId)
                .HasConstraintName("FK_CaseMilestoneTemplates_CaseCategories");

            entity.HasMany(d => d.CaseMilestones).WithMany(p => p.CaseMilestoneTemplates)
                .UsingEntity<Dictionary<string, object>>(
                    "CaseMilestoneTemplateMember",
                    r => r.HasOne<CaseMilestone>().WithMany()
                        .HasForeignKey("CaseMilestoneId")
                        .HasConstraintName("FK_CaseMilestoneTemplateMembers_CaseMilestones"),
                    l => l.HasOne<CaseMilestoneTemplate>().WithMany()
                        .HasForeignKey("CaseMilestoneTemplateId")
                        .HasConstraintName("FK_CaseMilestoneTemplateMembers_CaseMilestoneTemplates"),
                    j =>
                    {
                        j.HasKey("CaseMilestoneTemplateId", "CaseMilestoneId");
                        j.ToTable("CaseMilestoneTemplateMembers", "cases");
                        j.IndexerProperty<long>("CaseMilestoneTemplateId").HasColumnName("CaseMilestoneTemplateID");
                        j.IndexerProperty<long>("CaseMilestoneId").HasColumnName("CaseMilestoneID");
                    });
        });

        modelBuilder.Entity<CaseNature>(entity =>
        {
            entity.ToTable("CaseNatures", "cases");

            entity.HasIndex(e => new { e.Nature, e.CaseCategoryId }, "IX_CaseNatures").IsUnique();

            entity.Property(e => e.CaseNatureId).HasColumnName("CaseNatureID");
            entity.Property(e => e.CaseCategoryId).HasColumnName("CaseCategoryID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Nature)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.CaseCategory).WithMany(p => p.CaseNatures)
                .HasForeignKey(d => d.CaseCategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CaseNatures_CaseCategories");
        });

        modelBuilder.Entity<CaseNote>(entity =>
        {
            entity.ToTable("CaseNotes", "cases");

            entity.Property(e => e.CaseNoteId).HasColumnName("CaseNoteID");
            entity.Property(e => e.DatetimeCreated).HasColumnType("datetime");
            entity.Property(e => e.ErccaseId).HasColumnName("ERCCaseID");
            entity.Property(e => e.NotedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Erccase).WithMany(p => p.CaseNotes)
                .HasForeignKey(d => d.ErccaseId)
                .HasConstraintName("FK_CaseNotes_ERCCases");
        });

        modelBuilder.Entity<CaseRespondent>(entity =>
        {
            entity.ToTable("CaseRespondents", "cases");

            entity.Property(e => e.CaseRespondentId).HasColumnName("CaseRespondentID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CorrespondentId).HasColumnName("CorrespondentID");
            entity.Property(e => e.ErccaseId).HasColumnName("ERCCaseID");
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.CaseRespondents)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_CaseRespondents_Companies");

            entity.HasOne(d => d.Correspondent).WithMany(p => p.CaseRespondents)
                .HasForeignKey(d => d.CorrespondentId)
                .HasConstraintName("FK_CaseRespondents_Correspondents");

            entity.HasOne(d => d.Erccase).WithMany(p => p.CaseRespondents)
                .HasForeignKey(d => d.ErccaseId)
                .HasConstraintName("FK_CaseRespondents_ERCCases");
        });

        modelBuilder.Entity<CaseStatus>(entity =>
        {
            entity.ToTable("CaseStatuses", "cases");

            entity.HasIndex(e => e.Status, "IX_CaseStatuses").IsUnique();

            entity.Property(e => e.CaseStatusId).HasColumnName("CaseStatusID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CaseTask>(entity =>
        {
            entity.ToTable("CaseTasks", "cases");

            entity.Property(e => e.CaseTaskId)
                  .HasColumnName("CaseTaskID");
            entity.Property(e => e.DatetimeCreated)
                  .HasDefaultValueSql("(getdate())")
                  .HasColumnType("datetime");
            entity.Property(e => e.DocumentId)
                  .HasColumnName("DocumentID");
            entity.Property(e => e.ErccaseId)
                  .HasColumnName("ERCCaseID");
            entity.Property(e => e.Task)
                  .HasMaxLength(500)
                  .IsUnicode(false);
            entity.Property(e => e.TaskedBy)
                  .HasMaxLength(50)
                  .IsUnicode(false);

            entity.Property(e => e.UserId)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .HasColumnName("UserID");

            entity.HasOne(d => d.Document)
                  .WithMany(p => p.CaseTasks)
                  .HasForeignKey(d => d.DocumentId)
                  .OnDelete(DeleteBehavior.SetNull)
                  .HasConstraintName("FK_CaseTasks_Documents");

            entity.HasOne(d => d.Erccase)
                  .WithMany(p => p.CaseTasks)
                  .HasForeignKey(d => d.ErccaseId)
                  .HasConstraintName("FK_CaseTasks_ERCCases");
            entity.HasOne(ct => ct.User)
                  .WithMany()
                  .HasForeignKey(ct => ct.UserId)
                  .HasPrincipalKey(u => u.Username)
                  .OnDelete(DeleteBehavior.Restrict)
                  .HasConstraintName("FK_CaseTasks_Users_ByUsername");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK_Cities_1");

            entity.ToTable("Cities", "locations");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CityName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.StateId).HasColumnName("StateID");

            entity.HasOne(d => d.State).WithMany(p => p.Cities)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cities_States");
        });

        modelBuilder.Entity<CivilStatus>(entity =>
        {
            entity.ToTable("CivilStatuses", "HR");

            entity.Property(e => e.CivilStatusId).HasColumnName("CivilStatusID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Companies", "contacts");

            entity.HasIndex(e => e.CompanyName, "IX_Companies").IsUnique();

            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.AddressLine1).HasMaxLength(500);
            entity.Property(e => e.AddressLine2).HasMaxLength(500);
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CompanyName).HasMaxLength(250);
            entity.Property(e => e.EntityCategoryId).HasColumnName("EntityCategoryID");
            entity.Property(e => e.ShortName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.City).WithMany(p => p.Companies)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_Companies_Cities");

            entity.HasOne(d => d.EntityCategory).WithMany(p => p.Companies)
                .HasForeignKey(d => d.EntityCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Companies_EntityCategories");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContactId).HasName("PK_contacts");

            entity.ToTable("Contacts", "contacts");

            entity.Property(e => e.ContactId).HasColumnName("ContactID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.ContactNos)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.Firstname)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Lastname)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Middlename)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Salutation)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StreetAddress).IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_contacts_areas");

            entity.HasOne(d => d.Entity).WithMany(p => p.Contacts)
                .HasForeignKey(d => d.EntityId)
                .HasConstraintName("FK_Contacts_Entities1");
        });

        modelBuilder.Entity<ContactEntry>(entity =>
        {
            entity.HasKey(e => e.ContactEntryId).HasName("PK_contact_entries");

            entity.ToTable("ContactEntries", "contacts");

            entity.HasIndex(e => e.Entry, "IX_ContactEntries").IsUnique();

            entity.Property(e => e.ContactEntryId).HasColumnName("ContactEntryID");
            entity.Property(e => e.Entry)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ContactEntryValue>(entity =>
        {
            entity.HasKey(e => e.ContactEntryValueId).HasName("PK_contact_entry_values");

            entity.ToTable("ContactEntryValues", "contacts");

            entity.Property(e => e.ContactEntryValueId).HasColumnName("ContactEntryValueID");
            entity.Property(e => e.ContactEntryId).HasColumnName("ContactEntryID");
            entity.Property(e => e.ContactId).HasColumnName("ContactID");
            entity.Property(e => e.EntryValue)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.ContactEntry).WithMany(p => p.ContactEntryValues)
                .HasForeignKey(d => d.ContactEntryId)
                .HasConstraintName("FK_contact_entry_values_contact_entries");

            entity.HasOne(d => d.Contact).WithMany(p => p.ContactEntryValues)
                .HasForeignKey(d => d.ContactId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_contact_entry_values_contacts");
        });

        modelBuilder.Entity<Correspondent>(entity =>
        {
            entity.HasKey(e => e.CorrespondentId).HasName("PK_Correspomdents2");

            entity.ToTable("Correspondents", "contacts");

            entity.Property(e => e.CorrespondentId).HasColumnName("CorrespondentID");
            entity.Property(e => e.AddressLine1).HasMaxLength(500);
            entity.Property(e => e.AddressLine2).HasMaxLength(500);
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.ContactNos).HasMaxLength(250);
            entity.Property(e => e.Designation)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EmailAddress).HasMaxLength(250);
            entity.Property(e => e.FirstName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Salutation)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode).HasMaxLength(10);

            entity.HasOne(d => d.City).WithMany(p => p.Correspondents)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_Correspondents_Cities");

            entity.HasOne(d => d.Company).WithMany(p => p.Correspondents)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_Correspondents_Companies");
        });

        modelBuilder.Entity<CorrespondentCategory>(entity =>
        {
            entity.ToTable("CorrespondentCategories", "doc");

            entity.Property(e => e.CorrespondentCategoryId).HasColumnName("CorrespondentCategoryID");
            entity.Property(e => e.Category)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Countries", "locations");

            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Acronym)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.CountryName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DeliveryMethod>(entity =>
        {
            entity.ToTable("DeliveryMethods", "doc");

            entity.HasIndex(e => e.Method, "IX_DeliveryMethods").IsUnique();

            entity.Property(e => e.DeliveryMethodId).HasColumnName("DeliveryMethodID");
            entity.Property(e => e.Method)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DistributionList>(entity =>
        {
            entity.ToTable("DistributionLists", "doc");

            entity.HasIndex(e => e.Name, "IX_DistributionLists").IsUnique();

            entity.Property(e => e.DistributionListId).HasColumnName("DistributionListID");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DistributionListMember>(entity =>
        {
            entity.ToTable("DistributionListMembers", "doc");

            entity.HasIndex(e => new { e.CorrespondentId, e.DistributionListId }, "IX_DistributionListMembers").IsUnique();

            entity.Property(e => e.DistributionListMemberId).HasColumnName("DistributionListMemberID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CorrespondentId).HasColumnName("CorrespondentID");
            entity.Property(e => e.DistributionListId).HasColumnName("DistributionListID");

            entity.HasOne(d => d.Company).WithMany(p => p.DistributionListMembers)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_DistributionListMembers_Companies");

            entity.HasOne(d => d.Correspondent).WithMany(p => p.DistributionListMembers)
                .HasForeignKey(d => d.CorrespondentId)
                .HasConstraintName("FK_DistributionListMembers_Correspondents");

            entity.HasOne(d => d.DistributionList).WithMany(p => p.DistributionListMembers)
                .HasForeignKey(d => d.DistributionListId)
                .HasConstraintName("FK_DistributionListMembers_DistributionLists");
        });

        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasKey(e => e.DivisionId).HasName("PK_Departments");

            entity.ToTable("Divisions", "HR");

            entity.Property(e => e.DivisionId).HasColumnName("DivisionID");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.DivisionName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Secretary)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

            entity.HasOne(d => d.Service).WithMany(p => p.Divisions)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Divisions_Services");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.ToTable("Documents", "doc");

            entity.HasIndex(e => new { e.DocumentCategoryId, e.BarcodeNo }, "IX_Documents").IsUnique();

            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AttachedEvidences)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BarcodeNo)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.CustomerBarcode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DatetimeApproved).HasColumnType("datetime");
            entity.Property(e => e.DatetimeFiled).HasColumnType("datetime");
            entity.Property(e => e.DatetimeInitiated).HasColumnType("datetime");
            entity.Property(e => e.DeliveryMethodId).HasColumnName("DeliveryMethodID");
            entity.Property(e => e.DocumentCategoryId).HasColumnName("DocumentCategoryID");
            entity.Property(e => e.DocumentFilingTypeId).HasColumnName("DocumentFilingTypeID");
            entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            entity.Property(e => e.InternalAuthor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsDraft).HasColumnName("isDraft");
            entity.Property(e => e.IsFiled)
                .HasDefaultValue(false)
                .HasColumnName("isFiled");
            entity.Property(e => e.LastActivityDatetime).HasColumnType("datetime");
            entity.Property(e => e.OfficeId).HasColumnName("OfficeID");
            entity.Property(e => e.OriginatingDivisionId).HasColumnName("OriginatingDivisionID");
            entity.Property(e => e.Subject)
                .HasMaxLength(6000)
                .IsUnicode(false);

            entity.HasOne(d => d.DeliveryMethod).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DeliveryMethodId)
                .HasConstraintName("FK_Documents_DeliveryMethods");

            entity.HasOne(d => d.DocumentCategory).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentCategoryId)
                .HasConstraintName("FK_Documents_DocumentCategories");

            entity.HasOne(d => d.DocumentFilingType).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentFilingTypeId)
                .HasConstraintName("FK_Documents_DocumentFilingTypes");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Documents)
                .HasForeignKey(d => d.DocumentTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Documents_DocumentTypes");

            entity.HasOne(d => d.Office).WithMany(p => p.Documents)
                .HasForeignKey(d => d.OfficeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Documents_Offices");

            entity.HasOne(d => d.OriginatingDivision).WithMany(p => p.Documents)
                .HasForeignKey(d => d.OriginatingDivisionId)
                .HasConstraintName("FK_Documents_Divisions");
        });

        modelBuilder.Entity<DocumentAssignment>(entity =>
        {
            entity.ToTable("DocumentAssignments", "doc");

            entity.Property(e => e.DocumentAssignmentId).HasColumnName("DocumentAssignmentID");
            entity.Property(e => e.AssignedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DatetimeAssigned).HasColumnType("datetime");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentAssignments)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_DocumentAssignments_Documents");
        });

        modelBuilder.Entity<DocumentCategory>(entity =>
        {
            entity.ToTable("DocumentCategories", "doc");

            entity.HasIndex(e => e.Category, "IX_DocumentCategories").IsUnique();

            entity.Property(e => e.DocumentCategoryId).HasColumnName("DocumentCategoryID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DocumentCopy>(entity =>
        {
            entity.ToTable("DocumentCopies", "doc");

            entity.Property(e => e.DocumentCopyId).HasColumnName("DocumentCopyID");
            entity.Property(e => e.DatetimeSent).HasColumnType("datetime");
            entity.Property(e => e.DivisionId).HasColumnName("DivisionID");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.SentBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Division).WithMany(p => p.DocumentCopies)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentCopies_Divisions");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentCopies)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_DocumentCopies_Documents");
        });

        modelBuilder.Entity<DocumentCorrespondent>(entity =>
        {
            entity.ToTable("DocumentCorrespondents", "doc");

            entity.Property(e => e.DocumentCorrespondentId).HasColumnName("DocumentCorrespondentID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CorrespondentCategoryId).HasColumnName("CorrespondentCategoryID");
            entity.Property(e => e.CorrespondentId).HasColumnName("CorrespondentID");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");

            entity.HasOne(d => d.Company).WithMany(p => p.DocumentCorrespondents)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_DocumentCorrespondents_Companies");

            entity.HasOne(d => d.CorrespondentCategory).WithMany(p => p.DocumentCorrespondents)
                .HasForeignKey(d => d.CorrespondentCategoryId)
                .HasConstraintName("FK_DocumentCorrespondents_CorrespondentCategories");

            entity.HasOne(d => d.Correspondent).WithMany(p => p.DocumentCorrespondents)
                .HasForeignKey(d => d.CorrespondentId)
                .HasConstraintName("FK_DocumentCorrespondents_Correspondents");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentCorrespondents)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_DocumentCorrespondents_Documents");
        });

        modelBuilder.Entity<DocumentFilingType>(entity =>
        {
            entity.ToTable("DocumentFilingTypes", "doc");

            entity.HasIndex(e => e.FillingType, "IX_DocumentFilingTypes").IsUnique();

            entity.Property(e => e.DocumentFilingTypeId).HasColumnName("DocumentFilingTypeID");
            entity.Property(e => e.FillingType)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DocumentForward>(entity =>
        {
            entity.ToTable("DocumentForwards", "doc");

            entity.Property(e => e.DocumentForwardId).HasColumnName("DocumentForwardID");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DatetimeApproved).HasColumnType("datetime");
            entity.Property(e => e.DatetimeForwarded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DivisionId).HasColumnName("DivisionID");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.ForwardedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FromDivisionId).HasColumnName("FromDivisionID");
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.Division).WithMany(p => p.DocumentForwardDivisions)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentForwards_Divisions");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentForwards)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_DocumentForwards_Documents");

            entity.HasOne(d => d.FromDivision).WithMany(p => p.DocumentForwardFromDivisions)
                .HasForeignKey(d => d.FromDivisionId)
                .HasConstraintName("FK_DocumentForwards_Divisions1");

            entity.HasMany(d => d.NeededActions).WithMany(p => p.DocumentForwards)
                .UsingEntity<Dictionary<string, object>>(
                    "DocumentForwardNeededAction",
                    r => r.HasOne<NeededAction>().WithMany()
                        .HasForeignKey("NeededActionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DocumentForwardNeededActions_NeededActions"),
                    l => l.HasOne<DocumentForward>().WithMany()
                        .HasForeignKey("DocumentForwardId")
                        .HasConstraintName("FK_DocumentForwardNeededActions_DocumentForwards"),
                    j =>
                    {
                        j.HasKey("DocumentForwardId", "NeededActionId");
                        j.ToTable("DocumentForwardNeededActions", "doc");
                        j.IndexerProperty<long>("DocumentForwardId").HasColumnName("DocumentForwardID");
                        j.IndexerProperty<long>("NeededActionId").HasColumnName("NeededActionID");
                    });
        });

        modelBuilder.Entity<DocumentLocation>(entity =>
        {
            entity.ToTable("DocumentLocations", "doc");

            entity.Property(e => e.DocumentLocationId).HasColumnName("DocumentLocationID");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DatetimeApproved).HasColumnType("datetime");
            entity.Property(e => e.DatetimeReceived).HasColumnType("datetime");
            entity.Property(e => e.DivisionId).HasColumnName("DivisionID");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.ReceivedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Division).WithMany(p => p.DocumentLocations)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentLocations_Divisions");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentLocations)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_DocumentLocations_Documents");
        });

        modelBuilder.Entity<DocumentLog>(entity =>
        {
            entity.ToTable("DocumentLogs", "doc");

            entity.Property(e => e.DocumentLogId).HasColumnName("DocumentLogID");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.EventLog)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EventLogDescription)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.LogDatetime).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentLogs)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_DocumentLogs_Documents");
        });

        modelBuilder.Entity<DocumentReply>(entity =>
        {
            entity.ToTable("DocumentReplies", "doc");

            entity.Property(e => e.DocumentReplyId).HasColumnName("DocumentReplyID");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.RepliedDocumentId).HasColumnName("RepliedDocumentID");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentReplyDocuments)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DocumentReplies_Documents");

            entity.HasOne(d => d.RepliedDocument).WithMany(p => p.DocumentReplyRepliedDocuments)
                .HasForeignKey(d => d.RepliedDocumentId)
                .HasConstraintName("FK_DocumentReplies_Documents1");
        });

        modelBuilder.Entity<DocumentType>(entity =>
        {
            entity.ToTable("DocumentTypes", "doc");

            entity.HasIndex(e => new { e.TypeDescription, e.DocumentFilingTypeId }, "IX_DocumentTypes").IsUnique();

            entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            entity.Property(e => e.DocumentFilingTypeId).HasColumnName("DocumentFilingTypeID");
            entity.Property(e => e.IsInternal).HasColumnName("isInternal");
            entity.Property(e => e.TypeContents)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TypeDescription)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.DocumentFilingType).WithMany(p => p.DocumentTypes)
                .HasForeignKey(d => d.DocumentFilingTypeId)
                .HasConstraintName("FK_DocumentTypes_DocumentFilingTypes");
        });

        modelBuilder.Entity<DocumentVersion>(entity =>
        {
            entity.ToTable("DocumentVersions", "doc");

            entity.Property(e => e.DocumentVersionId).HasColumnName("DocumentVersionID");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.FileExtension)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.FileVersion)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDatetime).HasColumnType("datetime");
            entity.Property(e => e.SharepointLocation)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UploadedBy)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UploadedDatetime).HasColumnType("datetime");
            entity.Property(e => e.VersionStatusId).HasColumnName("VersionStatusID");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentVersions)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_DocumentVersions_Documents");

            entity.HasOne(d => d.VersionStatus).WithMany(p => p.DocumentVersions)
                .HasForeignKey(d => d.VersionStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentVersions_VersionStatuses");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees", "HR");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.BloodType)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Citizenship)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CivilStatusId).HasColumnName("CivilStatusID");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Firstname)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.GenderId).HasColumnName("GenderID");
            entity.Property(e => e.Gsisid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("GSISID");
            entity.Property(e => e.Kgweight)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("KGWeight");
            entity.Property(e => e.Lastname)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameExtension)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Pagibigid)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PAGIBIGID");
            entity.Property(e => e.PermanentAreaId).HasColumnName("PermanentAreaID");
            entity.Property(e => e.PermanentStreetAddress)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PhilhealthNo)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ResidentialAreaId).HasColumnName("ResidentialAreaID");
            entity.Property(e => e.ResidentialStreetAddress)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Salutation)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Sssno)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SSSNo");
            entity.Property(e => e.TelephoneNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tin)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("TIN");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.CivilStatus).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CivilStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_CivilStatuses");

            entity.HasOne(d => d.Gender).WithMany(p => p.Employees)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employees_Genders");

            entity.HasOne(d => d.PermanentArea).WithMany(p => p.EmployeePermanentAreas)
                .HasForeignKey(d => d.PermanentAreaId)
                .HasConstraintName("FK_Employees_Areas");

            entity.HasOne(d => d.ResidentialArea).WithMany(p => p.EmployeeResidentialAreas)
                .HasForeignKey(d => d.ResidentialAreaId)
                .HasConstraintName("FK_Employees_Areas1");
        });

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.HasKey(e => e.EntityId).HasName("PK_entities");

            entity.ToTable("Entities", "entities");

            entity.HasIndex(e => new { e.Acronym, e.EntityName }, "IX_Entities").IsUnique();

            entity.Property(e => e.EntityId).HasColumnName("EntityID");
            entity.Property(e => e.Acronym)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.EntityCategoryId).HasColumnName("EntityCategoryID");
            entity.Property(e => e.EntityCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntityName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.StreetAddress)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.Entities)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_entities_areas");

            entity.HasOne(d => d.EntityCategory).WithMany(p => p.Entities)
                .HasForeignKey(d => d.EntityCategoryId)
                .HasConstraintName("FK_entities_entity_categories");
        });

        modelBuilder.Entity<EntityCategory>(entity =>
        {
            entity.HasKey(e => e.EntityCategoryId).HasName("PK_entity_categories");

            entity.ToTable("EntityCategories", "entities");

            entity.HasIndex(e => new { e.Ecategory, e.SectorId }, "IX_EntityCategories").IsUnique();

            entity.Property(e => e.EntityCategoryId).HasColumnName("EntityCategoryID");
            entity.Property(e => e.Ecategory)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("ECategory");
            entity.Property(e => e.SectorId).HasColumnName("SectorID");

            entity.HasOne(d => d.Sector).WithMany(p => p.EntityCategories)
                .HasForeignKey(d => d.SectorId)
                .HasConstraintName("FK_entity_categories_sectors");
        });

        modelBuilder.Entity<Erccase>(entity =>
        {
            entity.HasKey(e => e.ErccaseId).HasName("PK_Cases");

            entity.ToTable("ERCCases", "cases");

            entity.HasIndex(e => e.CaseNo, "IX_ERCCases").IsUnique();

            entity.Property(e => e.ErccaseId).HasColumnName("ERCCaseID");
            entity.Property(e => e.ActualFaissuance).HasColumnName("ActualFAIssuance");
            entity.Property(e => e.ActualPaissuance).HasColumnName("ActualPAIssuance");
            entity.Property(e => e.AmountClaimed).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AmountSettled).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CaseBoxLocation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CaseCategoryId).HasColumnName("CaseCategoryID");
            entity.Property(e => e.CaseNatureId).HasColumnName("CaseNatureID");
            entity.Property(e => e.CaseNo)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.CaseStatusId).HasColumnName("CaseStatusID");
            entity.Property(e => e.DatetimeApproved).HasColumnType("datetime");
            entity.Property(e => e.DocketedBy)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FadeliberationDate).HasColumnName("FADeliberationDate");
            entity.Property(e => e.FatargetOrder).HasColumnName("FATargetOrder");
            entity.Property(e => e.IsArchived)
                .HasDefaultValue(false)
                .HasColumnName("isArchived");
            entity.Property(e => e.MeterSin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MeterSIN");
            entity.Property(e => e.PadeliberationDate).HasColumnName("PADeliberationDate");
            entity.Property(e => e.PatargetOrder).HasColumnName("PATargetOrder");
            entity.Property(e => e.PrayedForPa).HasColumnName("PrayedForPA");
            entity.Property(e => e.Synopsis)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.TargetFaissuance).HasColumnName("TargetFAIssuance");
            entity.Property(e => e.TargetPaissuance).HasColumnName("TargetPAIssuance");
            entity.Property(e => e.Title)
                .HasMaxLength(5000)
                .IsUnicode(false);

            entity.HasOne(d => d.CaseCategory).WithMany(p => p.Erccases)
                .HasForeignKey(d => d.CaseCategoryId)
                .HasConstraintName("FK_Cases_CaseCategories");

            entity.HasOne(d => d.CaseNature).WithMany(p => p.Erccases)
                .HasForeignKey(d => d.CaseNatureId)
                .HasConstraintName("FK_ERCCases_CaseNatures");

            entity.HasOne(d => d.CaseStatus).WithMany(p => p.Erccases)
                .HasForeignKey(d => d.CaseStatusId)
                .HasConstraintName("FK_Cases_CaseStatuses");
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.ToTable("ErrorLogs", "admin");

            entity.Property(e => e.ErrorLogId).HasColumnName("ErrorLogID");
            entity.Property(e => e.ErrorDatetime).HasColumnType("datetime");
            entity.Property(e => e.InvokedBy)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.Source).HasMaxLength(500);
            entity.Property(e => e.StackTrace).HasMaxLength(1000);
        });

        modelBuilder.Entity<EventLog>(entity =>
        {
            entity.ToTable("EventLogs", "DS");

            entity.Property(e => e.EventLogId).HasColumnName("EventLogID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Event)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.EventDatetime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Source)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<ExternalCase>(entity =>
        {
            entity.ToTable("ExternalCases", "cases");

            entity.HasIndex(e => e.CaseRefNo, "IX_ExternalCases").IsUnique();

            entity.Property(e => e.ExternalCaseId).HasColumnName("ExternalCaseID");
            entity.Property(e => e.CaseRefNo)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ExternalCaseCategoryId).HasColumnName("ExternalCaseCategoryID");
            entity.Property(e => e.ExternalCaseStatusId).HasColumnName("ExternalCaseStatusID");
            entity.Property(e => e.ParentCaseId).HasColumnName("ParentCaseID");
            entity.Property(e => e.SubjectMatter)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.ExternalCaseCategory).WithMany(p => p.ExternalCases)
                .HasForeignKey(d => d.ExternalCaseCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExternalCases_ExternalCaseCategories");

            entity.HasOne(d => d.ExternalCaseStatus).WithMany(p => p.ExternalCases)
                .HasForeignKey(d => d.ExternalCaseStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExternalCases_ExternalCaseStatuses");

            entity.HasOne(d => d.ParentCase).WithMany(p => p.InverseParentCase)
                .HasForeignKey(d => d.ParentCaseId)
                .HasConstraintName("FK_ExternalCases_ExternalCases");

            entity.HasMany(d => d.Erccases).WithMany(p => p.ExternalCases)
                .UsingEntity<Dictionary<string, object>>(
                    "ExternalCaseLink",
                    r => r.HasOne<Erccase>().WithMany()
                        .HasForeignKey("ErccaseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ExternalCaseLinks_ERCCases"),
                    l => l.HasOne<ExternalCase>().WithMany()
                        .HasForeignKey("ExternalCaseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ExternalCaseLinks_ExternalCases"),
                    j =>
                    {
                        j.HasKey("ExternalCaseId", "ErccaseId");
                        j.ToTable("ExternalCaseLinks", "cases");
                        j.IndexerProperty<long>("ExternalCaseId").HasColumnName("ExternalCaseID");
                        j.IndexerProperty<long>("ErccaseId").HasColumnName("ERCCaseID");
                    });
        });

        modelBuilder.Entity<ExternalCaseCategory>(entity =>
        {
            entity.ToTable("ExternalCaseCategories", "cases");

            entity.Property(e => e.ExternalCaseCategoryId).HasColumnName("ExternalCaseCategoryID");
            entity.Property(e => e.CaseCategory)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ExternalCaseDocument>(entity =>
        {
            entity.ToTable("ExternalCaseDocuments", "cases");

            entity.Property(e => e.ExternalCaseDocumentId).HasColumnName("ExternalCaseDocumentID");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.DocumentRefNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DocumentTitle)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ExternalCaseId).HasColumnName("ExternalCaseID");
            entity.Property(e => e.Pleadings)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Resolution)
                .HasMaxLength(1000)
                .IsUnicode(false);

            entity.HasOne(d => d.Document).WithMany(p => p.ExternalCaseDocuments)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_ExternalCaseDocuments_Documents");

            entity.HasOne(d => d.ExternalCase).WithMany(p => p.ExternalCaseDocuments)
                .HasForeignKey(d => d.ExternalCaseId)
                .HasConstraintName("FK_ExternalCaseDocuments_ExternalCases");
        });

        modelBuilder.Entity<ExternalCaseStatus>(entity =>
        {
            entity.ToTable("ExternalCaseStatuses", "cases");

            entity.Property(e => e.ExternalCaseStatusId).HasColumnName("ExternalCaseStatusID");
            entity.Property(e => e.CaseStatus)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.ToTable("Genders", "HR");

            entity.Property(e => e.GenderId).HasColumnName("GenderID");
            entity.Property(e => e.GenderValue)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Grid>(entity =>
        {
            entity.HasKey(e => e.GridId).HasName("PK_grids");

            entity.ToTable("Grids", "locations");

            entity.HasIndex(e => e.GridName, "IX_Grids").IsUnique();

            entity.Property(e => e.GridId).HasColumnName("GridID");
            entity.Property(e => e.GridName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HandlingOfficerType>(entity =>
        {
            entity.ToTable("HandlingOfficerTypes", "cases");

            entity.HasIndex(e => e.OfficerType, "IX_HandlingOfficerTypes").IsUnique();

            entity.Property(e => e.HandlingOfficerTypeId).HasColumnName("HandlingOfficerTypeID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.OfficerType)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Hearing>(entity =>
        {
            entity.ToTable("Hearings", "cases");

            entity.HasIndex(e => new { e.HearingDate, e.Time, e.HearingVenueId, e.OtherVenue }, "IX_Hearings").IsUnique();

            entity.Property(e => e.HearingId).HasColumnName("HearingID");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DatetimeApproved).HasColumnType("datetime");
            entity.Property(e => e.ErccaseId).HasColumnName("ERCCaseID");
            entity.Property(e => e.HearingCategoryId).HasColumnName("HearingCategoryID");
            entity.Property(e => e.HearingVenueId).HasColumnName("HearingVenueID");
            entity.Property(e => e.OtherVenue)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Time)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.Property(e => e.HearingLinks)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Erccase).WithMany(p => p.Hearings)
                .HasForeignKey(d => d.ErccaseId)
                .HasConstraintName("FK_Hearings_Cases");

            entity.HasOne(d => d.HearingCategory).WithMany(p => p.Hearings)
                .HasForeignKey(d => d.HearingCategoryId)
                .HasConstraintName("FK_Hearings_HearingCategories");

            entity.HasOne(d => d.HearingVenue).WithMany(p => p.Hearings)
                .HasForeignKey(d => d.HearingVenueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hearings_HearingVenues");

            // ALDRIN: Gumawa ako bagong model builder dito (Line 1813)
            //entity.HasMany(d => d.HearingTypes).WithMany(p => p.Hearings)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "HearingsInHearingType",
            //        r => r.HasOne<HearingType>().WithMany()
            //            .HasForeignKey("HearingTypeId")
            //            .HasConstraintName("FK_HearingsInHearingType_HearingTypes"),
            //        l => l.HasOne<Hearing>().WithMany()
            //            .HasForeignKey("HearingId")
            //            .HasConstraintName("FK_HearingsInHearingType_Hearings"),
            //        j =>
            //        {
            //            j.HasKey("HearingId", "HearingTypeId");
            //            j.ToTable("HearingsInHearingType", "cases");
            //            j.IndexerProperty<long>("HearingId").HasColumnName("HearingID");
            //            j.IndexerProperty<long>("HearingTypeId").HasColumnName("HearingTypeID");
            //        });
        });

        modelBuilder.Entity<HearingsInHearingType>(entity =>
        {
            entity.ToTable("HearingsInHearingType", "cases");

            entity.HasKey(h => new { h.HearingId, h.HearingTypeId });

            entity.HasOne(h => h.Hearing)
                .WithMany(h => h.HearingsInHearingTypes)
                .HasForeignKey(h => h.HearingId);

            entity.HasOne(h => h.HearingType)
                .WithMany(ht => ht.HearingsInHearingTypes)
                .HasForeignKey(h => h.HearingTypeId);
        });

        modelBuilder.Entity<HearingCategory>(entity =>
        {
            entity.ToTable("HearingCategories", "cases");

            entity.HasIndex(e => e.Category, "IX_HearingCategories").IsUnique();

            entity.Property(e => e.HearingCategoryId).HasColumnName("HearingCategoryID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HearingOfficer>(entity =>
        {
            entity.ToTable("HearingOfficers", "cases");

            entity.Property(e => e.HearingOfficerId).HasColumnName("HearingOfficerID");
            entity.Property(e => e.HearingId).HasColumnName("HearingID");
            entity.Property(e => e.HearingOfficerTypeId).HasColumnName("HearingOfficerTypeID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Hearing).WithMany(p => p.HearingOfficers)
                .HasForeignKey(d => d.HearingId)
                .HasConstraintName("FK_HearingOfficers_Hearings");

            entity.HasOne(d => d.HearingOfficerType).WithMany(p => p.HearingOfficers)
                .HasForeignKey(d => d.HearingOfficerTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HearingOfficers_HearingOfficerTypes");
        });

        modelBuilder.Entity<HearingOfficerType>(entity =>
        {
            entity.ToTable("HearingOfficerTypes", "cases");

            entity.Property(e => e.HearingOfficerTypeId).HasColumnName("HearingOfficerTypeID");
            entity.Property(e => e.OfficerType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HearingType>(entity =>
        {
            entity.ToTable("HearingTypes", "cases");

            entity.HasIndex(e => e.TypeOfHearing, "IX_HearingTypes").IsUnique();

            entity.Property(e => e.HearingTypeId).HasColumnName("HearingTypeID");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TypeOfHearing)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HearingVenue>(entity =>
        {
            entity.ToTable("HearingVenues", "cases");

            entity.HasIndex(e => new { e.VenueName, e.StreetAddress, e.CityId }, "IX_HearingVenues").IsUnique();

            entity.Property(e => e.HearingVenueId).HasColumnName("HearingVenueID");
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.StreetAddress)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.VenueName)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.City).WithMany(p => p.HearingVenues)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_HearingVenues_Cities");
        });

        modelBuilder.Entity<InternalDocument>(entity =>
        {
            entity.HasKey(e => e.InternalDocumentId).HasName("PK_InternalDocument2s");

            entity.ToTable("InternalDocuments", "doc");

            entity.Property(e => e.InternalDocumentId).HasColumnName("InternalDocumentID");
            entity.Property(e => e.DeliveryToId).HasColumnName("DeliveryToID");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.InDatetime).HasColumnType("datetime");
            entity.Property(e => e.OutDatetime).HasColumnType("datetime");
            entity.Property(e => e.Particular)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RecievedFromId).HasColumnName("RecievedFromID");
            entity.Property(e => e.RefNo)
                .HasMaxLength(19)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

            entity.HasOne(d => d.DeliveryTo).WithMany(p => p.InternalDocumentDeliveryTos)
                .HasForeignKey(d => d.DeliveryToId)
                .HasConstraintName("FK_InternalDocuments_Divisions1");

            entity.HasOne(d => d.Document).WithMany(p => p.InternalDocuments)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_InternalDocuments_Documents");

            entity.HasOne(d => d.RecievedFrom).WithMany(p => p.InternalDocumentRecievedFroms)
                .HasForeignKey(d => d.RecievedFromId)
                .HasConstraintName("FK_InternalDocuments_Divisions");

            entity.HasOne(d => d.Service).WithMany(p => p.InternalDocuments)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InternalDocuments_Services");
        });

        modelBuilder.Entity<LogType>(entity =>
        {
            entity.ToTable("LogTypes", "admin");

            entity.HasIndex(e => e.TypeOfLog, "IX_LogTypes").IsUnique();

            entity.Property(e => e.LogTypeId).HasColumnName("LogTypeID");
            entity.Property(e => e.TypeOfLog)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MigrationHistory>(entity =>
        {
            entity.HasKey(e => new { e.MigrationId, e.ContextKey }).HasName("PK_dbo.__MigrationHistory");

            entity.ToTable("__MigrationHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ContextKey).HasMaxLength(300);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<MilestonesAchieved>(entity =>
        {
            entity.HasKey(e => e.MilestoneAchievedId);

            entity.ToTable("MilestonesAchieved", "cases");

            entity.HasIndex(e => new { e.CaseMilestoneId, e.DatetimeAchieved, e.ErccaseId }, "IX_MilestonesAchieved");

            entity.Property(e => e.MilestoneAchievedId).HasColumnName("MilestoneAchievedID");
            entity.Property(e => e.CaseMilestoneId).HasColumnName("CaseMilestoneID");
            entity.Property(e => e.DatetimeAchieved)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ErccaseId).HasColumnName("ERCCaseID");
            entity.Property(e => e.PercentAchieved).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CaseMilestone).WithMany(p => p.MilestonesAchieveds)
                .HasForeignKey(d => d.CaseMilestoneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MilestonesAchieved_CaseMilestones");

            entity.HasOne(d => d.Erccase).WithMany(p => p.MilestonesAchieveds)
                .HasForeignKey(d => d.ErccaseId)
                .HasConstraintName("FK_MilestonesAchieved_ERCCases");
        });

        modelBuilder.Entity<NeededAction>(entity =>
        {
            entity.ToTable("NeededActions", "doc");

            entity.HasIndex(e => e.Action, "IX_NeededActions").IsUnique();

            entity.Property(e => e.NeededActionId).HasColumnName("NeededActionID");
            entity.Property(e => e.Action)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.DocumentAssignments).WithMany(p => p.NeededActions)
                .UsingEntity<Dictionary<string, object>>(
                    "DocumentAssignmentNeededAction",
                    r => r.HasOne<DocumentAssignment>().WithMany()
                        .HasForeignKey("DocumentAssignmentId")
                        .HasConstraintName("FK_DocumentNeededActions_DocumentAssignments"),
                    l => l.HasOne<NeededAction>().WithMany()
                        .HasForeignKey("NeededActionId")
                        .HasConstraintName("FK_DocumentNeededActions_NeededActions"),
                    j =>
                    {
                        j.HasKey("NeededActionId", "DocumentAssignmentId").HasName("PK_DocumentNeededActions");
                        j.ToTable("DocumentAssignmentNeededActions", "doc");
                        j.IndexerProperty<long>("NeededActionId").HasColumnName("NeededActionID");
                        j.IndexerProperty<long>("DocumentAssignmentId").HasColumnName("DocumentAssignmentID");
                    });
        });

        modelBuilder.Entity<OcNextBarcodeNo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("OC_NextBarcodeNo", "doc");

            entity.Property(e => e.BarcodeNo)
                .HasMaxLength(19)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.ToTable("Offices", "HR");

            entity.HasIndex(e => e.OfficeName, "IX_Offices").IsUnique();

            entity.Property(e => e.OfficeId).HasColumnName("OfficeID");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.OfficeName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OutgoingRecipient>(entity =>
        {
            entity.ToTable("OutgoingRecipients", "doc");

            entity.Property(e => e.OutgoingRecipientId).HasColumnName("OutgoingRecipientID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CorrespondentId).HasColumnName("CorrespondentID");
            entity.Property(e => e.DeliveryMethodId).HasColumnName("DeliveryMethodID");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");

            entity.HasOne(d => d.Company).WithMany(p => p.OutgoingRecipients)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_OutgoingRecipients_Companies");

            entity.HasOne(d => d.Correspondent).WithMany(p => p.OutgoingRecipients)
                .HasForeignKey(d => d.CorrespondentId)
                .HasConstraintName("FK_OutgoingRecipients_Correspondents");

            entity.HasOne(d => d.DeliveryMethod).WithMany(p => p.OutgoingRecipients)
                .HasForeignKey(d => d.DeliveryMethodId)
                .HasConstraintName("FK_OutgoingRecipients_DeliveryMethods");

            entity.HasOne(d => d.Document).WithMany(p => p.OutgoingRecipients)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_OutgoingRecipients_Documents");
        });

        modelBuilder.Entity<PaymentDetail>(entity =>
        {
            entity.HasKey(e => e.PaymentDetailId).HasName("PK_PaymentBreakdowns");

            entity.ToTable("PaymentDetails", "DS");

            entity.Property(e => e.PaymentDetailId).HasColumnName("PaymentDetailID");
            entity.Property(e => e.AccountPayableId).HasColumnName("AccountPayableID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Particular).IsUnicode(false);

            entity.HasOne(d => d.AccountPayable).WithMany(p => p.PaymentDetails)
                .HasForeignKey(d => d.AccountPayableId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PaymentDetails_AccountPayables");
        });

        modelBuilder.Entity<PreFilingAssessment>(entity =>
        {
            entity.ToTable("PreFilingAssessments", "cases");

            entity.HasIndex(e => e.AssessmentNo, "IX_PreFilingAssessments").IsUnique();

            entity.Property(e => e.PreFilingAssessmentId).HasColumnName("PreFilingAssessmentID");
            entity.Property(e => e.AssessedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AssessmentNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DatetimeAssessed).HasColumnType("datetime");
            entity.Property(e => e.PreFilingCategoryId).HasColumnName("PreFilingCategoryID");
            entity.Property(e => e.PreFilingStatusId).HasColumnName("PreFilingStatusID");
            entity.Property(e => e.Remarks)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.SubjectMatter)
                .HasMaxLength(2000)
                .IsUnicode(false);

            entity.HasOne(d => d.PreFilingCategory).WithMany(p => p.PreFilingAssessments)
                .HasForeignKey(d => d.PreFilingCategoryId)
                .HasConstraintName("FK_PreFilingAssessments_PreFilingCategories");

            entity.HasOne(d => d.PreFilingStatus).WithMany(p => p.PreFilingAssessments)
                .HasForeignKey(d => d.PreFilingStatusId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PreFilingAssessments_PreFilingStatuses");
        });

        modelBuilder.Entity<PreFilingCategory>(entity =>
        {
            entity.ToTable("PreFilingCategories", "cases");

            entity.Property(e => e.PreFilingCategoryId).HasColumnName("PreFilingCategoryID");
            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PreFilingCorrespondent>(entity =>
        {
            entity.ToTable("PreFilingCorrespondents", "cases");

            entity.Property(e => e.PreFilingCorrespondentId).HasColumnName("PreFilingCorrespondentID");
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CorrespondentId).HasColumnName("CorrespondentID");
            entity.Property(e => e.PreFilingAssessmentId).HasColumnName("PreFilingAssessmentID");

            entity.HasOne(d => d.Company).WithMany(p => p.PreFilingCorrespondents)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_PreFilingCorrespondents_Companies");

            entity.HasOne(d => d.Correspondent).WithMany(p => p.PreFilingCorrespondents)
                .HasForeignKey(d => d.CorrespondentId)
                .HasConstraintName("FK_PreFilingCorrespondents_Correspondents");

            entity.HasOne(d => d.PreFilingAssessment).WithMany(p => p.PreFilingCorrespondents)
                .HasForeignKey(d => d.PreFilingAssessmentId)
                .HasConstraintName("FK_PreFilingCorrespondents_PreFilingAssessments");
        });

        modelBuilder.Entity<PreFilingLog>(entity =>
        {
            entity.ToTable("PreFilingLogs", "cases");

            entity.Property(e => e.PreFilingLogId).HasColumnName("PreFilingLogID");
            entity.Property(e => e.ActionTaken)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.LogDatetime).HasColumnType("datetime");
            entity.Property(e => e.PreFilingAssessmentId).HasColumnName("PreFilingAssessmentID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.PreFilingAssessment).WithMany(p => p.PreFilingLogs)
                .HasForeignKey(d => d.PreFilingAssessmentId)
                .HasConstraintName("FK_PreFilingLogs_PreFilingAssessments");
        });

        modelBuilder.Entity<PreFilingStatus>(entity =>
        {
            entity.ToTable("PreFilingStatuses", "cases");

            entity.Property(e => e.PreFilingStatusId).HasColumnName("PreFilingStatusID");
            entity.Property(e => e.Status)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasKey(e => e.ProvinceId).HasName("PK_provinces");

            entity.ToTable("Provinces", "locations");

            entity.HasIndex(e => new { e.ProvinceName, e.RegionId }, "IX_Provinces").IsUnique();

            entity.Property(e => e.ProvinceId).HasColumnName("ProvinceID");
            entity.Property(e => e.ProvinceName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.RegionId).HasColumnName("RegionID");

            entity.HasOne(d => d.Region).WithMany(p => p.Provinces)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK_provinces_regions");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId).HasName("PK_regions");

            entity.ToTable("Regions", "locations");

            entity.HasIndex(e => new { e.RegionName, e.GridId }, "IX_Regions").IsUnique();

            entity.Property(e => e.RegionId).HasColumnName("RegionID");
            entity.Property(e => e.GridId).HasColumnName("GridID");
            entity.Property(e => e.RegionName)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Grid).WithMany(p => p.Regions)
                .HasForeignKey(d => d.GridId)
                .HasConstraintName("FK_regions_grids");
        });

        modelBuilder.Entity<RelatedCase>(entity =>
        {
            entity.ToTable("RelatedCases", "cases");

            entity.HasIndex(e => new { e.ErccaseId, e.ErccaseRelatedId }, "IX_RelatedCases").IsUnique();

            entity.Property(e => e.RelatedCaseId).HasColumnName("RelatedCaseID");
            entity.Property(e => e.DatetimeRelated).HasColumnType("datetime");
            entity.Property(e => e.ErccaseId).HasColumnName("ERCCaseID");
            entity.Property(e => e.ErccaseRelatedId).HasColumnName("ERCCaseRelatedID");
            entity.Property(e => e.RelatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Erccase).WithMany(p => p.RelatedCaseErccases)
                .HasForeignKey(d => d.ErccaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RelatedCases_ERCCases");

            entity.HasOne(d => d.ErccaseRelated).WithMany(p => p.RelatedCaseErccaseRelateds)
                .HasForeignKey(d => d.ErccaseRelatedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RelatedCases_ERCCases1");
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.HasKey(e => e.SectorId).HasName("PK_sectors");

            entity.ToTable("Sectors", "entities");

            entity.HasIndex(e => e.SectorName, "IX_Sectors").IsUnique();

            entity.Property(e => e.SectorId).HasColumnName("SectorID");
            entity.Property(e => e.SectorName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable("Services", "HR");

            entity.HasIndex(e => e.ServiceName, "IX_Services").IsUnique();

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.OfficeId).HasColumnName("OfficeID");
            entity.Property(e => e.Secretary)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ServiceName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Office).WithMany(p => p.Services)
                .HasForeignKey(d => d.OfficeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Services_Offices");
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.ToTable("Settings", "DS");

            entity.Property(e => e.SettingId).HasColumnName("SettingID");
            entity.Property(e => e.SettingName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.SettingValue)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SharepointMapping>(entity =>
        {
            entity.HasKey(e => e.SharpointMappingId);

            entity.ToTable("SharepointMappings", "doc");

            entity.Property(e => e.SharpointMappingId).HasColumnName("SharpointMappingID");
            entity.Property(e => e.DivisionId).HasColumnName("DivisionID");
            entity.Property(e => e.SharepointLocation)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.Division).WithMany(p => p.SharepointMappings)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SharepointMappings_Divisions");
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("States", "locations");

            entity.Property(e => e.StateId).HasColumnName("StateID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.StateName)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.Country).WithMany(p => p.States)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_States_Countries");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK_DocumentTypePerDivisions");

            entity.ToTable("Subscriptions", "doc");

            entity.Property(e => e.SubscriptionId).HasColumnName("SubscriptionID");
            entity.Property(e => e.DivisionId).HasColumnName("DivisionID");
            entity.Property(e => e.DocumentTypeId).HasColumnName("DocumentTypeID");
            entity.Property(e => e.EnableNotification).HasDefaultValue(true);

            entity.HasOne(d => d.Division).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.DivisionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentTypePerDivisions_Divisions");

            entity.HasOne(d => d.DocumentType).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.DocumentTypeId)
                .HasConstraintName("FK_DocumentTypePerDivisions_DocumentTypes");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07823061C7");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4C72B1D4C").IsUnique();

            entity.HasAlternateKey(e => e.Username)
              .HasName("AK_Users_Username");

            entity.Property(e => e.Department).HasMaxLength(150);
            entity.Property(e => e.Designation).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Fullname).HasMaxLength(255);
            entity.Property(e => e.IsArchive).HasDefaultValue(false);
            entity.Property(e => e.IsEmailNotif).HasDefaultValue(false);
            entity.Property(e => e.IsSystemNotif).HasDefaultValue(false);
            entity.Property(e => e.UserCategory).HasMaxLength(100);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<UserAction>(entity =>
        {
            entity.ToTable("UserActions", "doc");

            entity.Property(e => e.UserActionId).HasColumnName("UserActionID");
            entity.Property(e => e.ActionTaken)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ActionTypeId).HasColumnName("ActionTypeID");
            entity.Property(e => e.ApprovedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DatetimeApproved).HasColumnType("datetime");
            entity.Property(e => e.DatetimeCreated).HasColumnType("datetime");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.ActionType).WithMany(p => p.UserActions)
                .HasForeignKey(d => d.ActionTypeId)
                .HasConstraintName("FK_UserActions_ActionTypes");

            entity.HasOne(d => d.Document).WithMany(p => p.UserActions)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_UserActions_Documents");
        });

        modelBuilder.Entity<UserComment>(entity =>
        {
            entity.ToTable("UserComments", "doc");

            entity.Property(e => e.UserCommentId).HasColumnName("UserCommentID");
            entity.Property(e => e.Comment)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DatetimeCreated).HasColumnType("datetime");
            entity.Property(e => e.DocumentId).HasColumnName("DocumentID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Document).WithMany(p => p.UserComments)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_UserComments_Documents");
        });

        modelBuilder.Entity<UserLog>(entity =>
        {
            entity.ToTable("UserLogs", "admin");

            entity.Property(e => e.UserLogId).HasColumnName("UserLogID");
            entity.Property(e => e.LogDatetime).HasColumnType("datetime");
            entity.Property(e => e.LogMessage)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.LogTypeId).HasColumnName("LogTypeID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserID");

            entity.HasOne(d => d.LogType).WithMany(p => p.UserLogs)
                .HasForeignKey(d => d.LogTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLogs_LogTypes");
        });

        modelBuilder.Entity<UserTokenCache>(entity =>
        {
            entity.HasKey(e => e.UserTokenCacheId).HasName("PK_dbo.UserTokenCaches");

            entity.Property(e => e.CacheBits).HasColumnName("cacheBits");
            entity.Property(e => e.LastWrite).HasColumnType("datetime");
            entity.Property(e => e.WebUserUniqueId).HasColumnName("webUserUniqueId");
        });

        modelBuilder.Entity<VersionStatus>(entity =>
        {
            entity.ToTable("VersionStatuses", "doc");

            entity.Property(e => e.VersionStatusId).HasColumnName("VersionStatusID");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
