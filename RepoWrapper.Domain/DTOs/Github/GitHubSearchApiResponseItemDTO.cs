using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoWrapper.Domain.DTOs.Github
{
    public class GitHubSearchApiResponseItemDTO
    {
        public string name { get; set; }
        public GitHubSearchApiResponseItemOwnerDTO owner { get; set; }
    }
}
