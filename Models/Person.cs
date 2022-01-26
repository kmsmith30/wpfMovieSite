namespace Kvfx.Models
{
    public class Person
    {
        public int Id { get; private set; }

        public string FirstName { get; private set; }

        public string MiddleName { get; private set; }

        public string LastName { get; private set; }

        public string Suffix { get; private set; }

        public string FullName
        {
            get
            {
                // Must have First Name
                if (!string.IsNullOrEmpty(FirstName))
                {
                    // Last Name is optional
                    if (!string.IsNullOrEmpty(LastName))
                    {
                        // Middle Name is also optional
                        if (!string.IsNullOrEmpty(MiddleName))
                        {
                            // Suffix is optional as well
                            if (!string.IsNullOrEmpty(Suffix))
                            {
                                return $"{ FirstName } { MiddleName } { LastName } { Suffix }"; // "John F. Kennedy Jr."
                            }

                            return $"{ FirstName } { MiddleName } { LastName }"; // "Philip Seymour Hoffman", "Neil Patrick Harris"
                        }

                        // Another optional Suffix
                        if (!string.IsNullOrEmpty(Suffix))
                        {
                            return $"{ FirstName } { LastName } { Suffix }"; // "Robert Downey Jr.", "Ed Bagley Jr."
                        }

                        return $"{ FirstName } { LastName }"; // "Tom Hanks", "Vince Vaughn"
                    }

                    return FirstName; // "Cher", "Seal"
                }

                return string.Empty;
            }
        }
    
        public string FileTag { get; private set; }
    }
}
