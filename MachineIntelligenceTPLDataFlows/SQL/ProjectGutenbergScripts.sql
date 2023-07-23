/*
SQL script to build the suporting tables, stored procedures for Project Gutenberg Vectors Index
*/
drop table if exists dbo.ProjectGutenbergBooks;
CREATE TABLE [dbo].[ProjectGutenbergBooks](
	[Id] [int] NOT NULL identity(1,1),
	Author [varchar](100) NOT NULL,
	BookTitle [varchar](100) NOT NULL,
	[Url] [varchar](200) NOT NULL,
	Paragraph [varchar](6000) NOT NULL,
	ParagraphEmbeddings varchar(max) NOT NULL,
    ParagraphEmbeddingsCosineSimilarityDenominator [float] NOT NULL
 CONSTRAINT pkProjectGutenbergBooks PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
)
GO
/*
select * from ProjectGutenbergBooks;
*/

drop table if exists dbo.ProjectGutenbergBooksVectorsIndex;
CREATE TABLE [dbo].ProjectGutenbergBooksVectorsIndex(
	[Id] [int] NOT NULL,
	[vector_value_id] [int] NOT NULL,
	[vector_value] [float] NULL
 CONSTRAINT pkProjectGutenbergBooksVectorsIndex PRIMARY KEY CLUSTERED 
(
	[Id] ASC, [vector_value_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
)
GO
/*
select * from dbo.ProjectGutenbergBooksVectorsIndex;
*/

drop procedure if exists spSearchProjectGutenbergVectors;
GO
create procedure spSearchProjectGutenbergVectors
@jsonOpenAIEmbeddings nvarchar(max),
@bookTitle varchar(100)
as
BEGIN

SET NOCOUNT ON;

drop table if exists #t;
select
    cast([key] as int) as [vector_value_id],
    cast([value] as float) as [vector_value]
into
	#t
from 
    openjson(@jsonOpenAIEmbeddings, '$') -- '$.data[0].embedding')

/*
Use Cosine Distance - OpenAI Recommendations
https://help.openai.com/en/articles/6824809-embeddings-frequently-asked-questions
OpenAI embeddings are normalized to length 1, which means that:
- Cosine similarity can be computed slightly faster using just a dot product
- Cosine similarity and Euclidean distance will result in the identical rankings
*/
drop table if exists #results;
select top(20)
    v2.Id, 
    sum(v1.[vector_value] * v2.[vector_value]) / 
        (
            sqrt(sum(v1.[vector_value] * v1.[vector_value])) 
            * 
			--pre-computing 1536 dimension vectors saves about 500ms on non-filtered queries
			b1.ParagraphEmbeddingsCosineSimilarityDenominator
        ) as cosine_distance
into
    #results
from 
    #t v1
inner join 
    dbo.ProjectGutenbergBooksVectorsIndex v2 on v1.vector_value_id = v2.vector_value_id
inner join
	dbo.ProjectGutenbergBooks b1 on b1.Id = v2.Id
WHERE (
        (LEN(@bookTitle) > 0 AND b1.BookTitle = b1.BookTitle) 
        OR 
        (LEN(@bookTitle) = 0 AND b1.BookTitle IS NOT NULL)
	  )
group by
    v2.Id, b1.ParagraphEmbeddingsCosineSimilarityDenominator
order by
    cosine_distance desc;

select 
    a.Id,
    a.BookTitle,
	a.Author,
	a.Paragraph,
    --a.Url,
    r.cosine_distance as CosineDistance
from 
    #results r
inner join 
    dbo.ProjectGutenbergBooks a on r.Id = a.Id
order by
    cosine_distance desc
END
GO

-- exec spCreateProjectGutengergVectorsIndex;
