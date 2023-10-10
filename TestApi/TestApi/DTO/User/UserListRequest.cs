using TestApi.Enums;

namespace TestApi.DTO.User
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Offset">offset of elemts from start</param>
    /// <param name="Count">count elements on page</param>
    /// <param name="SortBy"> type of <sortinf(0 for asd, 1 for desc)/param>
    /// <param name="SortValue"></param>
    /// <param name="FilterBy">filter field of user</param>
    /// <param name="FilterValue"> string value of filter field</param>
    public class UserListRequest
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public SortEnum SortBy { get; set; }
        public string SortValue { get; set; }
        public string FilterBy { get; set; }
        public string FilterValue { get; set; }
    }
}
