using Microsoft.AspNetCore.Mvc;
using SinglePageArchitectureTraining.ApplicationService.Contracts;
using SinglePageArchitectureTraining.ApplicationService.Dtos.ProductDtos;
using SinglePageArchitectureTraining.Controllers.Dtos.ProductDtos;
using SinglePageArchitectureTraining.Frameworks.Abstracts;

namespace ApplicationServiceLayerTraining.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var selectAllOperationResponse = await _productService.SelectAllAsync();

        var selectDtos = new List<SelectProductDto>();

        foreach (var serviceSelectDto in selectAllOperationResponse.Value.SelectProductDtosList)
        {
            SelectProductDto selectDto = new SelectProductDto()
            {
                Id = serviceSelectDto.Id,
                Title = serviceSelectDto.Title,
                Quantity = serviceSelectDto.Quantity,
                UnitPrice = serviceSelectDto.UnitPrice
            };
            selectDtos.Add(selectDto);
        }

        var selectAllDto = new SelectAllProductsDto
        {
            SelectProductDtosList = selectDtos
        };

        return View(selectAllDto);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");
        var serviceSelectDto = new ServiceSelectProductDto { Id = id };

        var selectOperationResponse = await _productService.SelectAsync(serviceSelectDto);

        if (selectOperationResponse is null)
        {
            TempData["Index"] = "Product not found";
            return RedirectToAction(nameof(Index));
        }

        var selectDto = new SelectProductDto
        {
            Id = selectOperationResponse.Value.Id,
            Title = selectOperationResponse.Value.Title,
            Quantity = selectOperationResponse.Value.Quantity,
            UnitPrice = selectOperationResponse.Value.UnitPrice,
            Code = selectOperationResponse.Value.Code
        };

        return View(selectDto);

    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ServiceCreateProductDto createDto)
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var serviceSelectDto = new ServiceSelectProductDto { Code = createDto.Code };

        var selectOperationResponse = await _productService.SelectAsync(serviceSelectDto);

        if (ModelState.IsValid && selectOperationResponse.Status == Status.NotFound) //Enum should be able to check with int !
        {
            var serviceCreateDto = new ServiceCreateProductDto  /// Unreadable !!!
            {
                Title = createDto.Title,
                Code = createDto.Code,
                Quantity = createDto.Quantity,
                UnitPrice = createDto.UnitPrice
            };

            await _productService.InsertAsync(serviceCreateDto);
            await _productService.SaveAsync();

            TempData["Index"] = $"Product \"{serviceCreateDto.Title}\" created";
            return RedirectToAction(nameof(Index));
        }
        else if (ModelState.IsValid && selectOperationResponse.Status == Status.Successful)
        {
            TempData["Create"] = $"Product with ProductCode : {createDto.Code} already exist";
            return View();
        }
        else
        {
            return View();
        }
    }

    public async Task<IActionResult> Update(Guid id)
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var serviceSelectDto = new ServiceSelectProductDto { Id = id };

        var selectOperationResponse = await _productService.SelectAsync(serviceSelectDto);

        if (selectOperationResponse.Status == Status.NotFound)
        {
            TempData["Index"] = "Product does not exist";
            return RedirectToAction(nameof(Index));
        }

        var updateDto = new UpdateProductDto
        {
            Id = selectOperationResponse.Value.Id,
            Title = selectOperationResponse.Value.Title,
            Quantity = selectOperationResponse.Value.Quantity,
            UnitPrice = selectOperationResponse.Value.UnitPrice,
            Code = selectOperationResponse.Value.Code
        };

        return View(updateDto);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductDto updateDto)
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var serviceSelectDto = new ServiceSelectProductDto { Code = updateDto.Code };

        var selectOperationResponse = await _productService.SelectAsync(serviceSelectDto);

        bool updateCondition = (selectOperationResponse.Status == Status.Successful && selectOperationResponse.Value.Id == updateDto.Id) ||
                                selectOperationResponse.Status == Status.NotFound;

        if (ModelState.IsValid && updateCondition)
        {
            var serviceUpdateDto = new ServiceUpdateProductDto  /// Unreadable !!!
            {
                Id = updateDto.Id,
                Title = updateDto.Title,
                Quantity = updateDto.Quantity,
                UnitPrice = updateDto.UnitPrice,
                Code = updateDto.Code,
            };

            await _productService.UpdateAsync(serviceUpdateDto);
            await _productService.SaveAsync();

            TempData["Index"] = $"Product \"{updateDto.Title}\" successfully updated";
            return RedirectToAction(nameof(Index));
        }
        else if (ModelState.IsValid && !updateCondition)
        {
            TempData["Update"] = $"Product with ProductCode : {updateDto.Code} already exist";
            return View(updateDto);
        }
        else
        {
            return View();
        }
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var serviceSelectDto = new ServiceSelectProductDto { Id = id };
        var selectOperationResponse = await _productService.SelectAsync(serviceSelectDto);

        if (selectOperationResponse.Status == Status.NotFound)
        {
            TempData["Index"] = "Product does not exist";
            return RedirectToAction(nameof(Index));
        }
        var deleteDto = new DeleteProductDto
        {
            Id = new Guid(),
            Title = selectOperationResponse.Value.Title,
            Quantity = selectOperationResponse.Value.Quantity,
            UnitPrice = selectOperationResponse.Value.UnitPrice,
            Code = selectOperationResponse.Value.Code
        };
        // هر آیدی که میفرستی به ویو باز خودش آیدی که از ایندکس اومده رو پاس میده به ویو. 
        return View(deleteDto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteProductDto deleteDto)
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var serviceDeleteDto = new ServiceDeleteProductDto { Id = deleteDto.Id };
        await _productService.DeleteAsync(serviceDeleteDto);
        await _productService.SaveAsync();

        TempData["Index"] = $"Product \"{deleteDto.Title}\" deleted";
        return RedirectToAction(nameof(Index));
    }
}
