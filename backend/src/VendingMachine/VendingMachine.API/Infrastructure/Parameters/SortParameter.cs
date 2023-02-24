using Microsoft.IdentityModel.Tokens;

namespace VendingMachine.API.Infrastructure.Parameters
{
    public class SortParameter
    {
        #region Properties

        public bool Ascending { get; private set; }
        public string OrderBy { get; private set; }

        #endregion Properties

        #region Methods

        public static SortParameter FromString(string sortString)
        {
            if (sortString.IsNullOrEmpty())
            {
                return new SortParameter
                {
                    OrderBy = "dateCreated",
                    Ascending = false
                };
            }

            string property = sortString;
            bool ascending = false;

            if (sortString.Contains('|'))
            {
                var split = sortString.Split('|');

                if (split.Length > 2)
                {
                    throw new Exception("Sort string must be in format \"property|direction\".");
                }

                property = split[0];
                ascending = split[1] == "asc";
            }

            return new SortParameter
            {
                OrderBy = property,
                Ascending = ascending
            };
        }

        #endregion Methods
    }
}