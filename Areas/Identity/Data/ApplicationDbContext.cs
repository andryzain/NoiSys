using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoiSys.Areas.Administrasi.Models;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.Keuangan.Models;
using NoiSys.Areas.MasterData.Models;
using NoiSys.Areas.Order.Models;
using NoiSys.Areas.Penerimaan.Models;
using NoiSys.Areas.Pengiriman.Models;
using NoiSys.Areas.Reimbursment.Models;
using NoiSys.Areas.Transaksi.Models;

namespace NoiSys.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    #region Areas Master Data
    public DbSet<Pengguna> Penggunas { get; set; }
    public DbSet<LevelPengguna> LevelPenggunas { get; set; }
    public DbSet<Principal> Principals { get; set; }
    public DbSet<Satuan> Satuans { get; set; }
    public DbSet<Kategori> Kategoris { get; set; }
    public DbSet<Diskon> Diskons { get; set; }
    public DbSet<MetodePembayaran> MetodePembayarans { get; set; }
    public DbSet<Bengkel> Bengkels { get; set; }
    public DbSet<Mekanik> Mekaniks { get; set; }
    public DbSet<Produk> Produks { get; set; }
    public DbSet<Bank> Banks { get; set; }
    public DbSet<BankCabang> BankCabangs { get; set; }
    public DbSet<ItemReimbursment> ItemReimbursments { get; set; }
    #endregion

    #region Areas Penerimaan
    public DbSet<ReceiveOrder> ReceiveOrders { get; set; }
    public DbSet<ReceiveOrderDetail> ReceiveOrderDetails { get; set; }
    #endregion

    #region Areas Pembelian
    public DbSet<Permintaan> Permintaans { get; set; }
    public DbSet<PermintaanDetail> PermintaanDetails { get; set; }
    public DbSet<Pembelian> Pembelians { get; set; }
    public DbSet<PembelianDetail> PembelianDetails { get; set; }
    public DbSet<PembayaranBarang> PembayaranBarangs { get; set; }
    #endregion

    #region Areas Pengiriman
    public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
    public DbSet<DeliveryOrderDetail> DeliveryOrderDetails { get; set; }
    #endregion

    #region Areas Transaksi
    public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
    public DbSet<PurchaseRequestDetail> PurchaseRequestDetails { get; set; }
    public DbSet<ApprovalPurchaseRequest> ApprovalPurchaseRequests { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
    #endregion

    #region Areas Administrasi
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
    public DbSet<Pembayaran> Pembayarans { get; set; }
    #endregion

    #region Areas Keuangan
    public DbSet<PiutangUsaha> PiutangUsahas { get; set; }
    public DbSet<HutangUsaha> HutangUsahas { get; set; }
    #endregion

    #region Areas Reimbursment
    public DbSet<Pengajuan> Pengajuans { get; set; }
    public DbSet<PengajuanDetail> PengajuanDetails { get; set; }
    public DbSet<Persetujuan> Persetujuans { get; set; }
    public DbSet<PembayaranReimbursment> PembayaranReimbursments { get; set; }
    #endregion



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        //builder.ApplyConfiguration(new ApplicationRoleEntityConfiguration());

        foreach (var foreignKey in builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
        {
            foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.KodePengguna).HasMaxLength(255);
        builder.Property(u => u.NamaLengkap).HasMaxLength(255);
        builder.Property(u => u.LevelId).HasMaxLength(255);
    }
}

//public class ApplicationRoleEntityConfiguration : IEntityTypeConfiguration<ApplicationRole>
//{
//    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
//    {
//        builder.Property(u => u.Module).HasMaxLength(255);
//        builder.Property(u => u.Menu).HasMaxLength(255);
//        builder.Property(u => u.Keterangan).HasMaxLength(255);
//    }
//}