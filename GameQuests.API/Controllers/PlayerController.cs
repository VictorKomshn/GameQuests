using GameQuests.API.Mappers;
using GameQuests.API.ViewModel;
using GameQuests.Domain.AggregatesModel.EnemyAggregate;
using GameQuests.Domain.AggregatesModel.ItemAggregate;
using GameQuests.Domain.AggregatesModel.PlayerAggregate;
using GameQuests.Domain.AggregatesModel.PlayerAggregate.Abstract;
using GameQuests.Domain.SeedWork;
using Microsoft.AspNetCore.Mvc;

namespace GameQuests.API.Controllers
{
    [Route("player")]
    [ApiController]
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService ?? throw new ArgumentNullException(nameof(playerService));
        }

        [Route("moveto")]
        public async Task<IEnumerable<PlayerQuestViewModel?>?> MoveToAsync(Player player, Position position)
        {
            try
            {
                var recievedQuests = await _playerService.MoveToAsync(player, position);

                return recievedQuests?.Select(x => x.ToViewModel());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("kill")]
        public async Task<IEnumerable<PlayerQuestViewModel>?> KillEnemyAsync(Player player, Enemy enemy)
        {
            try
            {
                var recievedQuests = await _playerService.KillEnemyAsync(player, enemy);
                return recievedQuests?.Select(x => x.ToViewModel());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("collect")]
        public async Task<IEnumerable<PlayerQuestViewModel>?> CollectItemAsync(Player player, Item item)
        {
            try
            {
                var recievedQuests = await _playerService.CollectItemAsync(player, item);
                return recievedQuests?.Select(x => x.ToViewModel());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("drop")]
        public async Task<IEnumerable<PlayerQuestViewModel>?> DropItemAsync(Player player, Item item)
        {
            try
            {
                var recievedQuests = await _playerService.DropItemAsync(player, item);
                return recievedQuests?.Select(x => x.ToViewModel());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
