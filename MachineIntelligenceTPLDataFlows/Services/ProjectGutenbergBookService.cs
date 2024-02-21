using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using MachineIntelligenceTPLDataFlows.Classes;
using System.Threading.Tasks;

namespace MachineIntelligenceTPLDataFlows.Services
{
    public class ProjectGutenbergBookService : IProjectGutenbergBooksService
    {
        // private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        public ProjectGutenbergBookService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<string> GetBookText(string bookUrl)
        {
            var bookText = string.Empty;

            var httpClient = _httpClient;
            var responseService = await httpClient.GetAsync(bookUrl);

            // Check the Response
            if (responseService.IsSuccessStatusCode)
            {
                bookText = await responseService.Content.ReadAsStringAsync();
            }

            return bookText;
        }

        public List<ProjectGutenbergBook> GetBooksList()
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
                    BookTitle = "An Ideal Husband",
                    Author = "Oscar Wilde",
                    Url = mirrorLocation + "885/885-0.txt"},
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
                new ProjectGutenbergBook{
                    BookTitle = "The Mask of Red Death",
                    Author = "Edgar Allen Poe",
                    Url = mirrorLocation + "1064/1064-0.txt"},
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
                    Url = mirrorLocation + "8893/8893-0.txt"},

                // Alexandre Dumas
                new ProjectGutenbergBook{
                    BookTitle = "Twenty Years After",
                    Author = "Alexandre Dumas",
                    Url = mirrorLocation + "1259/1259-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Count of Monte Cristo",
                    Author = "Alexandre Dumas",
                    Url = mirrorLocation + "1184/1184-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Three Musketeers",
                    Author = "Alexandre Dumas",
                    Url = mirrorLocation + "1257/1257-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Man in the Iron Mask",
                    Author = "Alexandre Dumas",
                    Url = mirrorLocation + "2759/2759-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Black Tulip",
                    Author = "Alexandre Dumas",
                    Url = mirrorLocation + "965/965-0.txt"},

                // Charles Dickens
                new ProjectGutenbergBook{
                    BookTitle = "A Tale of Two Cities",
                    Author = "Charles Dickens",
                    Url = mirrorLocation + "98/98-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Great Expectations",
                    Author = "Charles Dickens",
                    Url = mirrorLocation + "1400/1400-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Oliver Twist",
                    Author = "Charles Dickens",
                    Url = mirrorLocation + "730/730-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "David Copperfield",
                    Author = "Charles Dickens",
                    Url = mirrorLocation + "766/766-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Hard Times",
                    Author = "Charles Dickens",
                    Url = mirrorLocation + "786/786-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Bleak House",
                    Author = "Charles Dickens",
                    Url = mirrorLocation + "1023/1023-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Pickwick Papers",
                    Author = "Charles Dickens",
                    Url = mirrorLocation + "580/580-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Old Curiosity Shop",
                    Author = "Charles Dickens",
                    Url = mirrorLocation + "700/700-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "American Notes",
                    Author = "Charles Dickens",
                    Url = mirrorLocation + "675/675-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Life And Adventures Of Martin Chuzzlewit",
                    Author = "Charles Dickens",
                    Url = mirrorLocation + "968/968-0.txt"},

                // Mark Twain
                new ProjectGutenbergBook{
                    BookTitle = "Adventures of Huckleberry Finn",
                    Author = "Mark Twain",
                    Url = mirrorLocation + "76/76-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Eve's Diary",
                    Author = "Mark Twain",
                    Url = mirrorLocation + "8525/8525-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Life on the Mississippi",
                    Author = "Mark Twain",
                    Url = mirrorLocation + "245/245-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "The Innocents Abroad",
                    Author = "Mark Twain",
                    Url = mirrorLocation + "3176/3176-0.txt"},
                new ProjectGutenbergBook{
                    BookTitle = "Roughing It",
                    Author = "Mark Twain",
                    Url = mirrorLocation + "3177/3177-0.txt"}
            };

            //var ensureNoDuplicateTitles = books.Select(x => x.BookTitle).Distinct().ToList();
            //var ensureNoDuplicateUrls = books.Select(x => x.Url).Distinct().ToList();

            // For debugging return a two books
            // It will process the sample question below from the top two books fine
            // Otherwise it returns 50 full novels/books
            //return books.Skip(1).Take(1).ToList();

            return books;
        }

        public List<SearchMessage> GetQueriesList()
        {
            var searchMessages = new List<SearchMessage>
            {
                // Edgar Allen Poe - The Raven
                new SearchMessage{
                    SearchString = "What did the Raven say?",
                    BookTitle = string.Empty, // Search the entire vector index
                    SemanticKernelPluginName = "AnswerBookQuestion"
                },
                // Mark Twain - Adventures of Huckleberry Finn
                new SearchMessage{
                    SearchString = "Who does the \"dauphin\" claim he is?",
                    BookTitle = "Adventures of Huckleberry Finn", // Search the entire vector index
                    SemanticKernelPluginName = "AnswerBookQuestion"
                },
                // Oscar Wilde - An Ideal Husband
                new SearchMessage{
                    SearchString = "Who does Lord Goring propose to for marriage?",
                    BookTitle = "An Ideal Husband", // Sometimes you know (or need) more information (for example, the specific book)
                    SemanticKernelPluginName = "AnswerBookQuestion"
                    // Filter the index on only the selected book title to greatly improve performance
                }
            };

            return searchMessages;
        }
    }
}
