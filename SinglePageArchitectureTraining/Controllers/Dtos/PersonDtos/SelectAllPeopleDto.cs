namespace SinglePageArchitectureTraining.Controllers.Dtos.PersonDtos;

public class SelectAllPersonsDto
{
    public IEnumerable<SelectPersonDto> SelectPersonDtosList { get; set; }
}
