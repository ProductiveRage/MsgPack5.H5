using Newtonsoft.Json;

namespace UnitTests
{
    internal static class ObjectComparer
    {
        public static bool AreEqual(object expected, object actual, out string messageIfNot)
        {
            // There might be a better way to do this (a testing library for that could be made to work with H5 but this should suffice for now)
            var jsonExpected = SerialiseToJson(expected);
            var jsonActual = SerialiseToJson(actual);
            if (jsonExpected == jsonActual)
            {
                messageIfNot = null;
                return true;
            }

            // 2020-06-08 DWR: There is a bug in in Bridge/H5 where the precision/interpretation of floating point numbers goes awry - this is accounted for when we read the values when decoding but if we're comparing those results to expected values that
            // are suffering from precision issues then we'll get a false negative. The way around it is to apply the same hack to the expected value (if both it and the actual are floats and if they weren't already found to match) and then compare again.
            if ((expected is float expectedFloat) && (actual is float actualFloat))
            {
                expectedFloat = float.Parse(((float)expectedFloat).ToString());
                if (expectedFloat.ToString() == actualFloat.ToString())
                {
                    messageIfNot = null;
                    return true;
                }
                messageIfNot = $"Floating point number values were too far apart - expected {expected} vs {actual}";
                return false;
            }

            messageIfNot = $"Expected {jsonExpected} but received {jsonActual}";
            return false;

            string SerialiseToJson(object value) => JsonConvert.SerializeObject(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        }
    }
}