namespace IceCity
{
    public class Owner
    {
        public string Name { get; set; }

        public Owner(string name)
        {
            Name = name;
        }

        // WHY CHANGED: "houses" (camelCase) → "Houses" (PascalCase) — C# property convention.
        // Also initialized to an empty list instead of null! to prevent accidental null errors.
        // SUGGESTED FEATURE: Add address/city field per house for multi-property billing.
        public List<House> Houses { get; set; } = new();
    }
}
