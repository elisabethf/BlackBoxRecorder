﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Test.ObjectComparison;

namespace BlackBox.Testing
{
    public static class Extensions
    {
        public static string ToMismatchDetailsString(this IEnumerable<ObjectComparisonMismatch> mismatches)
        {
            var toStringBuilder = new StringBuilder(Environment.NewLine + Environment.NewLine);
            if(mismatches.Any())
                mismatches.ToList().ForEach(m => toStringBuilder.AppendLine(m.ToString()));
            return toStringBuilder.ToString();
        }
    }
}
