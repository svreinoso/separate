using Separate.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Separate.Services
{
    public interface IEmailServices
    {
        void GetAllBookmarks();
        int GetBookmarksCount();
    }

    public class EmailServices : IEmailServices
    {

        private readonly ApplicationDbContext _context;

        public EmailServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public void GetAllBookmarks()
        {
            var a = _context.Bookmarks.Count();
            var stopHere = _context.Database.CanConnect();
        }

        public int GetBookmarksCount()
        {
            return _context.Bookmarks.Count();
        }
    }
}
