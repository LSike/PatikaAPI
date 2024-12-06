using PatikaAPI.Models;

namespace PatikaAPI.Services
{
    public class BetegsegService
    {
        public static string Delete(int id)
        {
            using (var context = new PatikaContext())
            {
                
                try
                {
                    Betegseg torlendo = context.Betegsegs.FirstOrDefault(b => b.Id == id);
                    if (torlendo is not null)
                    {
                        context.Betegsegs.Remove(torlendo);
                        context.SaveChanges();
                        return "Sikeres törlés!";
                    }
                    else
                    {
                        return "Nincs ilyen azonosítójú betegség";
                    }
                }
                catch (Exception ex)
                {
                    return $"{ex.Message}";
                }
            }
        }

        public async static void Post(Betegseg betegseg)
        {
            using (var context = new PatikaContext())
            {
                try
                {
                    if (betegseg is not null)
                    {
                        await context.Betegsegs.AddAsync(betegseg);
                        await context.SaveChangesAsync();
                        //return "Sikeres felvitel.";
                    }
                    else
                    {
                        //return "Üres adatokat kaptam!";
                    }
                }
                catch (Exception ex)
                {
                    //return $"Hiba! {ex.Message}";
                    
                }
            }
        }
    }
}
