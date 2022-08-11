using CatalogService.Application.Categories.Commands.CreateCategory;
using CatalogService.Application.Categories.Commands.DeleteCategory;
using CatalogService.Application.Categories.Commands.UpdateCategory;
using CatalogService.Application.Categories.Queries.GetCategories;
using CatalogService.Application.Categories.Queries.GetCategory;
using CatalogService.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.Models;

namespace WebAPI.Controllers;
/// <summary>
/// Category controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ApiControllerBase
{
    private readonly ILogger<CategoryController> _logger;
    private readonly LinkGenerator _linkGenerator;

    public CategoryController(ILogger<CategoryController> logger,
        LinkGenerator linkGenerator)
    {
        _logger = logger;
        _linkGenerator = linkGenerator;
    }
    /// <summary>
    /// Get categories
    /// </summary>
    /// <param name="categoriesQuery"></param>
    /// <returns>List of categories with links</returns>
    [Authorize(Roles = "Buyer")]
    [HttpGet]
    public async Task<IActionResult> GetCategoriesAsync([FromQuery] GetCategoriesQuery categoriesQuery)
    {
        var categories = await Mediator.Send(categoriesQuery);

        _logger.LogInformation("Returned {count} categories from database.", categories.Count);

        for (var index = 0; index < categories.Count; index++)
        {
            var categoryLinks = CreateLinksForCategory(categories[index].Id);
            categories[index].Links.AddRange(categoryLinks);
        }

        var ownersWrapper = new LinkCollectionWrapper<CategoryDto>(categories);

        return Ok(CreateLinksForCategories(ownersWrapper));
    }
    /// <summary>
    /// Get category by id
    /// </summary>
    /// <param name="id">category id</param>
    /// <returns>Category with links</returns>
    [Authorize(Roles = "Buyer")]
    [HttpGet("{id}", Name = "CategoryById")]
    public async Task<IActionResult> GetCategoryByIdAsync(int id)
    {
        var category = await Mediator.Send(new GetCategoryQuery() { Id = id });

        if (category is null)
        {
            _logger.LogError("Category with id: {categoryQueryId}, hasn't been found in db.", id);
            return NotFound();
        }

        category.Links.AddRange(CreateLinksForCategory(category.Id));

        return Ok(category);
    }
    /// <summary>
    /// Create category
    /// </summary>
    /// <param name="command">new category</param>
    /// <returns>returns count</returns>
    [Authorize(Roles = "Manager")]
    [HttpPost(Name = "CreateCategory")]
    public async Task<ActionResult<int>> CreateCategoryAsync([FromBody] CreateCategoryCommand command) => await Mediator.Send(command);

    [HttpPut("{id}", Name = "UpdateCategoryById")]
    public async Task<ActionResult> UpdateCategoryByIdAsync(int id, [FromBody]UpdateCategoryPayload command)
    {
        await Mediator.Send(new UpdateCategoryCommand() { Id = id, Image = command.Image, Name = command.Name, ParentId = command.ParentId});

        return NoContent();
    }
    /// <summary>
    /// Delete category
    /// </summary>
    /// <param name="id">category id</param>
    /// <returns></returns>
    [Authorize(Roles = "Manager")]
    [HttpDelete("{id}", Name = "DeleteCategoryById")]
    public async Task<ActionResult> DeleteCategoryByIdAsync(int id)
    {
        await Mediator.Send(new DeleteCategoryCommand(id));

        return NoContent();
    }

    private IEnumerable<Link> CreateLinksForCategory(int id)
    {
        var links = new List<Link>
        {
            new Link(_linkGenerator.GetUriByAction(
                HttpContext, 
                action: "GetCategoryById",
                controller: "Category", 
                values: new { id }
            ),
            "self",
            "GET"),

            new Link(_linkGenerator.GetUriByAction(
                HttpContext,
                action: "DeleteCategoryById",
                controller: "Category",
                values: new { id }),
            "delete_category",
            "DELETE"),

            new Link(_linkGenerator.GetUriByAction(
                HttpContext,
                action: "UpdateCategoryById",
                controller: "Category",
                values: new { id }),
            "update_category",
            "PUT"),

            new Link(_linkGenerator.GetUriByAction(
                HttpContext,
                action: "CreateCategory",
                controller: "Category",
                values: new { }),
            "create_category",
            "POST")
        };

        return links;
    }

    private LinkCollectionWrapper<CategoryDto> CreateLinksForCategories(LinkCollectionWrapper<CategoryDto> ocategoriesWrapper)
    {
        ocategoriesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(
            HttpContext,
            action: "GetCategories",
            controller: "Category",
            values: new { }),
                "self",
                "GET"));

        return ocategoriesWrapper;
    }
}
