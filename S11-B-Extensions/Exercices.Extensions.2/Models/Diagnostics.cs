namespace Exercices.Extensions.Models
{
    public class Diagnostics
    {
        public string OSArchitecture { get; internal set; } = default!;
        public string OSDescription { get; internal set; } = default!;
        public string FrameworkDescription { get; internal set; } = default!;
        public string HostName { get; internal set; } = default!;
    }
}