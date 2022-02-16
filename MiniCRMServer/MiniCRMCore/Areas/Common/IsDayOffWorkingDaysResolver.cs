using isdayoff;
using isdayoff.Contract;
using MiniCRMCore.Areas.Common.Interfaces;
using System;
using System.Threading.Tasks;

namespace MiniCRMCore.Areas.Common
{
    public class IsDayOffWorkingDaysResolver : IWorkingDaysResolver
    {
        private IsDayOff _resolver;

        public IsDayOffWorkingDaysResolver()
        {
            var settings = IsDayOffSettings.Build
                               .UseDefaultCountry(Country.Russia)
                               .Create();
            _resolver = new IsDayOff(settings);
        }

        public async Task<DateTime> GetNextWorkDateAsync()
        {
            var date = DateTime.Now.AddDays(1);
            while (true)
            {
                if (await this.IsWorkDayAsync(date))
                    return date;
                date = date.AddDays(1);
            }
        }

        public async Task<bool> IsWorkDayAsync(DateTime date)
        {
            var todayDayOffInfo = await _resolver.CheckDayAsync(date);
            return todayDayOffInfo == DayType.WorkingDay;
        }
    }
}