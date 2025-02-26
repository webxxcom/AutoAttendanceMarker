using Nure.NET.Types;
using Nure.NET;

namespace Automation
{
    public static class ScheduleFetcher
    {
        public static List<Event> GetClassesForGroup(string groupName)
        {
            var group = GetGroupByName(groupName);
            if (group == null)
            {
                Console.WriteLine($"Group {groupName} not found.");
                return [];
            }

            long startTime2025 = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
            long endTime = new DateTimeOffset(DateTime.MaxValue).ToUnixTimeSeconds();

            List<Event>? groupSchedule = Cist.GetEvents(
                type: EventType.Group,
                id: group.Id,
                startTime: startTime2025,
                endTime
            );

            return groupSchedule ?? [];
        }

        private static Group? GetGroupByName(string groupName)
        {
            List<Group>? groups = Cist.GetGroups();
            return groups?.Find(group => group.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
