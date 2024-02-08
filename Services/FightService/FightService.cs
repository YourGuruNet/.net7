using net7.Dtos.Fight;

namespace net7.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _dataContext;

        public FightService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto skillAttack)
        {
            var response  = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _dataContext.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(character => character.Id == skillAttack.AttackerId);
              
                var opponent = await _dataContext.Characters
                    .FirstOrDefaultAsync(character => character.Id == skillAttack.OpponentId);
               
            if(attacker is null || opponent is null || attacker.Skills is null)
                throw new Exception("Missing data");

            var skill = attacker.Skills.FirstOrDefault(s => s.Id == skillAttack.SkillId) ?? throw new Exception("Skill not found");

            int damage = skill.Damage + new Random().Next(attacker.Intelligence);
            damage -= new Random().Next(opponent.Defeats);

            if(damage > 0)
                opponent.HitPoint -= damage;

            if(opponent.HitPoint <= attacker.HitPoint)
                response.Message = $"{opponent.Name} has bean defeated!";

              await  _dataContext.SaveChangesAsync();
              response.Data = new AttackResultDto 
              {
                Attacker = attacker.Name,
                Opponent = opponent.Name,
                AttackerHr = attacker.HitPoint,
                OpponentHr = opponent.HitPoint,
                Damage = damage
              };
              
            }
            catch (Exception ex)
            {  
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto weaponAttack)
        {    
            var response  = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _dataContext.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(character => character.Id == weaponAttack.AttackerId);
              
                var opponent = await _dataContext.Characters
                    .FirstOrDefaultAsync(character => character.Id == weaponAttack.OpponentId);
               
            if(attacker is null || opponent is null || attacker.Weapon is null)
                throw new Exception("Missing data");
            
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defeats);

            if(damage > 0)
                opponent.HitPoint -= damage;

            if(opponent.HitPoint <= attacker.HitPoint)
                response.Message = $"{opponent.Name} has bean defeated!";

              await  _dataContext.SaveChangesAsync();
              response.Data = new AttackResultDto 
              {
                Attacker = attacker.Name,
                Opponent = opponent.Name,
                AttackerHr = attacker.HitPoint,
                OpponentHr = opponent.HitPoint,
                Damage = damage
              };
              
            }
            catch (Exception ex)
            {  
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }        
    }
}