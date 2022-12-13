using Newtonsoft.Json.Linq;

namespace AOC_2022.Day13
{
    internal class Packet : IComparable<Packet>
    {
        public Packet(JArray content)
        {
            Content = content;
        }

        public JArray Content { get; set; }

        public int CompareTo(Packet? other)
        {
            if (other == null) return 1;
            return CompareArrays(Content, other.Content);
        }

        private int CompareArrays(JArray left, JArray right)
        {
            for (var i = 0; i < left.Count; i++)
            {
                if (right.Count <= i)
                {
                    return 1;
                }
                // Case 1
                if (left[i].Type == JTokenType.Integer &&
                    right[i].Type == JTokenType.Integer)
                {
                    if ((int)left[i] == (int)right[i])
                    {
                        continue;
                    }
                    return (int)left[i] < (int)right[i] ? -1 : 1;
                }
                // Case 2
                if (left[i].Type == JTokenType.Array &&
                    right[i].Type == JTokenType.Array)
                {
                    var comparison = CompareArrays((JArray)left[i], (JArray)right[i]);
                    if (comparison == 0)
                    {
                        continue;
                    }
                    return comparison;
                }
                // Case 3
                else
                {
                    JArray leftArray;
                    JArray rightArray;
                    if (left[i].Type == JTokenType.Array)
                    {
                        leftArray = (JArray)left[i];
                    }
                    else
                    {
                        leftArray = new JArray((int)left[i]);
                    }
                    if (right[i].Type == JTokenType.Array)
                    {
                        rightArray = (JArray)right[i];
                    }
                    else
                    {
                        rightArray = new JArray((int)right[i]);

                    }
                    var comparison = CompareArrays(leftArray, rightArray);
                    if (comparison == 0)
                    {
                        continue;
                    }
                    return comparison;
                }
            }
            if (right.Count > left.Count)
            {
                return -1;
            }
            return 0;
        }
    }
}
