using System;
namespace Marketing.API.Dto
{
    public class UserLocationDTO
    {
        public string Id { get; set; }
        public Guid UserId { get; set; }
        public int LocationId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}