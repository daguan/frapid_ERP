// ReSharper disable All
using System;
using System.Collections.Generic;
using System.Dynamic;
using Frapid.NPoco;
using Frapid.Config.Entities;
namespace Frapid.Config.DataAccess
{
    public interface ICreateAppRepository
    {

        string AppName { get; set; }
        string Name { get; set; }
        string VersionNumber { get; set; }
        string Publisher { get; set; }
        DateTime PublishedOn { get; set; }
        string Icon { get; set; }
        string LandingUrl { get; set; }
        string[] Dependencies { get; set; }

        /// <summary>
        /// Prepares and executes ICreateAppRepository.
        /// </summary>
        void Execute();
    }
}