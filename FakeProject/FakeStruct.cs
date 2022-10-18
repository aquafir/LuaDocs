namespace FakeProject
{
    /// <summary>
    /// Structure for coordinates
    /// </summary>
    public struct Coords
    {
        /// <summary>
        /// Constructor for coordinate struct
        /// </summary>
        /// <param name="x">X coord</param>
        /// <param name="y">Y coord</param>
        public Coords(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// X coord
        /// </summary>
        public double X { get; }
        public double Y { get; }

        /// <summary>
        /// ToString override for coordinates
        /// </summary>
        /// <returns>Formatted coordinates</returns>
        public override string ToString() => $"({X}, {Y})";
    }
}
