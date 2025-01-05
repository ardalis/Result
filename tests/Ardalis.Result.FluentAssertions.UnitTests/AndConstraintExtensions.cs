using FluentAssertions.Primitives;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardalis.Result.FluentAssertions.UnitTests;

public static class AndConstraintExtensions
{

    public static AndConstraint<ObjectAssertions> ShouldBeOfTypeBooleanAssertionConstraint(this object obj)
    {
        return obj.Should().BeOfType(typeof(AndConstraint<BooleanAssertions>));
    }
}