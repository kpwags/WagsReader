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

        public string DisplayCount
        {
            get
            {
                if (UnreadCount > 999)
                {
                    return "999+";
                }

                return UnreadCount.ToString();
            }
        }

        public bool ShowUnread
        {
            get
            {
                return UnreadCount > 0;
            }
        }
    }
}
