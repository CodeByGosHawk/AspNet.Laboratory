using ApplicationServiceLayerTraining.ApplicationService.Contracts;
using ApplicationServiceLayerTraining.ApplicationService.Dtos.ProductDtos;
using ApplicationServiceLayerTraining.Controllers.Dtos.ProductDtos;
using Microsoft.AspNetCore.Mvc;

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

        var products = await _productService.SelectAllAsync();

        var selectProductsDtos = new List<SelectProductsDto>();

        foreach (var product in products)
        {
            SelectProductsDto selectProductDto = new SelectProductsDto() // problem in choosing names ? 
            {
                Id = product.Id,
                Title = product.Title,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice
            };
            selectProductsDtos.Add(selectProductDto);
        }

        return View(selectProductsDtos);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var product = await _productService.SelectByIdAsync(id);
        if (product is not null)
        {
            var selectProductDto = new SelectProductDto
            {
                Id = product.Id,
                Title = product.Title,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                Code = product.Code
            };
            return View(selectProductDto);
        }

        TempData["Index"] = "Product not found";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ServiceCreateProductDto createProductDto)
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var existingProduct = await _productService.SelectByProductCodeAsync(createProductDto.Code);
        if (ModelState.IsValid && existingProduct is null)
        {
            var serviceCreateProductDto = new ServiceCreateProductDto  /// Unreadable !!!
            {
                Title = createProductDto.Title,
                Code = createProductDto.Code,
                Quantity = createProductDto.Quantity,
                UnitPrice = createProductDto.UnitPrice
            };

            await _productService.InsertAsync(serviceCreateProductDto);
            await _productService.SaveAsync();
            TempData["Index"] = $"Product \"{serviceCreateProductDto.Title}\" created";
            return RedirectToAction(nameof(Index));
        }
        else if (ModelState.IsValid && existingProduct is not null)
        {
            TempData["Create"] = $"Product with ProductCode : {createProductDto.Code} already exist";
            return View();
        }
        else
        {
            return View();
        }
    }

    public async Task<IActionResult> Update(Guid Id)
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var product = await _productService.SelectByIdAsync(Id);
        if (product is null)
        {
            TempData["Index"] = "Product does not exist";
            return RedirectToAction(nameof(Index));
        }

        var updateProductDto = new UpdateProductDto
        {
            Id = product.Id,
            Title = product.Title,
            Quantity = product.Quantity,
            UnitPrice = product.UnitPrice,
            Code = product.Code
        };

        return View(updateProductDto);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductDto updateProductDto)//, Guid Id) what is id ? 
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        //if (Id != Product.Id) return NotFound(); // Why ???????? 
        var existingProduct = await _productService.SelectByProductCodeAsync(updateProductDto.Code);

        bool updateCondition = (existingProduct is not null && existingProduct.Id == updateProductDto.Id) ||
                                existingProduct is null;

        if (ModelState.IsValid && updateCondition)
        {
            var updatedProduct = await _productService.SelectByIdAsync(updateProductDto.Id);

            var serviceUpdateProductDto = new ServiceUpdateProductDto  /// Unreadable !!!
            {
                Id = updateProductDto.Id,
                Title = updateProductDto.Title,
                Quantity = updateProductDto.Quantity,
                UnitPrice = updateProductDto.UnitPrice,
                Code = updateProductDto.Code,
            };

            await _productService.UpdateAsync(serviceUpdateProductDto);
            await _productService.SaveAsync();
            TempData["Index"] = $"Product \"{updatedProduct.Title}\" successfully updated";
            return RedirectToAction(nameof(Index));
        }
        else if (ModelState.IsValid && !updateCondition)
        {
            TempData["Update"] = $"Product with ProductCode : {updateProductDto.Code} already exist";
            return View(updateProductDto);
        }
        else
        {
            return View();
        }
    }

    public async Task<IActionResult> Delete(Guid Id)
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var product = await _productService.SelectByIdAsync(Id);
        if (product is null)
        {
            TempData["Index"] = "Product does not exist";
            return RedirectToAction(nameof(Index));
        }
        var deleteProductDto = new DeleteProductDto // این اصن خیلی عجیبه ! شِت !!!0
        {
            Id = new Guid(), // من اینجا آیدی جدید بهش دادم ولی باز قبلی رو شناسایی می کنه پاکش میکنه !0
            Title = product.Title,
            Quantity = product.Quantity,
            UnitPrice = product.UnitPrice,
            Code = product.Code
        };
        // هر آیدی که میفرستی به ویو باز خودش آیدی که از ایندکس اومده رو پاس میده به ویو. 
        return View(deleteProductDto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteProductDto deleteProductDto)
    {
        if (_productService is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var deletedProduct = await _productService.SelectByIdAsync(deleteProductDto.Id);

        await _productService.DeleteByIdAsync(deleteProductDto.Id);
        await _productService.SaveAsync();

        TempData["Index"] = $"Product \"{deletedProduct.Title}\" deleted";
        return RedirectToAction(nameof(Index));
    }
}
