using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net7.Dtos.Fight;
using net7.Services.FightService;

namespace net7.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;
        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto attack) 
        {
            return Ok(await _fightService.WeaponAttack(attack));
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttack(SkillAttackDto attack) 
        {
            return Ok(await _fightService.SkillAttack(attack));
        }
    }
}