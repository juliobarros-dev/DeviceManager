using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace DeviceManager.Application.WebApi.Extensions;

[ExcludeFromCodeCoverage]
public static class AssemblyExtensions
{
	public static string GetDirectoryName(this Assembly assembly)
	{
		var location = assembly.Location;
		var directoryName = Path.GetDirectoryName(location) ?? throw new InvalidOperationException("Unable to get assembly directory name");

		return directoryName;
	}
}