namespace IceCity
{
    public class Owner
    {
        /// <summary>The owner's name.</summary>
        /// <remarks>
        /// <c>private set</c> means this property can only be assigned inside this class
        /// (e.g. via the constructor below). External code can read <c>owner.Name</c>
        /// but cannot write <c>owner.Name = "…"</c>. This prevents accidental modification
        /// after the object has been created.
        /// </remarks>
        public string Name { get; private set; }

        public Owner(string name)
        {
            Name = name;
        }

        // Houses must be set before use; initialized lazily after construction
        public List<House> Houses { get; set; } = null!;
    }
}
