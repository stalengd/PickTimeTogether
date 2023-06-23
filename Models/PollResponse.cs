namespace PickTimeTogether.Models;

public class PollResponse
{
    public int Id { get; set; }
    public int PollId { get; set; }
    public Poll Poll { get; set; } = default!;
    public string? Name { get; set; }
    public List<int>? SelectedTime { get; set; }
}

public class PollResponseDto
{
    public string? Name { get; set; }
    public List<int>? SelectedTime { get; set; }

    public PollResponseDto() { }
    public PollResponseDto(PollResponse response)
    {
        Name = response.Name;
        SelectedTime = response.SelectedTime;
    }

    public PollResponse ToDomainObject()
    {
        return new PollResponse()
        {
            Name = Name,
            SelectedTime = SelectedTime,
        };
    }
}
