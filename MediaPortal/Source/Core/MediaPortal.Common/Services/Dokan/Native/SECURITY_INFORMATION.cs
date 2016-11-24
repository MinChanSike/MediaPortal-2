﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPortal.Common.Services.Dokan.Native
{
  [Flags]
  internal enum SECURITY_INFORMATION : uint
  {
    OWNER_SECURITY_INFORMATION = 0x00000001,
    GROUP_SECURITY_INFORMATION = 0x00000002,
    DACL_SECURITY_INFORMATION = 0x00000004,
    SACL_SECURITY_INFORMATION = 0x00000008,
    UNPROTECTED_SACL_SECURITY_INFORMATION = 0x10000000,
    UNPROTECTED_DACL_SECURITY_INFORMATION = 0x20000000,
    PROTECTED_SACL_SECURITY_INFORMATION = 0x40000000,
    PROTECTED_DACL_SECURITY_INFORMATION = 0x80000000
  }
}