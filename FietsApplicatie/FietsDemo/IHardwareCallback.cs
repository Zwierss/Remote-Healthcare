using System.Collections.Generic;

namespace FietsDemo;

public interface IHardwareCallback
{
    void OnNewBikeData(IReadOnlyList<int> values);
    void OnNewHeartrateData(IReadOnlyList<int> values);
    void OnSuccessfulConnect();
}