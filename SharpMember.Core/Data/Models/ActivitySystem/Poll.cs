using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.ActivitySystem
{
    public class PollEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class Poll : PollEntity
    {
        public virtual List<PollOption> PollOptions { get; set; }
    }
}
