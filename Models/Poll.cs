namespace PickTimeTogether.Models;

public class Poll
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<PollResponse> Responses { get; set; } = new();
}

public class PollDto
{
    public string? Description { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<PollResponseDto> Responses { get; set; } = new();

    public PollDto() { }
    public PollDto(Poll poll)
    {
        Description = poll.Description;
        From = poll.From;
        To = poll.To;
        Responses = poll.Responses
            .Select(x => new PollResponseDto(x))
            .ToList();
    }

    public Poll ToDomainObject()
    {
        return new Poll()
        {
            Description = Description,
            From = From,
            To = To,
            Responses = Responses
                .Select(x => x.ToDomainObject())
                .ToList(),
        };
    }
}