using System;
using System.Collections.Generic;
using System.Text;

namespace Oobjs.Lesson1.Core.Entities
{
    public class Relation
    {
        public int Id { get; set; }

        public Guid ParentGuid { get; set; }

        public Guid ChildGuid { get; set; }
    }
}
