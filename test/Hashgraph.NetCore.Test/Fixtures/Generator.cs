#region Copyright (c) 2019, Jason Fabritz, All Rights Reserved
/*============================================================
 * 
 * Copyright 2019 Jason Fabritz, All Rights Reserved
 * 
 * This software is provided 'as-is' without any express or implied warranty. In no
 * event will the author be held liable for any damages arising from the use of this
 * software.
 * 
 * Permission is granted to anyone to use this software for any purpose, including
 * commercial applications, and to alter it and redistribute it freely, subject to the
 * following restrictions:
 * 
 * 1.  The origin of this software must not be misrepresented; you must not claim that
 * you wrote the original software.  If you use this software in a product, an 
 * acknowledgment (see the following) is not required, but requested:
 * 
 * Portions Copyright 2009 Jason Fabritz, All Rights Reserved
 * 
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source distribution.
 * 
 * ==========================================================*/
#endregion
using System;
using System.Text;

namespace Hashgrap.Net.Test.Fixtures
{
    public static class Generator
    {
        private static Random _random = new Random();
        private static char[] _sample = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-*&%$#@!".ToCharArray();

        public static Int32 Integer(Int32 minValueInclusive, Int32 maxValueInclusive)
        {
            return _random.Next(minValueInclusive, maxValueInclusive + 1);
        }

        public static Double Double(Double minValueInclusive, Double maxValueInclusive)
        {
            return (_random.NextDouble() * (maxValueInclusive - minValueInclusive)) + minValueInclusive;
        }

        public static String String(Int32 minLengthInclusive, Int32 maxLengthInclusive)
        {
            return Code(Integer(minLengthInclusive, maxLengthInclusive));
        }

        public static String Code(Int32 length)
        {
            var buffer = new char[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = _sample[_random.Next(0, _sample.Length)];
            }
            return new string(buffer);
        }

        public static String[] ArrayOfStrings(Int32 minCount, Int32 maxCount, Int32 minLength, Int32 maxLength)
        {
            String[] result = new String[Integer(minCount, maxCount)];
            for (Int32 index = 0; index < result.Length; index++)
            {
                result[index] = String(minLength, maxLength);
            }
            return result;
        }
    }
}
