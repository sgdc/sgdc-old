using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestDemo
{
   class StaticBall : SGDE.Entity
   {
      public StaticBall(float x, float y)
         : base( x, y )
      {
         this.mPhysBaby.SetStatic(true);
      }
   }
}
