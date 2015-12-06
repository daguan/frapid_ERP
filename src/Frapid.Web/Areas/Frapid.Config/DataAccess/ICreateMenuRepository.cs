// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Config.Entities;
namespace Frapid.Config.DataAccess
{
    public interface ICreateMenuRepository
    {

        int Sort { get; set; }
        string AppName { get; set; }
        string MenuName { get; set; }
        string Url { get; set; }
        string Icon { get; set; }
        string ParentMenuName { get; set; }

        /// <summary>
        /// Prepares and executes ICreateMenuRepository.
        /// </summary>
        int Execute();
    }
}