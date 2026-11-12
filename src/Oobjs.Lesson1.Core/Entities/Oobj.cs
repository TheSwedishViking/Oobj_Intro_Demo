using System;
using System.Collections.Generic;
using System.Text;

namespace Oobjs.Lesson1.Core.Entities
{
    public class Oobj
    {
        public Guid Guid { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public string ObjectTypeName { get; set; } = string.Empty;
    }
}
