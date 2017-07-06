using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models.ActivitySystem
{
    public class PollOptionEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Votes { get; set; }
    }

    public class PollOption : PollOptionEntity
    {
    }
}
