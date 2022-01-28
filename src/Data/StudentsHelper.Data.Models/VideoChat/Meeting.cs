namespace StudentsHelper.Data.Models
{
    using System;

    using StudentsHelper.Data.Common.Models;

    public class Meeting : BaseModel<string>
    {
        public Meeting()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
