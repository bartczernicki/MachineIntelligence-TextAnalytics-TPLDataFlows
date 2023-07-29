drop procedure if exists spCreateProjectGutengergVectorsIndex;
GO
create procedure spCreateProjectGutengergVectorsIndex
as
BEGIN

set nocount on;

-- Delete all the records, for re-runability
truncate table dbo.ProjectGutenbergBooksVectorsIndex;
-- truncate table dbo.ProjectGutenbergBooksVectorsCosineDistanceNumerators;

-- At scale, you need to chunk the vectors by thousands (tens of thousands)
with cte as
(
    select 
        v.Id,
        cast(tv.[key] as int) as vector_value_id,
        cast(tv.[value] as float) as vector_value  
    from 
        [dbo].[ProjectGutenbergBooks] as v
    cross apply 
        openjson(ParagraphEmbeddings) tv
)
insert into dbo.ProjectGutenbergBooksVectorsIndex
(Id, vector_value_id, vector_value)
select
    Id,
    vector_value_id,
    vector_value
from
    cte;

END

---- OPTIONAL INDEX
--DROP INDEX IF EXISTS ProjectGutenberg_SQLOPS_ProjectGutenbergBooksVectorsIndex_2_1 ON dbo.[ProjectGutenbergBooksVectorsIndex];
--GO
--CREATE INDEX ProjectGutenberg_SQLOPS_ProjectGutenbergBooksVectorsIndex_2_1 ON dbo.[ProjectGutenbergBooksVectorsIndex]  ([vector_value_id]) INCLUDE ([vector_value]);
GO
exec spCreateProjectGutengergVectorsIndex;
GO
drop index if exists ixcProjectGutenbergBooksVectorsIndex on dbo.ProjectGutenbergBooksVectorsIndex 
GO
create clustered columnstore index ixcProjectGutenbergBooksVectorsIndex
on dbo.ProjectGutenbergBooksVectorsIndex
order (id, vector_value_id);
GO
/*
select * from dbo.ProjectGutenbergBooksVectorsIndex
*/