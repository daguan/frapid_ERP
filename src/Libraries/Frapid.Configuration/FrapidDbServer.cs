using System;
using System.Linq;
using Frapid.Configuration.DbServer;
using Serilog;

namespace Frapid.Configuration
{
    public static class FrapidDbServer
    {
        private static readonly IDbServer Server = GetServer();

        public static IDbServer GetServer()
        {
            string providerName = ConfigurationManager.GetConfigurationValue("DbServerConfigFileLocation", "ProviderName");

            try
            {
                var iType = typeof (IDbServer);
                var members = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => iType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                    .Select(Activator.CreateInstance);

                foreach (var member in members.Cast<IDbServer>().Where(member => member.ProviderName.Equals(providerName)))
                {
                    return member;
                }
            }
            catch (Exception ex)
            {
                Log.Error("{Exception}", ex);
                throw;
            }

            return new PostgreSQL();
        }

        public static string GetConnectionString(string database = "", string userId = "", string password = "")
        {
            return Server.GetConnectionString(database, userId, password);
        }

        public static string GetSuperUserConnectionString(string database = "")
        {
            return Server.GetSuperUserConnectionString(database);
        }

        public static string GetMetaConnectionString()
        {
            return Server.GetConnectionString();
        }

        public static string GetConnectionString(string host, string database, string username, string password,
            int port)
        {
            return Server.GetConnectionString(host, database, username, password, port);
        }

        /// <summary>
        /// Do not use this function if the any of the paramters come from user input.
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure or function.</param>
        /// <param name="parameters">List of parameters of the function</param>
        /// <returns></returns>
        public static string GetProcedureCommand(string procedureName, string[] parameters)
        {
            return Server.GetProcedureCommand(procedureName, parameters);
        }

        public static string DefaultSchemaQualify(string input)
        {
            return Server.DefaultSchemaQualify(input);
        }

        public static string AddLimit(string limit)
        {
            return Server.AddLimit(limit);
        }

        public static string AddOffset(string offset)
        {
            return Server.AddOffset(offset);
        }
    }
}