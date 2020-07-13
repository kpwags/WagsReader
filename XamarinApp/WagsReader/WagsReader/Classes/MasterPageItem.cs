using System;

namespace WagsReader.Classes
{
    public class MasterPageItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }

        public int Level { get; set; } = 1;

        public int UnreadCount { get; set; } = 0;

        public bool ShowUnread
        {
            get
            {
                return UnreadCount > 0;
            }
        }
    }
}
