using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Views.ViewModels
{
    public class MemberItemVm
    {
        public bool Selected { get; set; } = false;
        public int Id { get; set; }
        public string Name { get; set; }
        public int MemberNumber { get; set; }
        public bool Renewed { get; set; }
    }
}
