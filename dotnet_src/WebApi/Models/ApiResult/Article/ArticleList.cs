using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.ApiResult
{
    public class ArticleListResult
    {
        public int id { get; set; }

        public long timestamp { get; set; }

        public string author { get; set; }
        public string reviewer { get; set; }

        public string title { get; set; }

        public string content_short { get; set; }

        public string content { get; set; }
        public string forecast { get; set; }
        public int importance { get; set; }
        /// <summary>
        ///   'type|1': ['CN', 'US', 'JP', 'EU'],
        /// </summary>
        public string[] type { get; set; }
        /// <summary>
        ///    'status|1': ['published', 'draft'],
        /// </summary>
        public string[] status { get; set; }
        public DateTime display_time { get; set; }
        public bool comment_disabled { get; set; }
        public int pageviews { get; set; }
        public string image_uri { get; set; }
        /// <summary>
        ///     platforms: ['a-platform']
        /// </summary>
        public int platforms { get; set; }



    }
}
