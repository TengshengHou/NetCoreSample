using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.AggergatesModel
{
    public class ProjectVisibleRule
    {
        public int ProjectId { get; set; }
        public bool Visible{ get; set; }
        public string Tags { get; set; }

    }
}
