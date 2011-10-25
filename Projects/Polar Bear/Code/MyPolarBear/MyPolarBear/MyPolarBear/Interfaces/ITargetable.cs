using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyPolarBear.Interfaces
{
    interface ITargetable
    {
        String GetTargetType();
        Vector2 GetPosition();
        Rectangle GetCollisionRect();
    }
}
