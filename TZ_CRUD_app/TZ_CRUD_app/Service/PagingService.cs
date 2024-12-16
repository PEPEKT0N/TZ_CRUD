namespace TZ_CRUD_app.Service
{
    public class PagingService
    {
        private const int DEFAULT_PAGE_SIZE = 8;
        private const int MAX_PAGE_SIZE = 1024;

        private readonly int _pageSize;

        public int PageSize { get { return _pageSize; } }

        public PagingService(int? pageSize=DEFAULT_PAGE_SIZE)
        {
            _pageSize = pageSize ?? DEFAULT_PAGE_SIZE;
            if (pageSize > MAX_PAGE_SIZE)
            {
                pageSize = MAX_PAGE_SIZE;
            }
            if (pageSize <= 0)
            {
                pageSize = DEFAULT_PAGE_SIZE;
            }
        }

        public int DecodeCursor(string? cursor)
        {
            int val;
            if (int.TryParse(cursor, out val))
            {
                return val;
            }
            return 0;
        }

        public string? EncodeCursor(int? cursor)
        {
            return cursor?.ToString();
        }
    }
}
