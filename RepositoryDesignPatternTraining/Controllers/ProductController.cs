using Microsoft.AspNetCore.Mvc;
using RepositoryDesignPatternTraining.Controllers.Dtos.ProductDtos;
using RepositoryDesignPatternTraining.Models.DomainModels.ProductAggregates;
using RepositoryDesignPatternTraining.Models.Services.Contracts;

namespace RepositoryDesignPatternTraining.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Index()
    {
        if (_productRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var products = await _productRepository.SelectAll();

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
        if (_productRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var product = await _productRepository.SelectById(id);
        if (product is not null)
        {
            var selectProductDto = new SelectProductDto
            {
                Id = product.Id,
                Title = product.Title,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                ProductCode = product.ProductCode
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
    public async Task<IActionResult> Create(Product product)
    {
        if (_productRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var existingProduct = await _productRepository.SelectByProductCode(product.ProductCode);
        if (ModelState.IsValid && existingProduct is null)
        {
            var createdProduct = new Product
            {
                Id = new Guid(),
                Title = product.Title,
                ProductCode = product.ProductCode,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice
            };

            await _productRepository.Insert(createdProduct);
            await _productRepository.Save();
            TempData["Index"] = $"Product \"{createdProduct.Title}\" created";
            return RedirectToAction(nameof(Index));
        }
        else if (ModelState.IsValid && existingProduct is not null)
        {
            TempData["Create"] = $"Product with ProductCode : {product.ProductCode} already exist";
            return View();
        }
        else
        {
            return View();
        }
    }

    public async Task<IActionResult> Update(Guid Id)
    {
        if (_productRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var product = await _productRepository.SelectById(Id);
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
            ProductCode = product.ProductCode
        };

        return View(updateProductDto);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductDto updateProductDto)//, Guid Id) what is id ? 
    {
        if (_productRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        //if (Id != Product.Id) return NotFound(); // Why ???????? 
        var existingProduct = await _productRepository.SelectByProductCode(updateProductDto.ProductCode);
        bool updateCondition = (existingProduct is not null && existingProduct.Id == updateProductDto.Id) ||
                                existingProduct is null ?
        true : false;

        if (ModelState.IsValid && updateCondition)
        {
            var updatedProduct = await _productRepository.SelectById(updateProductDto.Id);

            updatedProduct.Title = updateProductDto.Title;
            updatedProduct.Quantity = updateProductDto.Quantity;
            updatedProduct.UnitPrice = updateProductDto.UnitPrice;
            updatedProduct.ProductCode = updateProductDto.ProductCode;

            await _productRepository.Update(updatedProduct);
            await _productRepository.Save();
            TempData["Index"] = $"Product \"{updatedProduct.Title}\" successfully updated";
            return RedirectToAction(nameof(Index));
        }
        else if (ModelState.IsValid && !updateCondition)
        {
            TempData["Update"] = $"Product with ProductCode : {updateProductDto.ProductCode} already exist";
            return View(updateProductDto);
        }
        else
        {
            return View();
        }
    }

    public async Task<IActionResult> Delete(Guid Id)
    {
        if (_productRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var product = await _productRepository.SelectById(Id);
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
            ProductCode = product.ProductCode
        };
        // هر آیدی که میفرستی به ویو باز خودش آیدی که از ایندکس اومده رو پاس میده به ویو. 
        return View(deleteProductDto);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteProductDto deleteProductDto)
    {
        if (_productRepository is null) return Problem("Entity set 'TrainingProjectDbContext.Product' is null.");

        var deletedProduct = await _productRepository.SelectById(deleteProductDto.Id);

        await _productRepository.Delete(deletedProduct);
        await _productRepository.Save();

        TempData["Index"] = $"Product \"{deletedProduct.Title}\" deleted";
        return RedirectToAction(nameof(Index));
    }
}
