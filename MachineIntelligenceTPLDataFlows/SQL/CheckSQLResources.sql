SELECT 
cpu_count as CPUCores,
sqlserver_start_time as SQLStartTime,
(committed_kb/1024) as MemoryUsed,
(committed_target_kb/1024) as MemoryAvailable
FROM sys.dm_os_sys_info;