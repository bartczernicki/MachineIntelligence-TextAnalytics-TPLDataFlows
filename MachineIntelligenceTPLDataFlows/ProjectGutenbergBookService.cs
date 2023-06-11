using System;
using System.Collections.Generic;
using Classes;

namespace MachineIntelligenceTPLDataFlows
{
    public static class ProjectGutenbergBookService
    {
        public static List<ProjectGutenbergBook> GetBooks()
        {
            // List of example hard-coded books from Project Gutenberg
            // Oscar Wilde, Bram Stoker, Edgar Allen Poe
            var books = new List<ProjectGutenbergBook>
            {
                // Oscar Wilde
                new ProjectGutenbergBook{
                    BookTitle = "The Importance of Being Earnest: A Trivial Comedy for Serious People",
                    Author = "Oscar Wilde",
                    Url = "http://www.gutenberg.org/cache/epub/844/pg844.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Picture of Dorian Gray",
                    Author = "Oscar Wilde",
                    Url = "http://www.gutenberg.org/cache/epub/174/pg174.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Happy Prince and Other Tales",
                    Author = "Oscar Wilde",
                    Url = "http://www.gutenberg.org/files/902/902-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "An Ideal Husband",
                    Author = "Oscar Wilde",
                    Url = "http://www.gutenberg.org/files/885/885-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "De Profundis",
                    Author = "Oscar Wilde",
                    Url = "http://www.gutenberg.org/cache/epub/921/pg921.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Lady Windermere's Fan",
                    Author = "Oscar Wilde",
                    Url = "http://www.gutenberg.org/files/790/790-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Canterville Ghost",
                    Author = "Oscar Wilde",
                    Url = "http://www.gutenberg.org/cache/epub/14522/pg14522.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Lord Arthur Savile's Crime",
                    Author = "Oscar Wilde",
                    Url = "http://www.gutenberg.org/files/773/773-0.txt"},

                // Bram Stoker
                new ProjectGutenbergBook{
                    BookTitle = "Dracula",
                    Author = "Bram Stoker",
                    Url = "http://www.gutenberg.org/cache/epub/345/pg345.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Dracula's Guest",
                    Author = "Bram Stoker",
                    Url = "https://www.gutenberg.org/ebooks/10150"},
                new ProjectGutenbergBook{
                    BookTitle = "The Lair of the White Worm",
                    Author = "Bram Stoker",
                    Url = "http://www.gutenberg.org/cache/epub/1188/pg1188.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Jewel of Seven Stars",
                    Author = "Bram Stoker",
                    Url = "http://www.gutenberg.org/cache/epub/3781/pg3781.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Lady of the Shroud",
                    Author = "Bram Stoker",
                    Url = "http://www.gutenberg.org/cache/epub/3095/pg3095.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Famous Impostors",
                    Author = "Bram Stoker",
                    Url = "https://www.gutenberg.org/files/51391/51391-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Mystery of the Sea",
                    Author = "Bram Stoker",
                    Url = "https://www.gutenberg.org/files/42455/42455-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Man",
                    Author = "Bram Stoker",
                    Url = "http://www.gutenberg.org/cache/epub/2520/pg2520.txt"},

                // Edgar Allen Poe Books
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 1",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/files/2147/2147-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 2",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/files/2148/2148-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 3",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/files/2149/2149-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 4",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/files/2150/2150-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 5",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/files/2151/2151-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Bells, and Other Poems",
                    Author = "Edgar Allen Poe",
                    Url = "https://www.gutenberg.org/files/50852/50852-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Cask of Amontillado",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/cache/epub/1063/pg1063.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Mask of Red Death",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/cache/epub/1064/pg1064.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Narrative of Arthur Gordon Pym of Nantucket",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/cache/epub/51060/pg51060.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Edgar Allan Poe's Complete Poetical Works",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/cache/epub/10031/pg10031.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Eureka: A Prose Poem",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/files/32037/32037-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Raven",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/cache/epub/1065/pg1065.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Fall of the House of Usher",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/cache/epub/932/pg932.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Selections from Poe",
                    Author = "Edgar Allen Poe",
                    Url = "http://www.gutenberg.org/files/8893/8893-0.txt"}
            };

            return books;
        }
    }
}
