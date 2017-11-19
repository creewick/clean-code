using System;
using System.Collections;
using System.Diagnostics;
using NUnit.Framework;
using System.Collections.Generic;

namespace ControlDigit
{
    public static class ControlDigitExtensions
    {
        public static int ControlDigit(this long number)
        {
            int sum = 0;
            int factor = 1;
            do
            {
                int digit = (int) (number % 10);
                sum += factor * digit;
                factor = 4 - factor;
                number /= 10;
            } while (number > 0);

            int result = sum % 11;
            if (result == 10)
                result = 1;
            return result;
        }

        public static int ControlDigit2(this long number)
        {
            var sum = GetControlSum(number);
            return GetControlModulo(sum);
        }

        private static IEnumerable<int> GetNextDigit(long number)
        {
            yield return (int)number % 10;
            number /= 10;
        }

        private static int GetControlSum(long number)
        {
            var sum = 0;
            var factor = 1;
            while (number > 0)
            {
                sum += factor * (int)(number % 10);
                factor = factor == 1 ? 3 : 1;
                number /= 10;
            }
            return sum;
        }

        private static int GetControlModulo(int controlSum)
        {
            var modulo = controlSum % 11;
            return modulo == 10 ? 1 : modulo;
        }
    }

    [TestFixture]
    public class ControlDigitExtensions_Tests
    {
        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 2)]
        [TestCase(9, ExpectedResult = 9)]
        [TestCase(10, ExpectedResult = 3)]
        [TestCase(15, ExpectedResult = 8)]
        [TestCase(17, ExpectedResult = 1)]
        [TestCase(18, ExpectedResult = 0)]
        public int TestControlDigit(long x)
        {
            return x.ControlDigit();
        }

        [Test]
        public void CompareImplementations()
        {
            for (long i = 0; i < 100000; i++)
                Assert.AreEqual(i.ControlDigit(), i.ControlDigit2());
        }
    }

    [TestFixture]
    public class ControlDigit_PerformanceTests
    {
        [Test]
        public void TestControlDigitSpeed()
        {
            var count = 10000000;
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
                12345678L.ControlDigit();
            Console.WriteLine("Old " + sw.Elapsed);
            sw.Restart();
            for (int i = 0; i < count; i++)
                12345678L.ControlDigit2();
            Console.WriteLine("New " + sw.Elapsed);
        }
    }
}