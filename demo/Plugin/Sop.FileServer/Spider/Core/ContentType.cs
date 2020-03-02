using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sop.FileServer.Spider.Core
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ContentType
	{
		Auto,
		Html,
		Json
	}
}