using System;
using System.Linq;
using Xunit;

namespace Analects.Extensions.Tests
{
    public partial class EnumerableExtensionsTests
    {
        public class AtLeast
        {
            [Fact]
            public void CorrectForLessItems()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtLeast(9);

                Assert.True(result);
            }

            [Fact]
            public void CorrectForExactAmountOfItems()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtLeast(10);

                Assert.True(result);
            }

            [Fact]
            public void CorrectForTooManyItems()
            {
                var source = Enumerable.Range(0, 10);

                var result = source.AtLeast(11);

                Assert.False(result);
            }
        }
    }
}