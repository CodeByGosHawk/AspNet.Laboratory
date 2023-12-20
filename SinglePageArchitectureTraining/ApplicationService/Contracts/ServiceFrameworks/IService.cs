using SinglePageArchitectureTraining.Frameworks.Contracts;

namespace SinglePageArchitectureTraining.ApplicationService.Contracts.ServiceFrameworks;

public interface IService<TInsert,TSelect,TSelectAll,TUpdate,TDelete>
{
    Task<IResponse<TSelectAll>> SelectAllAsync();
    Task<IResponse<TSelect>> SelectAsync(TSelect selectDto);
    Task<IResponse<TInsert>> InsertAsync(TInsert insertDto);
    Task<IResponse<TUpdate>> UpdateAsync(TUpdate updateDto);
    Task<IResponse<TDelete>> DeleteAsync(TDelete deleteDto);
    Task SaveAsync();
}