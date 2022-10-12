using System.Collections.Generic;

namespace FietsDemo;

public interface IClientCallback
{
    void OnNewBikeData(IReadOnlyList<int> values);
    void OnNewHeartrateData(IReadOnlyList<int> values);
}