﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SafeComm.Shared.Services
{
    public interface IDialogService
    {
        void ShowMessage(string title, string message);
    }
}
