using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DeviceManager.Application.WebApi.Utils;

public class RemoveVersionParameterFilter : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		var versionParameter = operation.Parameters?.FirstOrDefault(p => p.Name.ToLower() == "version");
		if (versionParameter != null)
			operation.Parameters.Remove(versionParameter);
	}
}