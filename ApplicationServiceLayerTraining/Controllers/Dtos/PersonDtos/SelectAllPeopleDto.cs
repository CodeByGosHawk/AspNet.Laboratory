namespace ApplicationServiceLayerTraining.Controllers.Dtos.PersonDtos;

public class SelectAllPeopleDto
{
    public IEnumerable<SelectPersonDto> SelectPersonDtosList { get; set; }
}
