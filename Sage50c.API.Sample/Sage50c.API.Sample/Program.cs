using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Sage50c.API.Sample {
    static class Program {

        #region "Constants"

        // These directores are the default directories where the Sage50c dlls are located.
        private const string _sageDirInterops = "Program Files (x86)\\Common Files\\sage\\2070\\50c2022\\Interops\\";
        private const string _sageDirExtraOnline = "Program Files (x86)\\Common Files\\sage\\2070\\50c2022\\Extra Online\\ Files (x86)\\Common Files\\sage\\2070\\50c2022\\Interops\\";

        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(){

            ListOfDirectories.Add($@"{BaseDirectoryRoot}{_sageDirInterops}");
            ListOfDirectories.Add($@"{BaseDirectoryRoot}{_sageDirExtraOnline}");

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fApi());
        }

        #region "AssemblyResolve"

        private static string BaseDirectoryRoot { get; set; } = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory);
        private static List<string> ListOfDirectories = new List<string>();

        /// <summary>
        /// Load the assembly from the specified path.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The arguments of the event.</param>
        /// <returns>The loaded assembly.</returns>
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                // Get the name of the requested assembly.
                const string extentionType = ".dll";
                var assemblyName = new AssemblyName(args.Name).Name;

                foreach (var directory in ListOfDirectories)
                {
                    // Combine the folder path with the assembly name and ".dll" extension.
                    var assemblyPath = Path.Combine(directory, assemblyName + extentionType);

                    if (File.Exists(assemblyPath))
                    {
                        // Load the assembly from the specified path.
                        return Assembly.LoadFrom(assemblyPath);
                    }
                }
            }
            catch (Exception ex)
            {
                // This exception will be thrown if the assembly is not found in the external DLLs folder.
            }

            // Return null if the assembly is not found in the external DLLs folder.
            return null;
        }

        #endregion
    }
}
