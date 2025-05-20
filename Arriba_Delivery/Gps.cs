namespace Arriba_Delivery;

/// <summary>
/// A function for dealing with taxicab distances and
/// locational data.
/// </summary>
static class Gps
{
    /// <summary>
    /// A function the finds the taxicab distance between two user locations
    /// </summary>
    /// <param name="location1">The first users location</param>
    /// <param name="location2">The second users location</param>
    /// <returns>The taxi cab distance in float form</returns>
    public static float GetDistance(string location1, string location2)
    {
        int[] coords1 = Array.ConvertAll(location1.Split(","), int.Parse);
        int[] coords2 = Array.ConvertAll(location2.Split(","), int.Parse);
        float distance = Math.Abs(coords1[0] - coords2[0]) + Math.Abs(coords1[1] - coords2[1]);
        return distance;
    }

    /// <summary>
    /// Returns the distance to travel between three locations. Useful when finding
    /// how long a trip will take for a deliverer.
    /// </summary>
    /// <param name="location1">The first users location, often the deliverer</param>
    /// <param name="location2">The second users location, often the restaurant/client</param>
    /// <param name="location3">The third and final location, often the destination/customer</param>
    /// <returns>The taxi cab distance in float form</returns>
    public static float TotalDistance(string location1, string location2, string location3)
    {
        return GetDistance(location1, location2) + GetDistance(location2, location3);
    }
}