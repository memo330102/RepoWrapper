namespace RepoWrapper.Domain.DTOs.Github
{
    public class GitHubSearchApiResponseDTO
    {
        public int total_count { get; set; }
        public List<GitHubSearchApiResponseItemDTO> items { get; set; }
    }
}
