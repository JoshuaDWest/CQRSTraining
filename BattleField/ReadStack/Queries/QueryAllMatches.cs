using System;
using System.Collections.Generic;
using System.Text;
using ReadStack.ReadModel;
using Xer.Cqrs.QueryStack;

namespace ReadStack.Queries
{
    public class QueryAllMatches : IQuery<IReadOnlyCollection<MatchReadModel>>
    {
    }
}
