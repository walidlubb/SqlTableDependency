USE [master]
GO
/****** Object:  Database [NotificationsDB]    Script Date: 2024-12-21 10:46:41 AM ******/
CREATE DATABASE [NotificationsDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NotificationsDB', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\NotificationsDB.mdf' , SIZE = 532480KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NotificationsDB_log', FILENAME = N'D:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\NotificationsDB_log.ldf' , SIZE = 8921088KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [NotificationsDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NotificationsDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NotificationsDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NotificationsDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NotificationsDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NotificationsDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NotificationsDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [NotificationsDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NotificationsDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NotificationsDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NotificationsDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NotificationsDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NotificationsDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NotificationsDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NotificationsDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NotificationsDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NotificationsDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [NotificationsDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NotificationsDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NotificationsDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NotificationsDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NotificationsDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NotificationsDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NotificationsDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NotificationsDB] SET RECOVERY FULL 
GO
ALTER DATABASE [NotificationsDB] SET  MULTI_USER 
GO
ALTER DATABASE [NotificationsDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NotificationsDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NotificationsDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NotificationsDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NotificationsDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [NotificationsDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'NotificationsDB', N'ON'
GO
ALTER DATABASE [NotificationsDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [NotificationsDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [NotificationsDB]
GO
/****** Object:  User [NotificationAdmin]    Script Date: 2024-12-21 10:46:42 AM ******/
CREATE USER [NotificationAdmin] FOR LOGIN [NotificationAdmin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [NotificationAdmin]
GO
/****** Object:  Table [dbo].[DeadLetterQueue]    Script Date: 2024-12-21 10:46:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeadLetterQueue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NotificationId] [int] NOT NULL,
	[ErrorMessage] [nvarchar](max) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationsUsers]    Script Date: 2024-12-21 10:46:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationsUsers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[NotificationEventId] [nvarchar](50) NULL,
	[UserId] [bigint] NOT NULL,
	[StatusId] [smallint] NULL,
	[UserLink] [nvarchar](1000) NULL,
	[TableName] [nvarchar](50) NULL,
	[TableID] [bigint] NULL,
	[DeveloperDescriptionState] [nvarchar](max) NULL,
	[isAlertSent] [bit] NULL,
	[isEmailSent] [bit] NULL,
	[NotificationActionId] [bigint] NOT NULL,
	[CreatedDate] [date] NULL,
	[UserRole] [nvarchar](50) NULL,
	[LocationId] [bigint] NULL,
	[CompanyID] [bigint] NULL,
	[UserEmail] [nvarchar](50) NULL,
 CONSTRAINT [PK_NotificationsUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[CleanupProcessedNotifications]    Script Date: 2024-12-21 10:46:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CleanupProcessedNotifications]
AS
BEGIN
    DELETE FROM [dbo].[NotificationsUsers] WHERE [isAlertSent] = 1 AND [CreatedDate] < DATEADD(DAY, -1, GETDATE());
END
GO
/****** Object:  StoredProcedure [dbo].[dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_QueueActivationSender]    Script Date: 2024-12-21 10:46:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_QueueActivationSender] 
WITH EXECUTE AS SELF
AS 
BEGIN 
    SET NOCOUNT ON;
    DECLARE @h AS UNIQUEIDENTIFIER;
    DECLARE @mt NVARCHAR(200);

    RECEIVE TOP(1) @h = conversation_handle, @mt = message_type_name FROM [dbo].[dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Sender];

    IF @mt = N'http://schemas.microsoft.com/SQL/ServiceBroker/EndDialog'
    BEGIN
        END CONVERSATION @h;
    END

    IF @mt = N'http://schemas.microsoft.com/SQL/ServiceBroker/DialogTimer' OR @mt = N'http://schemas.microsoft.com/SQL/ServiceBroker/Error'
    BEGIN 
        

        END CONVERSATION @h;

        DECLARE @conversation_handle UNIQUEIDENTIFIER;
        DECLARE @schema_id INT;
        SELECT @schema_id = schema_id FROM sys.schemas WITH (NOLOCK) WHERE name = N'dbo';

        
        IF EXISTS (SELECT * FROM sys.triggers WITH (NOLOCK) WHERE object_id = OBJECT_ID(N'[dbo].[tr_dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Sender]')) DROP TRIGGER [dbo].[tr_dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Sender];

        
        IF EXISTS (SELECT * FROM sys.service_queues WITH (NOLOCK) WHERE schema_id = @schema_id AND name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Sender') EXEC (N'ALTER QUEUE [dbo].[dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Sender] WITH ACTIVATION (STATUS = OFF)');

        
        SELECT conversation_handle INTO #Conversations FROM sys.conversation_endpoints WITH (NOLOCK) WHERE far_service LIKE N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_%' ORDER BY is_initiator ASC;
        DECLARE conversation_cursor CURSOR FAST_FORWARD FOR SELECT conversation_handle FROM #Conversations;
        OPEN conversation_cursor;
        FETCH NEXT FROM conversation_cursor INTO @conversation_handle;
        WHILE @@FETCH_STATUS = 0 
        BEGIN
            END CONVERSATION @conversation_handle WITH CLEANUP;
            FETCH NEXT FROM conversation_cursor INTO @conversation_handle;
        END
        CLOSE conversation_cursor;
        DEALLOCATE conversation_cursor;
        DROP TABLE #Conversations;

        
        IF EXISTS (SELECT * FROM sys.services WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Receiver') DROP SERVICE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Receiver];
        
        IF EXISTS (SELECT * FROM sys.services WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Sender') DROP SERVICE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Sender];

        
        IF EXISTS (SELECT * FROM sys.service_queues WITH (NOLOCK) WHERE schema_id = @schema_id AND name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Receiver') DROP QUEUE [dbo].[dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Receiver];
        
        IF EXISTS (SELECT * FROM sys.service_queues WITH (NOLOCK) WHERE schema_id = @schema_id AND name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Sender') DROP QUEUE [dbo].[dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_Sender];

        
        IF EXISTS (SELECT * FROM sys.service_contracts WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534') DROP CONTRACT [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534];
        
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/StartMessage/Insert') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/StartMessage/Insert];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/StartMessage/Update') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/StartMessage/Update];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/StartMessage/Delete') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/StartMessage/Delete];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/Id') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/Id];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/UserId') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/UserId];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/UserRole') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/UserRole];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/LocationId') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/LocationId];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/CompanyID') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/CompanyID];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/UserEmail') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/UserEmail];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/isAlertSent') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/isAlertSent];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/UserLink') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/UserLink];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/TableName') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/TableName];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/TableID') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/TableID];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/DeveloperDescriptionState') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/DeveloperDescriptionState];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/CreatedDate') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/CreatedDate];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/NotificationActionId') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/NotificationActionId];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/NotificationEventId') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/NotificationEventId];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/StatusId') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/StatusId];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/isEmailSent') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/isEmailSent];
        IF EXISTS (SELECT * FROM sys.service_message_types WITH (NOLOCK) WHERE name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/EndMessage') DROP MESSAGE TYPE [dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534/EndMessage];

        
        IF EXISTS (SELECT * FROM sys.objects WITH (NOLOCK) WHERE schema_id = @schema_id AND name = N'dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_QueueActivationSender') DROP PROCEDURE [dbo].[dbo_NotificationsUsers_8d42a9cc-cba4-42db-879e-3ce74a629534_QueueActivationSender];

        
    END
END
GO
USE [master]
GO
ALTER DATABASE [NotificationsDB] SET  READ_WRITE 
GO
