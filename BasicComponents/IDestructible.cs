﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtelierXNA
{
   public interface IDestructible
   {
      bool ToDestroy { get; set; }
   }
}
