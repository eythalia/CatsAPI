namespace CatImageApi.Application.DTOs
{
    public class CatAdditionResponseDTO
    {
        public List<string> Succeed { get; set; }
        public int TotalSucceed { get; set; }
        public int TotalExisting { get; set; }
        public int TotalFailed { get; set; }

    }
}
