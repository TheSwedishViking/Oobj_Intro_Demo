using System;
using System.Collections.Generic;
using System.Text;

namespace Oobjs.Lesson1.Core.Entities
{
    public class FieldValue
    {
        public int Id { get; set; }

        public Guid OobjGuid { get; set; }

        public string Field { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}
