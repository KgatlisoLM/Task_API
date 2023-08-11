namespace Task_API.Models.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }


    public class TaskCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
    }


    public class TaskUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
    }
}
