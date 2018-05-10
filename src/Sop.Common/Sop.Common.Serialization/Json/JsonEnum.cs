//<sopcce.com>
//--------------------------------------------------------------
//<version>V0.1</verion>
//<createdate>2018-1-23</createdate>
//<author>guojq</author>
//<email>sopcce@qq.com</email>
//<log date="2018-2-23" version="0.5">创建</log>
//--------------------------------------------------------------
//<sopcce.com>

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sop.Common.Serialization.Json
{
  public enum PropertyNameType
  {
    Default = 0,
    ToLower = 1,
    ToUpper = 2,
    CamelCase = 3,
  }

  public enum DateTimeType
  {
    Default = 0,
    //
    // 摘要:
    //     Treat as local time. If the System.DateTime object represents a Coordinated Universal
    //     Time (UTC), it is converted to the local time.
    MicrosoftDateFormatLocal = 10,
    //
    // 摘要:
    //     Treat as a UTC. If the System.DateTime object represents a local time, it is
    //     converted to a UTC.
    MicrosoftDateFormatUtc = 11,
    //
    // 摘要:
    //     Treat as a local time if a System.DateTime is being converted to a string. If
    //     a string is being converted to System.DateTime, convert to a local time if a
    //     time zone is specified.
    MicrosoftDateFormatUnspecified = 12,
    //
    // 摘要:
    //     Time zone information should be preserved when converting.
    MicrosoftDateFormatRoundtripKind = 13,
    //
    // 摘要:
    //     Treat as local time. If the System.DateTime object represents a Coordinated Universal
    //     Time (UTC), it is converted to the local time.
    IsoDateFormatLocal = 20,
    //
    // 摘要:
    //     Treat as a UTC. If the System.DateTime object represents a local time, it is
    //     converted to a UTC.
    IsoDateFormatUtc = 21,
    //
    // 摘要:
    //     Treat as a local time if a System.DateTime is being converted to a string. If
    //     a string is being converted to System.DateTime, convert to a local time if a
    //     time zone is specified.
    IsoDateFormatUnspecified = 22,
    //
    // 摘要:
    //     Time zone information should be preserved when converting.
    IsoDateFormatRoundtripKind = 23
  }

}
