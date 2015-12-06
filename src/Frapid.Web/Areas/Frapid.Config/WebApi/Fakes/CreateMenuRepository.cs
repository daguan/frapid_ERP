// ReSharper disable All
using System;
using System.Collections.Generic;
using Frapid.Config.DataAccess;
using Frapid.Config.Entities;

namespace Frapid.Config.Api.Fakes
{
    public class CreateMenuRepository : ICreateMenuRepository
    {
        public int Sort { get; set; }
        public string AppName { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string ParentMenuName { get; set; }

        public int Execute()
        {
            return 1;
        }
    }
}