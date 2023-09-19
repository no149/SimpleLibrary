using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Services
{
    public class FinderService
    {
        public FinderService() { }

        public async Task<FindResult[]> FindByIsbn(string isbn)
        {
            var result = await SearchResources(isbn);
            return new SearchResultParser().Parse(result);
        }

        private async Task<string> SearchResources(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
                throw new ArgumentException("Isbn argument must have a value.");

            using (var client = new HttpClient())
            {
                return await (await client.GetAsync($"https://libs.nlai.ir/search?query={isbn}")).Content.ReadAsStringAsync(); ;
            }
        }
    }
}
