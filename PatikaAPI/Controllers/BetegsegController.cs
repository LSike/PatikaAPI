using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatikaAPI.Models;
using System.Security.Cryptography.Xml;

namespace PatikaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BetegsegController : ControllerBase
    {
        #region Szinkron végpontok
        [HttpGet]
        public IActionResult Get()
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    List<Betegseg> result = context.Betegsegs.ToList();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    List<Betegseg> result =
                    [
                        new Betegseg
                        {
                            Id = -1,
                            Megnevezes = ex.Message
                        },
                    ];
                    return StatusCode(400,result);
                }
            }
        }

        [HttpGet("ById")]
        public IActionResult Get(int id)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    Betegseg result = context.Betegsegs.FirstOrDefault(b => b.Id == id);
                    if (result == null)
                        return NotFound("Nincs ilyen azonosítójú betegség");
                    else
                        return Ok(result);
                }
                catch (Exception ex)
                {
                    Betegseg hiba = new Betegseg
                    {
                        Id = -1,
                        Megnevezes = ex.Message
                    };
                    return StatusCode(400, hiba);
                }
            }
        }

        [HttpGet("ToGyogyszerNev")]
        public IActionResult Get(string gynev)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    List<Betegseg> result = context.Kezels.Include(k => k.Gyogyszer).Include(k => k.Betegseg).Where(k => k.Gyogyszer.Nev == gynev).Select(k => k.Betegseg).ToList();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    List<Betegseg> result = new List<Betegseg>();
                    result.Add(new Betegseg
                    {
                        Id = -1,
                        Megnevezes=ex.Message
                    });
                    return StatusCode(400, result);
                }
            }
        }

        [HttpGet("ToGyogyszerId")]
        public IActionResult GetById(int id)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    List<Betegseg> result = context.Kezels.Include(k => k.Gyogyszer).Include(k => k.Betegseg).Where(k => k.Gyogyszer.Id == id).Select(k => k.Betegseg).ToList();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    List<Betegseg> result = new List<Betegseg>();
                    result.Add(new Betegseg
                    {
                        Id = -1,
                        Megnevezes = ex.Message
                    });
                    return StatusCode(400, result);
                }
            }
        }

        [HttpPost("UjBetegseg")]
        public IActionResult Post(Betegseg ujBetegseg)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    context.Betegsegs.Add(ujBetegseg);
                    return StatusCode(200, "Sikeres rögzítés.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete("TorolBetegseg")]
        public IActionResult Delete(int id)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    Betegseg torlendo = new Betegseg()
                    {
                        Id = id
                    };
                    context.Betegsegs.Remove(torlendo);
                    context.SaveChanges();
                    return Ok("Sikeres törlés.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPut("ModositBetegseg")]
        public IActionResult Put(Betegseg ujBetegseg)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    
                    if (context.Betegsegs.Contains(ujBetegseg))
                    {
                        context.Betegsegs.Update(ujBetegseg);
                        context.SaveChanges();
                        return StatusCode(200, "Sikeres rögzítés.");
                    }
                    else
                    {
                        return NotFound("Nincs ilyen azonosítójú gyógyszer.");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
        #endregion

        #region Aszinkron végpontok
        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            using(var  context = new PatikaContext())
            {
                try
                {
                    List<Betegseg> result = await context.Betegsegs.ToListAsync();
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    List<Betegseg> result = new List<Betegseg>()
                    {
                        new Betegseg() { 
                            Id = -1,
                            Megnevezes = ex.Message
                        }
                    };
                    return BadRequest(result);
                }
            }
        }
        #endregion
    }
}
