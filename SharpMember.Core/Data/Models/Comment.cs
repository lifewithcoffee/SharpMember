using System;
using System.Collections.Generic;
using System.Text;

namespace SharpMember.Core.Data.Models
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class Comment : CommentEntity
    {
        public virtual WorkTask WorkTask { get; set; }
    }
}
