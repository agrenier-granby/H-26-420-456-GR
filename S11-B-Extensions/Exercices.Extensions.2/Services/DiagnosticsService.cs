using Exercices.Extensions.Models;
using System.Net;
using System.Runtime.InteropServices;

namespace Exercices.Extensions.Services
{
    /// <summary>
    /// Service to fetch current runtime <see cref="Diagnostics"/>
    /// </summary>
    public class DiagnosticsService
    {
        private static readonly Diagnostics _diagnostics;

        static DiagnosticsService()
        {
            _diagnostics = new Diagnostics
            {
                OSArchitecture = RuntimeInformation.OSArchitecture.ToString(),
                OSDescription = RuntimeInformation.OSDescription,
                FrameworkDescription = RuntimeInformation.FrameworkDescription,
                HostName = Dns.GetHostName()
            };
        }

        /// <summary>
        /// Gets current runtime <see cref="Diagnostics"/>
        /// </summary>
        /// <returns>Current runtime status</returns>
        public Diagnostics GetDiagnostics()
        {
            return _diagnostics;
        }
    }

}