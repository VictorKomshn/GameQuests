using GameQuests.Domain.SeedWork.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameQuests.Domain.AggregatesModel.PlayerAggregate.Abstract
{
    public interface IPlayerRepository : IBaseRepository<Player>
    {
    }
}
