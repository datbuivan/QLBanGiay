using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Nhom3_QLBanGiay.Models;

public partial class QlbanGiayContext : DbContext
{
    public QlbanGiayContext()
    {
    }

    public QlbanGiayContext(DbContextOptions<QlbanGiayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietHoaDonBan> ChiTietHoaDonBans { get; set; }

    public virtual DbSet<ChiTietHoaDonNhap> ChiTietHoaDonNhaps { get; set; }

    public virtual DbSet<ChiTietSanPham> ChiTietSanPhams { get; set; }

    public virtual DbSet<DoiTuongMh> DoiTuongMhs { get; set; }

    public virtual DbSet<HinhAnhSp> HinhAnhSps { get; set; }

    public virtual DbSet<HoaDonBan> HoaDonBans { get; set; }

    public virtual DbSet<HoaDonNhap> HoaDonNhaps { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<KichThuoc> KichThuocs { get; set; }

    public virtual DbSet<LoaiSp> LoaiSps { get; set; }

    public virtual DbSet<MauSac> MauSacs { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-TD4QPLI2\\MAYAO;Initial Catalog=QLBanGiay;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietHoaDonBan>(entity =>
        {
            entity.HasKey(e => new { e.MaHoaDonBan, e.MaSanPham, e.MaMauSac, e.MaKichThuoc }).HasName("PK__ChiTietH__A0173C248CA3C4DA");

            entity.ToTable("ChiTietHoaDonBan");

            entity.Property(e => e.MaHoaDonBan).HasMaxLength(30);
            entity.Property(e => e.MaSanPham).HasMaxLength(30);
            entity.Property(e => e.MaMauSac).HasMaxLength(30);
            entity.Property(e => e.MaKichThuoc).HasMaxLength(30);

            entity.HasOne(d => d.MaHoaDonBanNavigation).WithMany(p => p.ChiTietHoaDonBans)
                .HasForeignKey(d => d.MaHoaDonBan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__MaHoa__3F466844");

            entity.HasOne(d => d.ChiTietSanPham).WithMany(p => p.ChiTietHoaDonBans)
                .HasForeignKey(d => new { d.MaSanPham, d.MaMauSac, d.MaKichThuoc })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHoaDonBan__403A8C7D");
        });

        modelBuilder.Entity<ChiTietHoaDonNhap>(entity =>
        {
            entity.HasKey(e => new { e.MaHoaDonNhap, e.MaSanPham, e.MaMauSac, e.MaKichThuoc }).HasName("PK__ChiTietH__8ECFCE1B36290FCD");

            entity.ToTable("ChiTietHoaDonNhap");

            entity.Property(e => e.MaHoaDonNhap).HasMaxLength(30);
            entity.Property(e => e.MaSanPham).HasMaxLength(30);
            entity.Property(e => e.MaMauSac).HasMaxLength(30);
            entity.Property(e => e.MaKichThuoc).HasMaxLength(30);

            entity.HasOne(d => d.MaHoaDonNhapNavigation).WithMany(p => p.ChiTietHoaDonNhaps)
                .HasForeignKey(d => d.MaHoaDonNhap)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__MaHoa__412EB0B6");

            entity.HasOne(d => d.ChiTietSanPham).WithMany(p => p.ChiTietHoaDonNhaps)
                .HasForeignKey(d => new { d.MaSanPham, d.MaMauSac, d.MaKichThuoc })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHoaDonNha__4222D4EF");
        });

        modelBuilder.Entity<ChiTietSanPham>(entity =>
        {
            entity.HasKey(e => new { e.MaSanPham, e.MaMauSac, e.MaKichThuoc }).HasName("PK__ChiTietS__A47F6AED3021045E");

            entity.ToTable("ChiTietSanPham");

            entity.Property(e => e.MaSanPham).HasMaxLength(30);
            entity.Property(e => e.MaMauSac).HasMaxLength(30);
            entity.Property(e => e.MaKichThuoc).HasMaxLength(30);

            entity.HasOne(d => d.MaKichThuocNavigation).WithMany(p => p.ChiTietSanPhams)
                .HasForeignKey(d => d.MaKichThuoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietSa__MaKic__4316F928");

            entity.HasOne(d => d.MaMauSacNavigation).WithMany(p => p.ChiTietSanPhams)
                .HasForeignKey(d => d.MaMauSac)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietSa__MaMau__440B1D61");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.ChiTietSanPhams)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietSa__MaSan__44FF419A");
        });

        modelBuilder.Entity<DoiTuongMh>(entity =>
        {
            entity.HasKey(e => e.MaDoiTuongMh).HasName("PK__DoiTuong__51FD5449EB8CC9EF");

            entity.ToTable("DoiTuongMH");

            entity.Property(e => e.MaDoiTuongMh)
                .HasMaxLength(30)
                .HasColumnName("MaDoiTuongMH");
            entity.Property(e => e.TenDoiTuongMh)
                .HasMaxLength(50)
                .HasColumnName("TenDoiTuongMH");
        });

        modelBuilder.Entity<HinhAnhSp>(entity =>
        {
            entity.HasKey(e => new { e.TenFileAnh, e.MaSanPham }).HasName("PK__HinhAnhS__41D342639B6A511A");

            entity.ToTable("HinhAnhSP");

            entity.Property(e => e.TenFileAnh).HasMaxLength(200);
            entity.Property(e => e.MaSanPham).HasMaxLength(30);

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.HinhAnhSps)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HinhAnhSP__MaSan__45F365D3");
        });

        modelBuilder.Entity<HoaDonBan>(entity =>
        {
            entity.HasKey(e => e.MaHoaDonBan).HasName("PK__HoaDonBa__6A50CA8ACDD6EE45");

            entity.ToTable("HoaDonBan");

            entity.Property(e => e.MaHoaDonBan).HasMaxLength(30);
            entity.Property(e => e.MaKhachHang).HasMaxLength(30);
            entity.Property(e => e.MaNhanVien).HasMaxLength(30);

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.HoaDonBans)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK__HoaDonBan__MaKha__46E78A0C");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.HoaDonBans)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK__HoaDonBan__MaNha__47DBAE45");
        });

        modelBuilder.Entity<HoaDonNhap>(entity =>
        {
            entity.HasKey(e => e.MaHoaDonNhap).HasName("PK__HoaDonNh__448838B56807216B");

            entity.ToTable("HoaDonNhap");

            entity.Property(e => e.MaHoaDonNhap).HasMaxLength(30);
            entity.Property(e => e.MaNhaCungCap).HasMaxLength(30);
            entity.Property(e => e.MaNhanVien).HasMaxLength(30);

            entity.HasOne(d => d.MaNhaCungCapNavigation).WithMany(p => p.HoaDonNhaps)
                .HasForeignKey(d => d.MaNhaCungCap)
                .HasConstraintName("FK__HoaDonNha__MaNha__48CFD27E");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.HoaDonNhaps)
                .HasForeignKey(d => d.MaNhanVien)
                .HasConstraintName("FK__HoaDonNha__MaNha__49C3F6B7");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__KhachHan__88D2F0E56466D3EE");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKhachHang).HasMaxLength(30);
            entity.Property(e => e.DiaChi).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenKhachHang).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(255);

            entity.HasOne(d => d.UserNameNavigation).WithMany(p => p.KhachHangs)
                .HasForeignKey(d => d.UserName)
                .HasConstraintName("FK__KhachHang__UserN__4AB81AF0");
        });

        modelBuilder.Entity<KichThuoc>(entity =>
        {
            entity.HasKey(e => e.MaKichThuoc).HasName("PK__KichThuo__22BFD66473803276");

            entity.ToTable("KichThuoc");

            entity.Property(e => e.MaKichThuoc).HasMaxLength(30);
            entity.Property(e => e.TenKichThuoc).HasMaxLength(20);
        });

        modelBuilder.Entity<LoaiSp>(entity =>
        {
            entity.HasKey(e => e.MaLoaiSp).HasName("PK__LoaiSP__1224CA7C9A59B2F5");

            entity.ToTable("LoaiSP");

            entity.Property(e => e.MaLoaiSp)
                .HasMaxLength(30)
                .HasColumnName("MaLoaiSP");
            entity.Property(e => e.TenLoaiSp)
                .HasMaxLength(50)
                .HasColumnName("TenLoaiSP");
        });

        modelBuilder.Entity<MauSac>(entity =>
        {
            entity.HasKey(e => e.MaMauSac).HasName("PK__MauSac__B9A9116233B559CA");

            entity.ToTable("MauSac");

            entity.Property(e => e.MaMauSac).HasMaxLength(30);
            entity.Property(e => e.TenMauSac).HasMaxLength(20);
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNhaCungCap).HasName("PK__NhaCungC__53DA9205F9102FCC");

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.MaNhaCungCap).HasMaxLength(30);
            entity.Property(e => e.DiaChi).HasMaxLength(100);
            entity.Property(e => e.DienThoai).HasMaxLength(20);
            entity.Property(e => e.TenNhaCungCap).HasMaxLength(50);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__NhanVien__77B2CA4701FB9CC1");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNhanVien).HasMaxLength(30);
            entity.Property(e => e.DiaChi).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenNhanVien).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(255);

            entity.HasOne(d => d.UserNameNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.UserName)
                .HasConstraintName("FK__NhanVien__UserNa__4BAC3F29");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__SanPham__FAC7442DB71C3855");

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSanPham).HasMaxLength(30);
            entity.Property(e => e.ChatLieu).HasMaxLength(50);
            entity.Property(e => e.HinhAnhAvatar).HasMaxLength(255);
            entity.Property(e => e.MaDoiTuongMh)
                .HasMaxLength(30)
                .HasColumnName("MaDoiTuongMH");
            entity.Property(e => e.MaLoaiSp)
                .HasMaxLength(30)
                .HasColumnName("MaLoaiSP");
            entity.Property(e => e.TenSanPham).HasMaxLength(50);

            entity.HasOne(d => d.MaDoiTuongMhNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaDoiTuongMh)
                .HasConstraintName("FK__SanPham__MaDoiTu__4CA06362");

            entity.HasOne(d => d.MaLoaiSpNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaLoaiSp)
                .HasConstraintName("FK__SanPham__MaLoaiS__4D94879B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserName).HasName("PK__User__C9F28457B89AE3C2");

            entity.ToTable("User");

            entity.Property(e => e.UserName).HasMaxLength(255);
            entity.Property(e => e.PassWord).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
