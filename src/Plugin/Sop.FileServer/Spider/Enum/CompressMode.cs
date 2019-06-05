using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sop.FileServer.Spider.Core
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum CompressMode
	{
		None = 0,
		Gzip = 1,
		Lz4 = 2
	}
}
