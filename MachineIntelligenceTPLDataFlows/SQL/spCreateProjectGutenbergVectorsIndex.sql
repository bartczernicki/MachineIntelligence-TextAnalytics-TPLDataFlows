drop procedure if exists spCreateProjectGutengergVectorsIndex;
GO
create procedure spCreateProjectGutengergVectorsIndex
as
BEGIN

set nocount on;

-- Delete all the records, for re-runability
truncate table dbo.ProjectGutenbergBooksVectorsIndex;

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
GO
exec spCreateProjectGutengergVectorsIndex;

/*
select * from dbo.ProjectGutenbergBooksVectorsIndex
*/