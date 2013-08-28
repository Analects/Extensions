using System;
using System.Linq;
using Xunit;

namespace Analects.Extensions.Tests
{
    public partial class EnumerableExtensionsTests
    {
        public class AtMost
        {
            [Fact]
            public void CorrectForLessItems()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtMost(9);

                Assert.False(result);
            }

            [Fact]
            public void CorrectForExactAmountOfItems()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtMost(10);

                Assert.True(result);
            }

            [Fact]
            public void CorrectForTooManyItems()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtMost(11);

                Assert.True(result);
            }
        }
    }
}