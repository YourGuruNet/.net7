using net7.Dtos.Fight;

namespace net7.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttack);
         Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttack);
    }
}