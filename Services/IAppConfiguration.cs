using System;
using System.Collections.Generic;
using System.Text;

namespace UserSchedule.Services
{
    public interface IAppConfiguration
    {
        string DatabaseName { get; }
        string GetConnectionString();
    }

}
