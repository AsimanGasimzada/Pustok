using Microsoft.AspNetCore.Mvc;
using Pustok.Business.Dtos;

namespace Pustok.MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController(IHttpClientFactory _httpClientFactory) : Controller
{

    private readonly HttpClient _httpClient = _httpClientFactory.CreateClient("ApiClient");

    public async Task<IActionResult> Index()
    {
        var productResponse = await _httpClient.GetAsync("https://localhost:44342/api/Products");

        if (!productResponse.IsSuccessStatusCode)
            return BadRequest();

        var productResult = await productResponse.Content.ReadFromJsonAsync<ResultDto<List<ProductGetDto>>>() ?? new();

        return View(productResult.Data);
    }


    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteResponse = await _httpClient.DeleteAsync($"https://localhost:44342/api/Products/{id}");

        var deleteResult = await deleteResponse.Content.ReadFromJsonAsync<ResultDto>() ?? new();

        if (!deleteResult.IsSucced)
        {
            return NotFound(deleteResult.Message);
        }

        return RedirectToAction("Index");
    }


    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var updatedProduct = await _httpClient.GetAsync($"https://localhost:44342/api/Products/GetUpdateProduct/{id}") ;

        if (!updatedProduct.IsSuccessStatusCode)
            return BadRequest();

        var productResult = await updatedProduct.Content.ReadFromJsonAsync<ResultDto<ProductUpdateDto>>() ?? new();

        // Fetch categories from API
        var categoriesResponse = await _httpClient.GetAsync("https://localhost:44342/api/Categories");

        if (categoriesResponse.IsSuccessStatusCode)
        {
            var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
            ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
        }
        else
        {
            ViewBag.Categories = new List<CategoryGetDto>();
        }

        return View(productResult.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProductUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            // Reload categories on validation failure
            var categoriesResponse = await _httpClient.GetAsync("https://localhost:44342/api/Categories");
            if (categoriesResponse.IsSuccessStatusCode)
            {
                var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
                ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
            }
            return View(dto);
        }

        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(dto.Id.ToString()), "Id");
            content.Add(new StringContent(dto.Name), "Name");
            content.Add(new StringContent(dto.Description), "Description");
            content.Add(new StringContent(dto.Price.ToString()), "Price");
            content.Add(new StringContent(dto.CategoryId.ToString()), "CategoryId");

            if (dto.Image != null)
            {
                var fileContent = new StreamContent(dto.Image.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.Image.ContentType);
                content.Add(fileContent, "Image", dto.Image.FileName);
            }

            var response = await _httpClient.PutAsync($"https://localhost:44342/api/Products", content);

            if (!response.IsSuccessStatusCode)
            {
                // Reload categories on API error
                var categoriesResponse = await _httpClient.GetAsync("https://localhost:44342/api/Categories");
                if (categoriesResponse.IsSuccessStatusCode)
                {
                    var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
                    ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
                }

                var errorResultObject = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();
                ModelState.AddModelError("", errorResultObject.Message);

                return View(dto);
            }
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Create()
    {
        // Fetch categories from API
        var categoriesResponse = await _httpClient.GetAsync("https://localhost:44342/api/Categories");

        if (categoriesResponse.IsSuccessStatusCode)
        {
            var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
            ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
        }
        else
        {
            ViewBag.Categories = new List<CategoryGetDto>();
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            // Reload categories on validation failure
            var categoriesResponse = await _httpClient.GetAsync("https://localhost:44342/api/Categories");
            if (categoriesResponse.IsSuccessStatusCode)
            {
                var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
                ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
            }
            return View(dto);
        }

        using (var content = new MultipartFormDataContent())
        {
            content.Add(new StringContent(dto.Name), "Name");
            content.Add(new StringContent(dto.Description), "Description");
            content.Add(new StringContent(dto.Price.ToString()), "Price");
            content.Add(new StringContent(dto.CategoryId.ToString()), "CategoryId");

            if (dto.Image != null)
            {
                var fileContent = new StreamContent(dto.Image.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(dto.Image.ContentType);
                content.Add(fileContent, "Image", dto.Image.FileName);
            }

            var response = await _httpClient.PostAsync("https://localhost:44342/api/Products", content);

            if (!response.IsSuccessStatusCode)
            {
                // Reload categories on API error
                var categoriesResponse = await _httpClient.GetAsync("https://localhost:44342/api/Categories");
                if (categoriesResponse.IsSuccessStatusCode)
                {
                    var categoriesResult = await categoriesResponse.Content.ReadFromJsonAsync<ResultDto<List<CategoryGetDto>>>() ?? new();
                    ViewBag.Categories = categoriesResult.Data ?? new List<CategoryGetDto>();
                }

                var errorResultObject = await response.Content.ReadFromJsonAsync<ResultDto>() ?? new();

                ModelState.AddModelError("", errorResultObject.Message);

                return View(dto);
            }
        }

        return RedirectToAction("Index");
    }
}
