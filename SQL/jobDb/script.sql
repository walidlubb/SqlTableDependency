USE [msdb]
GO

/****** Object:  Job [run]    Script Date: 2024-12-21 10:48:46 AM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 2024-12-21 10:48:46 AM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'run', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'HP\„Õ„œ', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [save]    Script Date: 2024-12-21 10:48:47 AM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'save', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'
WITH CTE AS (
    SELECT TOP 1001
        [NotificationEventId],
        [UserId],
        [StatusId],
        [UserLink],
        [TableName],
        [TableID],
        [DeveloperDescriptionState],
        [NotificationActionId],
        GETDATE() AS CreatedDate,
        [UserRole],
        [LocationId],
        [CompanyID],
        [UserEmail],
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum
    FROM [LiveCMMS1].[dbo].[NotificationsUsers]
    WHERE [UserEmail] IS NOT NULL
)
INSERT INTO [NotificationsDB].[dbo].[NotificationsUsers] (
    [NotificationEventId],
    [UserId],
    [StatusId],
    [UserLink],
    [TableName],
    [TableID],
    [DeveloperDescriptionState],
    [isAlertSent],
    [isEmailSent],
    [NotificationActionId],
    [CreatedDate],
    [UserRole],
    [LocationId],
    [CompanyID],
    [UserEmail]
)
SELECT
    [NotificationEventId],
    [UserId],
    [StatusId],
    [UserLink],
    [TableName],
    [TableID],
    [DeveloperDescriptionState],
    0 AS isAlertSent,
    0 AS isEmailSent,
    [NotificationActionId],
    CreatedDate,
    [UserRole],
    [LocationId],
    [CompanyID],
    CONCAT(UserEmail, ''_'', RowNum) AS UserEmail -- Append RowNum to UserEmail
FROM CTE;
GO
', 
		@database_name=N'NotificationsDB', 
		@output_file_name=N'C:\out.txt', 
		@flags=18
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'am', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=2, 
		@freq_subday_interval=10, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20241123, 
		@active_end_date=99991231, 
		@active_start_time=100000, 
		@active_end_time=185959, 
		@schedule_uid=N'0d06b727-7527-4452-84d8-57259e67010f'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO


