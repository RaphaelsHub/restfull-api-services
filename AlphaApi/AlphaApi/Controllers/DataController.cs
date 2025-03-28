using AlphaApi.Interfaces;
using AlphaApi.MicroServices;
using AlphaApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaApi.Controllers;

[ApiController]
[Route("alphaApi/[controller]")]
public class DataController(IDataService dataService, BetaApiClient alphaApiClient) : ControllerBase
{
    [HttpGet]
    public IActionResult GetData()
    {
        var data = dataService.GetAll();
        return Ok(new { Message = "Hello from AlphaApi!", Data = data });
    }

    [HttpPost]
    public IActionResult PostData([FromBody] Data data)
    {
        if (data == null || string.IsNullOrWhiteSpace(data.Name))
        {
            return BadRequest(new { Message = "Invalid data provided in AlphaApi!" });
        }

        dataService.Add(data);
        return Ok(new { Message = "Data added successfully in AlphaApi!", Data = data });
    }

    [HttpPut("{id}")]
    public IActionResult PutData(int id, [FromBody] Data updatedData)
    {
        var existingData = dataService.GetById(id);
        if (existingData == null)
        {
            return NotFound(new { Message = "Data not found in AlphaApi!" });
        }

        updatedData = updatedData with { Id = id };

        dataService.Update(updatedData);
        return Ok(new { Message = "Data updated successfully in AlphaApi!", Data = updatedData });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteData(int id)
    {
        var data = dataService.GetById(id);
        if (data == null)
        {
            return NotFound(new { Message = "Data not found in AlphaApi!" });
        }

        dataService.Delete(id);
        return Ok(new { Message = $"Data with Id {id} deleted successfully from AlphaApi!" });
    }

    [HttpGet("beta")]
    public async Task<IActionResult> GetAlphaData()
    {
        var result = await alphaApiClient.GetDataFromBetaApi();
        return Ok(result);
    }

    [HttpPost("beta")]
    public async Task<IActionResult> PostAlphaData([FromBody] Data data)
    {
        var result = await alphaApiClient.PostDataToBetaApi(data);
        return Ok(result);
    }

    [HttpPut("beta/{id}")]
    public async Task<IActionResult> PutAlphaData(int id, [FromBody] Data data)
    {
        var result = await alphaApiClient.PutDataToBetaApi(id, data);
        return Ok(result);
    }

    [HttpDelete("beta/{id}")]
    public async Task<IActionResult> DeleteAlphaData(int id)
    {
        var result = await alphaApiClient.DeleteDataFromBetaApi(id);
        return Ok(result);
    }
}