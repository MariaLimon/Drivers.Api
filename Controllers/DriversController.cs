using Microsoft.AspNetCore.Mvc;
using Drivers.Api.Services;
using Drivers.Api.Models;
using Drivers.Api.Configurations;


namespace Drivers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversController : ControllerBase
{

    private readonly ILogger<DriversController> _logger;
    private readonly DriverServices _driversServices;


    public DriversController(ILogger<DriversController> 
    logger,
    DriverServices driversServices)
    {
        _logger = logger;
        _driversServices = driversServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetDrivers(){
        var drivers = await _driversServices.GetAsync();
        return Ok(drivers);
    }

   [HttpPost]
    public async Task<IActionResult> InsertDriver([FromBody] Driver driverToInsert)
    {
        if(driverToInsert == null)
            return BadRequest();
        if(driverToInsert.Name == string.Empty)
            ModelState.AddModelError("Name","El driver no debe estar vacio");

        await _driversServices.InsertDriver(driverToInsert);

        return Created("Created",true);
    }

    [HttpDelete("ID")]
    public async Task<IActionResult> DeleteDriver(string id)
    {
        if(id == null)
            return BadRequest();
        if(id == string.Empty)
            ModelState.AddModelError("Id","No debe dejar el id vacio");

        await _driversServices.DeleteDriver(id);

        return Ok();
    }

    [HttpPut("DriverToUpdate")]
    public async Task<IActionResult> UpdateDriver(Driver driver)
    {
        if(driver == null)
            return BadRequest();
        if(driver.Id == string.Empty)
            ModelState.AddModelError("Id","No debe dejar el id vacio");
        if(driver.Name == string.Empty)
            ModelState.AddModelError("Name","No debe dejar el nombre vacio");
        if(driver.Number <= 0 )
            ModelState.AddModelError("Numbre","No debe dejar el numero vacio o con un nuemro invalido");
        if(driver.Team == string.Empty)
            ModelState.AddModelError("Team","No debe dejar el Team vacio");

        await _driversServices.UpdateDriver(driver);

        return Ok();
    }

    [HttpGet("ID")]
    public async Task<IActionResult> GetDriverById(string idToSearch)
    {
        var drivers = await _driversServices.GetDriverById(idToSearch);
        return Ok(drivers);
    }

}
 