using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;
using System;

namespace Salon.Application.Tests.Extensions
{
    public static class FluentAssertionExtensions
    {
        public static void HaveEquivalentMembers<TExpectation>(this ObjectAssertions obj, TExpectation expectation)
        {
            obj.BeEquivalentTo(expectation, (EquivalencyAssertionOptions<TExpectation> opt) => opt.ComparingByMembers<TExpectation>(), "");
        }

        public static void HaveEquivalentMembers<TExpectation>(this ObjectAssertions obj, TExpectation expectation, Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config, string because = "", params object[] becauseArgs)
        {
            obj.BeEquivalentTo(expectation, (EquivalencyAssertionOptions<TExpectation> opt) => config(opt.ComparingByMembers<TExpectation>()), because, becauseArgs);
        }
    }
}
