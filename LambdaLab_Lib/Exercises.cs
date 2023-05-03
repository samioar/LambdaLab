namespace LambdaLab_Lib
{
    public class Exercsises
    {
        public static int CountTotalNumberOfSpartans(List<Spartan> spartans)
        {
            return spartans.Count();
        }

        public static int CountTotalNumberOfSpartansInUKAndUSA(List<Spartan> spartans)
        {

            return spartans.Count(s => s.Country == "U.K." || s.Country == "U.S.A."); 
        }

        public static int NumberOfSpartansBornAfter1980(List<Spartan> spartans)
        {
            return spartans.Count(x => x.DateOfBirth.Year >1980);
        }

        public static int SumOfAllSpartaMarksMoreThan50Inclusive(List<Spartan> spartans)
        {
            return (int)spartans.Where(s => s.SpartaMark >= 50).Sum(x => x.SpartaMark); //where filters and sum selects
        }

        //To 2 decimal points
        public static double AverageSpartanMark(List<Spartan> spartans)
        {
            return Math.Round(spartans.Average(s => s.SpartaMark), 2); //Math.Round(input, 2) fixes
        }
          //      Expected: 68.290000000000006d
          //      But was:  68.285714285714292d (without math.round)

        public static List<Spartan> SortByAlphabeticallyByLastName(List<Spartan> spartans)
        {
            return spartans.OrderBy(s => s.LastName).ToList(); //Return type is list

        }

        public static List<string> ListAllDistinctCities(List<Spartan> spartans)
        {
            return spartans.Select(s => s.City).Distinct().ToList();
        }

        public static List<Spartan> ListAllSpartansWithIdBeginingWithB(List<Spartan> spartans)
        {
            return spartans.FindAll(s => s.SpartanId.StartsWith('B')) ;
        }
    }
}