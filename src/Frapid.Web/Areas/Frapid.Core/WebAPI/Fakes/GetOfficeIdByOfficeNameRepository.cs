// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Core.DataAccess;
using Frapid.Core.Entities;

namespace Frapid.Core.Api.Fakes
{
    public class GetOfficeIdByOfficeNameRepository : IGetOfficeIdByOfficeNameRepository
    {
        public string OfficeName { get; set; }

        public int Execute()
        {
            return 1;
        }
    }
}