using System;
using System.Collections.Generic;
using System.Text;

namespace OMSBlazor.Dto.Category
{
    public class UpdateCategoryDto
    {
        public string CategoryName { get; set; }

        public string? Description { get; set; }

        public string? Picture { get; set; }
    }
}
