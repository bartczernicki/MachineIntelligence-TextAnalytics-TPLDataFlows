using System;
using System.Collections.Generic;

namespace MachineIntelligenceTPLDataFlows
{
    public static class ProjectGutenbergBookService
    {
        public static List<ProjectGutenbergBook> GetBooks()
        {
            var books = new List<ProjectGutenbergBook>
            {
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 1",
                    Url = "http://www.gutenberg.org/files/2147/2147-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 2",
                    Url = "http://www.gutenberg.org/files/2148/2148-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 3",
                    Url = "http://www.gutenberg.org/files/2149/2149-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 4",
                    Url = "http://www.gutenberg.org/files/2150/2150-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 5",
                    Url = "http://www.gutenberg.org/files/2151/2151-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Cask of Amontillado",
                    Url = "http://www.gutenberg.org/cache/epub/1063/pg1063.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Mask of Red Death",
                    Url = "http://www.gutenberg.org/cache/epub/1064/pg1064.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Narrative of Arthur Gordon Pym of Nantucket",
                    Url = "http://www.gutenberg.org/cache/epub/51060/pg51060.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Edgar Allan Poe's Complete Poetical Works",
                    Url = "http://www.gutenberg.org/cache/epub/10031/pg10031.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Eureka: A Prose Poem",
                    Url = "http://www.gutenberg.org/files/32037/32037-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Raven",
                    Url = "http://www.gutenberg.org/cache/epub/1065/pg1065.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Fall of the House of Usher",
                    Url = "http://www.gutenberg.org/cache/epub/932/pg932.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Selections from Poe",
                    Url = "http://www.gutenberg.org/files/8893/8893-0.txt"}
            };

            return books;
        }
    }
}
