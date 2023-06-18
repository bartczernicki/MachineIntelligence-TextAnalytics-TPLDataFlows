using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MachineIntelligenceTPLDataFlows.Classes;
using static System.Net.WebRequestMethods;

namespace MachineIntelligenceTPLDataFlows
{
    public static class ProjectGutenbergBookService
    {
        public static List<ProjectGutenbergBook> GetBooks()
        {
            // Project Gutenberg list of Mirrors
            // https://www.gutenberg.org/MIRRORS.ALL

            // Project Gutenberg Mirror location
            string mirrorLocation = "https://www.gutenberg.org/files/";

            // List of example hard-coded books from Project Gutenberg
            // Oscar Wilde, Bram Stoker, Edgar Allen Poe
            var books = new List<ProjectGutenbergBook>
            {
                // Oscar Wilde
                new ProjectGutenbergBook{
                    BookTitle = "The Importance of Being Earnest: A Trivial Comedy for Serious People",
                    Author = "Oscar Wilde",
                    Url = mirrorLocation + "844/844-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Picture of Dorian Gray",
                    Author = "Oscar Wilde",
                    Url = mirrorLocation + "174/174-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Happy Prince and Other Tales",
                    Author = "Oscar Wilde",
                    Url = mirrorLocation + "902/902-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "An Ideal Husband",
                    Author = "Oscar Wilde",
                    Url = mirrorLocation + "885/885-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "De Profundis",
                    Author = "Oscar Wilde",
                    Url = mirrorLocation + "1338/1338.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Lady Windermere's Fan",
                    Author = "Oscar Wilde",
                    Url = mirrorLocation + "790/790-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Canterville Ghost",
                    Author = "Oscar Wilde",
                    Url = mirrorLocation + "14522/14522-8.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Lord Arthur Savile's Crime",
                    Author = "Oscar Wilde",
                    Url = mirrorLocation + "773/773-0.txt"},

                // Bram Stoker
                new ProjectGutenbergBook{
                    BookTitle = "Dracula",
                    Author = "Bram Stoker",
                    Url = mirrorLocation + "345/345-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Dracula's Guest",
                    Author = "Bram Stoker",
                    Url = mirrorLocation + "10150/10150-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Lair of the White Worm",
                    Author = "Bram Stoker",
                    Url = mirrorLocation + "1188/1188.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Jewel of Seven Stars",
                    Author = "Bram Stoker",
                    Url = mirrorLocation + "3781/3781-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Lady of the Shroud",
                    Author = "Bram Stoker",
                    Url = mirrorLocation + "3095/3095.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Famous Impostors",
                    Author = "Bram Stoker",
                    Url = mirrorLocation + "51391/51391-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Mystery of the Sea",
                    Author = "Bram Stoker",
                    Url = mirrorLocation + "42455/42455-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Man",
                    Author = "Bram Stoker",
                    Url = mirrorLocation + "2520/2520.txt"},

                // Edgar Allen Poe Books
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 1",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "2147/2147-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 2",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "2148/2148-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 3",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "2149/2149-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 4",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "2150/2150-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Works of Edgar Allen Poe Volume 5",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "2151/2151-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Bells, and Other Poems",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "50852/50852-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Cask of Amontillado",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "1063/1063.txt"},
                //new ProjectGutenbergBook{
                //    BookTitle = "The Mask of Red Death",
                //    Author = "Edgar Allen Poe",
                //    Url = "http://www.gutenberg.org/cache/epub/1064/pg1064.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Narrative of Arthur Gordon Pym of Nantucket",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "51060/51060-8.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Edgar Allan Poe's Complete Poetical Works",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "10031/10031-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Eureka: A Prose Poem",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "32037/32037-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Raven",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "1065/1065-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Fall of the House of Usher",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "932/932.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Selections from Poe",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "8893/8893-0.txt"}
            };

            // for debugging return a single book
            return books.Take(2).ToList();

            //return books;
        }
    }
}
