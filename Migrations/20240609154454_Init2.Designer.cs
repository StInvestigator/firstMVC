﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using firstMVC.Services;

#nullable disable

namespace firstMVC.Migrations
{
    [DbContext(typeof(SiteContext))]
    [Migration("20240609154454_Init2")]
    partial class Init2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.6");

            modelBuilder.Entity("firstMVC.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OriginalName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("firstMVC.Models.Profession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Professions");
                });

            modelBuilder.Entity("firstMVC.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("firstMVC.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Balance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsMale")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ProfessionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("firstMVC.Models.UserSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Mastery")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SkillId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SkillId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSkills");
                });

            modelBuilder.Entity("firstMVC.Models.Image", b =>
                {
                    b.HasOne("firstMVC.Models.User", null)
                        .WithMany("Gallery")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("firstMVC.Models.Profession", b =>
                {
                    b.HasOne("firstMVC.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("firstMVC.Models.Skill", b =>
                {
                    b.HasOne("firstMVC.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("firstMVC.Models.User", b =>
                {
                    b.HasOne("firstMVC.Models.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");

                    b.Navigation("Image");
                });

            modelBuilder.Entity("firstMVC.Models.UserSkill", b =>
                {
                    b.HasOne("firstMVC.Models.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("firstMVC.Models.User", null)
                        .WithMany("Skills")
                        .HasForeignKey("UserId");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("firstMVC.Models.User", b =>
                {
                    b.Navigation("Gallery");

                    b.Navigation("Skills");
                });
#pragma warning restore 612, 618
        }
    }
}
