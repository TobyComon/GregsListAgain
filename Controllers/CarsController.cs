using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using GregsListAgain.Models;
using GregsListAgain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GregsListAgain.Controllers
{
    [ApiController]

    [Route("api/[controller]")]

    public class CarsController : ControllerBase
    {
        private readonly CarsService _cs;

        public CarsController(CarsService cs)
        {
            _cs = cs;
        }

        [HttpGet]
        public ActionResult<List<Car>> Get()
        {
            try
            {
                List<Car> cars = _cs.Get();
                return Ok(cars);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Car> Get(string id)
        {
            try
            {
                Car car = _cs.Get(id);
                return Ok(car);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Car>> Create([FromBody] Car carData)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                carData.CreatorId = userInfo.Id;
                Car newCar = _cs.Create(carData);
                newCar.Creator = userInfo;
                return Ok(newCar);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Car>> Edit(int id, [FromBody] Car carData)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                carData.CreatorId = userInfo.Id;
                carData.Id = id;
                Car car = _cs.Edit(carData);
                return Ok(car);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult<String>> Delete(int id)
        {
            try
            {
                Account userInfo = await HttpContext.GetUserInfoAsync<Account>();
                _cs.Delete(id, userInfo.Id);
                return Ok("It been delongoed");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
    }
}