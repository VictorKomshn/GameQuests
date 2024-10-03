using GameQuests.API.Mappers;
using GameQuests.API.ViewModel;
using GameQuests.Domain.AggregatesModel.PlayerAggregate;
using GameQuests.Domain.AggregatesModel.PlayerAggregate.Abstract;
using GameQuests.Domain.AggregatesModel.QuestAggregate.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace GameQuests.API.Controllers
{
    [Route("quest")]
    [ApiController]
    public class QuestController : Controller
    {
        private readonly IQuestService _questService;

        public QuestController(IQuestService questService)
        {
            _questService = questService ?? throw new ArgumentNullException(nameof(questService));
        }

        public async Task<IEnumerable<QuestViewModel>> GetAvailableAsync(Player player)
        {
            try
            {
                var availableQuests = await _questService.GetAvailableAsync(player);
                return availableQuests.Select(x => x.ToViewModel());
            }
            catch (Exception ex)
            {
                return Array.Empty<QuestViewModel>();
            }
        }
    }
}
