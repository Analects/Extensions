using System;
using System.Linq;
using Xunit;

namespace Analects.Extensions.Tests
{
    public partial class EnumerableExtensionsTests
    {
        public class AtMostWithCondition
        {
            [Fact]
            public void CorrectForExactAmountOfItemsEven()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtMost(5, i => i % 2 == 0);

                Assert.True(result);
            }

            [Fact]
            public void CorrectForExactAmountOfItemsOdd()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtMost(5, i => i % 2 != 0);

                Assert.True(result);
            }

            [Fact]
            public void CorrectForLessItemsEven()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtMost(4, i => i % 2 == 0);

                Assert.False(result);
            }

            [Fact]
            public void CorrectForLessItemsOdd()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtMost(4, i => i % 2 != 0);

                Assert.False(result);
            }

            [Fact]
            public void CorrectForTooManyItemsEven()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtMost(6, i => i % 2 == 0);

                Assert.True(result);
            }

            [Fact]
            public void CorrectForTooManyItemsOdd()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtMost(6, i => i % 2 != 0);

                Assert.True(result);
            }
        }
    }
}