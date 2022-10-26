namespace DoctorLogic;

public static class Util
{
    /// <summary>
    /// It takes two byte arrays and a count, and returns a new byte array that is the concatenation of the first two
    /// arrays, with the second array truncated to the specified count
    /// </summary>
    /// <param name="b1">The first byte array to concatenate.</param>
    /// <param name="b2">The array to copy from</param>
    /// <param name="count">The number of bytes to copy from the second array.</param>
    /// <returns>
    /// A byte array
    /// </returns>
    public static byte[] Concat(byte[] b1, byte[] b2, int count)
    {
        byte[] r = new byte[b1.Length + count];
        Buffer.BlockCopy(b1, 0, r, 0, b1.Length);
        Buffer.BlockCopy(b2, 0, r, b1.Length, count);
        return r;
    }
}