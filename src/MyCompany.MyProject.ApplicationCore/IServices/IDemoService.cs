﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyCompany.MyProject.ApplicationCore.IServices
{
    public interface IDemoService
    {
        System.Threading.Tasks.Task CreateDemoAsync(string name);
        void UpdateDemo(Guid id, string name);
    }
}