using Microsoft.Identity.Client;
using Microsoft.Net.Http.Headers;

namespace WebTemplate.Models;

public class Smena
{
    [Key]
    public int Id { get; set; }
    public required string nazivSmene { get; set; }
    public TimeOnly pocetakSmene { get; set; }
    public TimeOnly krajSmene { get; set; }
    public List<VodjaSmene> ?vodjeSmene { get; set; }
}