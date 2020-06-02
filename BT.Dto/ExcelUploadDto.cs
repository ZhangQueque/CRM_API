using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BT.Dto
{
    public class ExcelUploadDto
    {
        public IFormFile UploadFile { get; set; }
    }
}
