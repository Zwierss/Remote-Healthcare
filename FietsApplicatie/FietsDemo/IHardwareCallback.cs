using System.Collections.Generic;

namespace FietsDemo;

public interface IHardwareCallback
{
    /// <summary>
    /// "When new bike data is received, call the OnNewBikeData function with the values."
    /// 
    /// The OnNewBikeData function is defined in the BikeData class
    /// </summary>
    /// <param name="values">A list of integers representing the data from the bike.</param>
    void OnNewBikeData(IReadOnlyList<int> values);
    /// <summary>
    /// > This function is called when new heartrate data is available
    /// </summary>
    /// <param name="values">A list of heartrate values.</param>
    void OnNewHeartrateData(IReadOnlyList<int> values);
    /// <summary>
    /// This function is called when the client successfully connects to the server.
    /// </summary>
    void OnSuccessfulConnect();
}