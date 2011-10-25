using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyPolarBear.GameObjects
{
    class SeedPouch
    {
        private int mNumSeeds;
        private int mMaxSeeds;

        public SeedPouch(int maxSeeds)
        {
            mMaxSeeds = maxSeeds;
            mNumSeeds = 0;
        }

        public int HowManySeeds()
        {
            return mNumSeeds;
        }

        public int AddSeeds(int amt)
        {
            int numSeedsAdded = 0;

            if (amt < 0)
            {
                return 0;
            }

            mNumSeeds += amt;
            numSeedsAdded += amt;

            if (mNumSeeds > mMaxSeeds)
            {
                numSeedsAdded -= (mNumSeeds - mMaxSeeds);
                mNumSeeds = mMaxSeeds;
            }

            return numSeedsAdded;
        }

        public int RemoveSeeds(int amt)
        {
            int numSeedsRemoved = 0;

            if (amt < 0)
            {
                return 0;
            }

            mNumSeeds -= amt;
            numSeedsRemoved += amt;

            if (mNumSeeds < 0)
            {
                numSeedsRemoved -= (0 - mNumSeeds);
                mNumSeeds = 0;
            }

            return numSeedsRemoved;
        }
    }
}
