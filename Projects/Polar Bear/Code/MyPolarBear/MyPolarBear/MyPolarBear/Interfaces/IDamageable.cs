using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPolarBear.Interfaces
{
    interface IDamageable
    {
        void TakeDamage(int amount, String damageType, Entity source);
    }
}
