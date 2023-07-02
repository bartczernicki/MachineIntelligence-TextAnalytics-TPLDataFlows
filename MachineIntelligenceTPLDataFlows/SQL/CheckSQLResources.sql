-- Use this to check SQL Resources available
-- Azure SQL needs 2 vCores, 2+ gig to process the selected documents/books
SELECT 
cpu_count as CPUCores,
sqlserver_start_time as SQLStartTime,
(committed_kb/1024) as MemoryUsed,
(committed_target_kb/1024) as MemoryAvailable
FROM sys.dm_os_sys_info;