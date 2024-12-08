namespace RepoWrapper.Domain.DTOs.Github
{
    public class GitHubSearchApiResponseItemDTO
    {
        public string name { get; set; }
        public GitHubSearchApiResponseItemOwnerDTO owner { get; set; }
    }
}
