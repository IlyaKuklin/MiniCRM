using System;
using System.Threading.Tasks;

namespace MiniCRMCore.Areas.Common.Interfaces
{
    public interface IWorkingDaysResolver
    {
        public Task<bool> IsWorkDayAsync(DateTime date);
        public Task<DateTime> GetNextWorkDateAsync();
    }
}