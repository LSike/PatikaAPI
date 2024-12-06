namespace PatikaAPI.DTOs
{
    /// <summary>
    /// A gyógyszerek nevét és hatóanyagát tartalmazó osztály
    /// </summary>
    public class GyogyszerNevHatoanyagDTO
    {
        public int Id { get; set; }

        public string Nev { get; set; } = null!;

        public string Hatoanyag { get; set; } = null!;
    }
}
