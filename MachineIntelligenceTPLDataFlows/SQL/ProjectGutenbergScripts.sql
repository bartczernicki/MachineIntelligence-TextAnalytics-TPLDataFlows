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
	ParagraphEmbeddings varchar(max) NOT NULL
 CONSTRAINT pkProjectGutenbergBooks PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
)

/*
select * from ProjectGutenbergBooks;
*/

drop table if exists dbo.ProjectGutenbergBooksVectorsIndex;
CREATE TABLE [dbo].ProjectGutenbergBooksVectorsIndex(
	[Id] [int] NOT NULL,
	[vector_value] [float] NULL
 CONSTRAINT pkProjectGutenbergBooksVectorsIndex PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
)

/*
select * from dbo.ProjectGutenbergBooksVectorsIndex;
*/