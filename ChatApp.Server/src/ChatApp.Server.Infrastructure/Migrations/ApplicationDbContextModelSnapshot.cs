﻿// <auto-generated />
using System;
using ChatApp.Server.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatApp.Server.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChatApp.Server.Domain.ChannelRolePermissions.ChannelRolePermission", b =>
                {
                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("CanEdit")
                        .HasColumnType("bit");

                    b.HasKey("ChannelId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("ChannelRolePermissions");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Channels.Channel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("IconUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.ConversationParticipants.ConversationParticipant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("ParticipantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("ParticipantId");

                    b.ToTable("ConversationParticipants");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Conversations.Conversation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.DirectMessages.DirectMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.ToTable("DirectMessages");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.FriendShips.FriendShip", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AppUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RequesterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("RequesterId");

                    b.ToTable("FriendShips");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Messages.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Roles.AppRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ColorHex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("Level")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.HasIndex("NormalizedName", "ServerId")
                        .IsUnique()
                        .HasDatabaseName("RoleNameServerIdIndex");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.ServerMemberRoles.ServerMemberRole", b =>
                {
                    b.Property<Guid>("ServerMemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AppRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ServerMemberId", "AppRoleId");

                    b.HasIndex("AppRoleId");

                    b.ToTable("ServerMemberRoles");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.ServerMembers.ServerMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ActiveRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Nickname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ActiveRoleId");

                    b.HasIndex("ServerId");

                    b.HasIndex("UserId");

                    b.ToTable("ServerMembers");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Servers.Server", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IconUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Tokens.Token", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CurrentUsageCount")
                        .HasColumnType("int");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ExpirationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<int>("MaxUsageCount")
                        .HasColumnType("int");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TokenType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ServerId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.UserRoles.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("ServerId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Users.AppUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("BirthOfDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CreateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CustomStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeleteAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("DeleteUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(MAX)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<bool?>("Gender")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LastActive")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("UpdateAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("UpdateUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.ChannelRolePermissions.ChannelRolePermission", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Channels.Channel", "Channel")
                        .WithMany("RolePermissions")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatApp.Server.Domain.Roles.AppRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Channel");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Channels.Channel", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Servers.Server", "Server")
                        .WithMany("Channels")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Server");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.ConversationParticipants.ConversationParticipant", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Conversations.Conversation", "Conversation")
                        .WithMany("Participants")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChatApp.Server.Domain.Users.AppUser", "Participant")
                        .WithMany("Conversations")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("Participant");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.DirectMessages.DirectMessage", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Conversations.Conversation", "Conversation")
                        .WithMany()
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.FriendShips.FriendShip", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Users.AppUser", null)
                        .WithMany("FriendShips")
                        .HasForeignKey("AppUserId");

                    b.HasOne("ChatApp.Server.Domain.Users.AppUser", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChatApp.Server.Domain.Users.AppUser", "Requester")
                        .WithMany()
                        .HasForeignKey("RequesterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Requester");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Messages.Message", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Channels.Channel", "Channel")
                        .WithMany("Messages")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Channel");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Roles.AppRole", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Servers.Server", "Server")
                        .WithMany("Roles")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Server");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.ServerMemberRoles.ServerMemberRole", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Roles.AppRole", "AppRole")
                        .WithMany("ServerMemberRoles")
                        .HasForeignKey("AppRoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChatApp.Server.Domain.ServerMembers.ServerMember", "ServerMember")
                        .WithMany("ServerMemberRoles")
                        .HasForeignKey("ServerMemberId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AppRole");

                    b.Navigation("ServerMember");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.ServerMembers.ServerMember", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Roles.AppRole", "ActiveRole")
                        .WithMany()
                        .HasForeignKey("ActiveRoleId");

                    b.HasOne("ChatApp.Server.Domain.Servers.Server", "Server")
                        .WithMany("Members")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChatApp.Server.Domain.Users.AppUser", "User")
                        .WithMany("ServerMemberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ActiveRole");

                    b.Navigation("Server");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Servers.Server", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Users.AppUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Tokens.Token", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Users.AppUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChatApp.Server.Domain.Servers.Server", "Server")
                        .WithMany()
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.UserRoles.UserRole", b =>
                {
                    b.HasOne("ChatApp.Server.Domain.Roles.AppRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChatApp.Server.Domain.Servers.Server", "Server")
                        .WithMany()
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ChatApp.Server.Domain.Users.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("Server");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Channels.Channel", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Conversations.Conversation", b =>
                {
                    b.Navigation("Participants");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Roles.AppRole", b =>
                {
                    b.Navigation("ServerMemberRoles");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.ServerMembers.ServerMember", b =>
                {
                    b.Navigation("ServerMemberRoles");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Servers.Server", b =>
                {
                    b.Navigation("Channels");

                    b.Navigation("Members");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("ChatApp.Server.Domain.Users.AppUser", b =>
                {
                    b.Navigation("Conversations");

                    b.Navigation("FriendShips");

                    b.Navigation("ServerMemberships");
                });
#pragma warning restore 612, 618
        }
    }
}
