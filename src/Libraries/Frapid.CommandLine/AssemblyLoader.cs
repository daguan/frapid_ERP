using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace frapid
{
    internal class AssemblyLoader
    {
        public void PreLoad()
        {
            this.AssembliesFromApplicationBaseDirectory();
        }

        void AssembliesFromApplicationBaseDirectory()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            this.AssembliesFromPath(baseDirectory);

            var privateBinPath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
            if (Directory.Exists(privateBinPath))
                this.AssembliesFromPath(privateBinPath);
        }

        void AssembliesFromPath(string path)
        {
            var assemblyFiles = Directory.GetFiles(path)
                .Where(file =>
                {
                    var extension = Path.GetExtension(file);
                    return extension != null && extension.Equals(".dll", StringComparison.OrdinalIgnoreCase);
                });

            foreach (var assemblyFile in assemblyFiles)
            {
                Assembly.LoadFrom(assemblyFile);
            }
        }
    }
}